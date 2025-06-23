// This source code is based on https://justsimplycode.com/2020/12/06/auto-generate-builders-using-source-generator-in-net-5
using System.Text;
using FluentBuilderGenerator.Extensions;
using FluentBuilderGenerator.Helpers;
using FluentBuilderGenerator.Models;
using FluentBuilderGenerator.SyntaxReceiver;
using FluentBuilderGenerator.Types;
using FluentBuilderGenerator.Wrappers;
using Microsoft.CodeAnalysis;

namespace FluentBuilderGenerator.FileGenerators;

internal partial class FluentBuilderClassesGenerator : IFilesGenerator
{
    private static readonly string[] FluentBuilderIgnoreAttributeClassNames =
    {
        "FluentBuilder.FluentBuilderIgnoreAttribute",
        "FluentBuilderIgnoreAttribute",
        "FluentBuilder.FluentBuilderIgnore",
        "FluentBuilderIgnore"
    };

    private static readonly string[] SystemUsings =
    {
        "System",
        "System.Collections",
        "System.Collections.Generic",
        "System.Reflection"
    };

    private static readonly FileDataType[] ExtraBuilders =
    {
        FileDataType.ArrayBuilder,
        FileDataType.ICollectionBuilder,
        FileDataType.IEnumerableBuilder,
        FileDataType.IListBuilder,
        FileDataType.IReadOnlyCollectionBuilder,
        FileDataType.IReadOnlyListBuilder
    };

    private readonly IGeneratorExecutionContextWrapper _context;
    private readonly IAutoGenerateBuilderSyntaxReceiver _receiver;

    public FluentBuilderClassesGenerator(IGeneratorExecutionContextWrapper context, IAutoGenerateBuilderSyntaxReceiver receiver)
    {
        _context = context;
        _receiver = receiver;
    }

    public IReadOnlyList<FileData> GenerateFiles()
    {
        var applicableClassSymbols = GetClassSymbols();
        var extraClassSymbols = applicableClassSymbols.Select(x => x.ClassSymbol).ToList();

        var classes = applicableClassSymbols.Select(classSymbol => new FileData
        (
            FileDataType.Builder,
            $"{classSymbol.ClassSymbol.FullBuilderClassName.ToSafeClassName()}.g.cs",
            CreateClassBuilderCode(classSymbol.FluentData, classSymbol.ClassSymbol, extraClassSymbols)
        ));

        // Extra
        var extra = extraClassSymbols
            .Where(e => ExtraBuilders.Contains(e.Type))
            .OrderBy(e => e.Type)
            .Select(classSymbol => new FileData
            (
                classSymbol.Type,
                $"{classSymbol.NamedTypeSymbol.GenerateFileName()}_{classSymbol.Type}.g.cs",
                CreateIEnumerableBuilderCode(classSymbol)
            ));

        return classes.Union(extra).ToList();
    }

    private string CreateClassBuilderCode(FluentData fluentData, ClassSymbol classSymbol, List<ClassSymbol> allClassSymbols)
    {
        var publicConstructors = classSymbol.NamedTypeSymbol.Constructors
            .Where(c => c.DeclaredAccessibility == Accessibility.Public)
            .OrderBy(c => c.Parameters.Length)
            .ToArray();
        if (!publicConstructors.Any())
        {
            throw new NotSupportedException($"Unable to generate a FluentBuilder for the class '{classSymbol.NamedTypeSymbol}' because no public constructor is defined.");
        }

        var constructorCode = GenerateUsingConstructorCode(classSymbol, publicConstructors);

        var propertiesCode = GenerateWithPropertyCode(fluentData, classSymbol, allClassSymbols);

        var usings = SystemUsings.ToList();
        usings.Add($"{_context.AssemblyName}.FluentBuilder");
        usings.Add($"{classSymbol.NamedTypeSymbol.ContainingNamespace}");
        usings.AddRange(propertiesCode.ExtraUsings);

        var usingsAsStrings = string.Join("\r\n", usings.Distinct().Select(u => $"using {u};"));

        return
            $$"""
              {{Header.Text}}

              {{(_context.SupportsNullable ? "#nullable enable" : string.Empty)}}
              {{usingsAsStrings}}

              namespace {{classSymbol.BuilderNamespace}}
              {
                  public static partial class {{classSymbol.BuilderClassName.ToSafeClassName()}}Extensions
                  {
                      {{fluentData.ClassModifier}} static {{classSymbol.BuilderClassName}} AsBuilder{{classSymbol.NamedTypeSymbol.GetTypeArguments()}}(this {{classSymbol.NamedTypeSymbol}} instance){{classSymbol.NamedTypeSymbol.GetWhereStatement()}}
                      {
                          return new {{classSymbol.BuilderClassName}}().UsingInstance(instance);
                      }
                  }
              
                  {{fluentData.ClassModifier}} partial class {{classSymbol.BuilderClassName}} : Builder<{{classSymbol.NamedTypeSymbol}}>{{classSymbol.NamedTypeSymbol.GetWhereStatement()}}
                  {
              {{propertiesCode.StringBuilder}}
              {{constructorCode.StringBuilder}}
              {{GenerateSeveralMethods(fluentData, classSymbol)}}
                  }
              }
              {{(_context.SupportsNullable ? "#nullable disable" : string.Empty)}}
              """;
    }

    private static (StringBuilder StringBuilder, IReadOnlyList<string> ExtraUsings) GenerateUsingConstructorCode(
        ClassSymbol classSymbol,
        IReadOnlyList<IMethodSymbol> publicConstructors
    )
    {
        var builderClassName = classSymbol.BuilderClassName;

        var extraUsings = new List<string>();

        var sb = new StringBuilder();
        foreach (var publicConstructor in publicConstructors)
        {
            var constructorParameters = GetConstructorParameters(publicConstructor);

            var constructorParametersAsString = string.Join(", ", constructorParameters.Select(x => MethodParameterBuilder.Build(x.Symbol, x.Type)));
            var constructorHashCode = publicConstructor.GetDeterministicHashCodeAsString();

            sb.AppendLine(8, $"private bool _Constructor{constructorHashCode}_IsSet;");

            var defaultValues = new List<string>();
            foreach (var p in constructorParameters)
            {
                var (defaultValue, extraUsingsFromDefaultValue) = DefaultValueHelper.GetDefaultValue(p.Symbol, p.Symbol.Type);
                if (extraUsingsFromDefaultValue != null)
                {
                    extraUsings.AddRange(extraUsingsFromDefaultValue);
                }

                defaultValues.Add(defaultValue);
            }

            sb.AppendLine(8, $"private Lazy<{classSymbol.NamedTypeSymbol}> _Constructor{constructorHashCode} = new Lazy<{classSymbol.NamedTypeSymbol}>(() => new {classSymbol.NamedTypeSymbol}({string.Join(",", defaultValues)}));");

            sb.AppendLine(8, $"public {builderClassName} UsingConstructor({constructorParametersAsString})");
            sb.AppendLine(8, @"{");

            sb.AppendLine(8, $"    _Constructor{constructorHashCode} = new Lazy<{classSymbol.NamedTypeSymbol}>(() =>");
            sb.AppendLine(8, @"    {");

            sb.AppendLine(8, $"        return new {classSymbol.NamedTypeSymbol}");
            sb.AppendLine(8, @"        (");
            sb.AppendLines(20, constructorParameters.Select(x => x.Symbol.Name), ", ");
            sb.AppendLine(8, @"        );");

            sb.AppendLine(8, @"    });");

            sb.AppendLine(8, $"    _Constructor{constructorHashCode}_IsSet = true;");

            sb.AppendLine();
            sb.AppendLine(8, @"    return this;");
            sb.AppendLine(8, @"}");

            sb.AppendLine();
        }

        return (sb, extraUsings.Distinct().ToList());
    }

    private static List<(IParameterSymbol Symbol, string Type)> GetConstructorParameters(IMethodSymbol publicConstructor)
    {
        var constructorParameters = new List<(IParameterSymbol Symbol, string Type)>();

        foreach (var parameter in publicConstructor.Parameters)
        {
            // Use "params" in case it's an Array, else just use type-T.
            var type = parameter.Type.GetFluentTypeKind() == FluentTypeKind.Array ? $"params {parameter.Type}" : parameter.Type.ToString();
            constructorParameters.Add((parameter, type));
        }

        return constructorParameters;
    }

    private (StringBuilder StringBuilder, IReadOnlyList<string> ExtraUsings) GenerateWithPropertyCode(
        FluentData fluentData,
        ClassSymbol classSymbol,
        List<ClassSymbol> allClassSymbols)
    {
        var builderClassName = classSymbol.BuilderClassName;

        var (propertiesPublicSettable, propertiesPrivateSettable) = GetProperties(classSymbol, fluentData.HandleBaseClasses, fluentData.Accessibility);

        var extraUsings = new List<string>();

        var sb = new StringBuilder();
        foreach (var property in propertiesPublicSettable.Union(propertiesPrivateSettable))
        {
            var initOnly = property.IsInitOnly();

            // Use "params" in case it's an Array, else just use type-T.
            var type = property.Type.GetFluentTypeKind() == FluentTypeKind.Array ? $"params {property.Type}" : property.Type.ToString();

            var (defaultValue, extraUsingsFromDefaultValue) = DefaultValueHelper.GetDefaultValue(property, property.Type);
            if (extraUsingsFromDefaultValue != null)
            {
                extraUsings.AddRange(extraUsingsFromDefaultValue);
            }

            sb.AppendLine(!initOnly, $"        private bool _{CamelCase(property.Name)}IsSet;");
        
            sb.AppendLine($"        private Lazy<{property.Type}> _{CamelCase(property.Name)} = new Lazy<{property.Type}>(() => {defaultValue});");

            sb.AppendLine($"        public {builderClassName} With{property.Name}({type} value) => With{property.Name}(() => value);");

            sb.Append(GenerateWithPropertyFuncMethod(classSymbol, property));

            sb.Append(GeneratePropertyActionMethodIfApplicable(classSymbol, property, allClassSymbols));

            if (fluentData.Methods == FluentBuilderMethods.WithAndWithout)
            {
                sb.AppendLine($"        public {builderClassName} Without{property.Name}()");
                sb.AppendLine("        {");
                sb.AppendLine($"            With{property.Name}(() => {defaultValue});");



                sb.AppendLine(!initOnly, $"            _{CamelCase(property.Name)}IsSet = false;");
                sb.AppendLine("            return this;");
                sb.AppendLine("        }");
                sb.AppendLine();
            }
        }

        return (sb, extraUsings.Distinct().ToList());
    }

    private StringBuilder GeneratePropertyActionMethodIfApplicable(
        ClassSymbol classSymbol,
        IPropertySymbol property,
        List<ClassSymbol> allClassSymbols)
    {
        var existingClassSymbol = allClassSymbols.FirstOrDefault(c => c.NamedTypeSymbol.Name == property.Type.Name);
        if (existingClassSymbol is not null)
        {
            return GenerateWithPropertyActionMethod(classSymbol, existingClassSymbol, property);
        }

        if (property.TryGetIDictionaryElementTypes(out var dictionaryTypes))
        {
            return GenerateWithIDictionaryBuilderActionMethod(classSymbol, property, dictionaryTypes);
        }

        if (property.TryGetIEnumerableElementType(out var elementType, out var kind))
        {
            return GenerateWithIEnumerableBuilderActionMethod(kind, classSymbol, property, elementType, allClassSymbols);
        }

        return new StringBuilder();
    }

    private static StringBuilder GenerateWithPropertyFuncMethod(ClassSymbol classSymbol, IPropertySymbol property)
    {
        var className = classSymbol.BuilderClassName;

        return new StringBuilder()
            .AppendLine($"        public {className} With{property.Name}(Func<{property.Type}> func)")
            .AppendLine("        {")
            .AppendLine($"            _{CamelCase(property.Name)} = new Lazy<{property.Type}>(func);")
            .AppendLine(!property.IsInitOnly(), $"            _{CamelCase(property.Name)}IsSet = true;")
            .AppendLine("            return this;")
            .AppendLine("        }");
    }

    private static StringBuilder GenerateWithPropertyActionMethod(ClassSymbol classSymbol, ClassSymbol propertyClassSymbol, IPropertySymbol property)
    {
        var className = classSymbol.BuilderClassName;
        var builderName = propertyClassSymbol.FullBuilderClassName;

        // Replace MyAddressBuilder<T> by MyAddressBuilder<short>
        if (property.Type is INamedTypeSymbol propertyNamedType && builderName.TryGetGenericTypeArguments(out var genericTypeArgumentValue))
        {
            var list = propertyNamedType.TypeArguments.Select(t => t.ToString());
            builderName = builderName.Replace($"<{genericTypeArgumentValue}>", $"<{string.Join(", ", list)}>");
        }

        return new StringBuilder()
            .AppendLine($"        public {className} With{property.Name}(Action<{builderName}> action, bool useObjectInitializer = true) => With{property.Name}(() =>")
            .AppendLine(@"        {")
            .AppendLine($"            var builder = new {builderName}();")
            .AppendLine(@"            action(builder);")
            .AppendLine(@"            return builder.Build(useObjectInitializer);")
            .AppendLine(@"        });");
    }

    private StringBuilder GenerateWithIEnumerableBuilderActionMethod(
        FluentTypeKind kind,
        ClassSymbol classSymbol,
        IPropertySymbol property,
        INamedTypeSymbol? typeSymbol,
        List<ClassSymbol> allClassSymbols)
    {
        var className = classSymbol.BuilderClassName;
        var typeSymbolClassName = typeSymbol?.GenerateShortTypeName();
        var existingClassSymbol = allClassSymbols.FirstOrDefault(c => c.NamedTypeSymbol.Name == typeSymbolClassName);

        string fullBuilderName;
        if (existingClassSymbol != null && typeSymbol != null)
        {
            var shortBuilderName = $"{kind}{typeSymbol.GenerateShortTypeName(true)}";
            fullBuilderName = $"{typeSymbol.ContainingNamespace}.{shortBuilderName}";

            if (allClassSymbols.All(cs => cs.NamedTypeSymbol.Name != shortBuilderName))
            {
                string itemBuilderFullName;

                if (existingClassSymbol.FluentData.BuilderType == BuilderType.Custom)
                {
                    itemBuilderFullName = existingClassSymbol.FluentData.FullBuilderClassName;
                }
                else
                {
                    itemBuilderFullName = $"{typeSymbol.GenerateFullTypeName(true)}";
                }

                var fileDataType = kind.ToFileDataType();
                allClassSymbols.Add(new ClassSymbol
                {
                    Type = fileDataType,
                    FluentData = new FluentData
                    {
                        BuilderType = BuilderType.Extra,
                        Namespace = typeSymbol.ContainingNamespace.ToString(),
                        ShortBuilderClassName = shortBuilderName,
                        FullBuilderClassName = fullBuilderName
                    },
                    //BuilderNamespace = typeSymbol.ContainingNamespace.ToString(),
                    //BuilderClassName = shortBuilderName,
                    //FullBuilderClassName = fullBuilderName,
                    NamedTypeSymbol = typeSymbol,
                    ItemBuilderFullName = itemBuilderFullName
                });
            }
        }
        else
        {
            // Normal
            fullBuilderName = $"{_context.AssemblyName}.FluentBuilder.{kind}Builder{(typeSymbol == null ? string.Empty : "<" + typeSymbol.GenerateFullTypeName() + ">")}";
        }

        // If the property.Type is an interface or array, no cast is needed. Else cast the interface to the real type.
        var cast = property.Type.TypeKind is TypeKind.Interface or TypeKind.Array ?
            string.Empty :
            $"({property.Type}) ";

        return new StringBuilder()
            .AppendLine($"        public {className} With{property.Name}(Action<{fullBuilderName}> action, bool useObjectInitializer = true) => With{property.Name}(() =>")
            .AppendLine(@"        {")
            .AppendLine($"            var builder = new {fullBuilderName}();")
            .AppendLine(@"            action(builder);")
            .AppendLine($"            return {cast}builder.Build(useObjectInitializer);")
            .AppendLine(@"        });");
    }

    private static (IReadOnlyList<IPropertySymbol> PublicSettable, IReadOnlyList<IPropertySymbol> PrivateSettable) GetProperties(ClassSymbol classSymbol, bool handleBaseClasses, FluentBuilderAccessibility accessibility)
    {
        var properties = classSymbol.NamedTypeSymbol.GetMembers().OfType<IPropertySymbol>()
            .Where(x => x.SetMethod is not null)
            .Where(x => x.CanBeReferencedByName)
            .Where(x => !x.GetAttributes().Any(a => FluentBuilderIgnoreAttributeClassNames.Contains(a.AttributeClass?.GetFullType())))
            .ToList();

        var propertyNames = properties.Select(x => x.Name);

        if (handleBaseClasses)
        {
            var baseType = classSymbol.NamedTypeSymbol.BaseType;

            while (baseType != null)
            {
                properties.AddRange(baseType.GetMembers().OfType<IPropertySymbol>()
                    .Where(x => x.CanBeReferencedByName)
                    .Where(x => x.SetMethod is not null)
                    .Where(x => !propertyNames.Contains(x.Name)));

                baseType = baseType.BaseType;
            }
        }

        var propertiesPublicSettable = properties.Where(p => p.IsPublicSettable()).ToArray();
        var propertiesPrivateSettable = accessibility == FluentBuilderAccessibility.PublicAndPrivate ? properties.Where(p => p.IsPrivateSettable()).ToArray() : [];

        return (propertiesPublicSettable, propertiesPrivateSettable);
    }

    private static string GenerateSeveralMethods(FluentData fluentData, ClassSymbol classSymbol)
    {
        var publicConstructors = classSymbol.NamedTypeSymbol.Constructors.Where(c => c.DeclaredAccessibility == Accessibility.Public).ToArray();
        var (propertiesPublicSettable, propertiesPrivateSettable) = GetProperties(classSymbol, fluentData.HandleBaseClasses, fluentData.Accessibility);
        var className = classSymbol.NamedTypeSymbol.GenerateShortTypeName();

        var output = new StringBuilder();

        foreach (var property in propertiesPrivateSettable)
        {
            BuildPrivateSetMethod(output, className, property);
        }

        var hasParameterLessConstructor = publicConstructors.Any(p => p.Parameters.IsEmpty);

        output.AppendLine(8, $"public {classSymbol.BuilderClassName} UsingInstance({className} value) => UsingInstance(() => value);");
        output.AppendLine();

        output.AppendLine(8, $"public {classSymbol.BuilderClassName} UsingInstance(Func<{className}> func)");
        output.AppendLine(8, @"{");
        output.AppendLine(8, $"    Instance = new Lazy<{className}>(func);");
        output.AppendLine(8, @"    return this;");
        output.AppendLine(8, @"}");
        output.AppendLine();

        output.AppendLine(8, $"public override {className} Build() => Build({hasParameterLessConstructor.ToString().ToLowerInvariant()});");
        output.AppendLine();

        output.AppendLine(8, $"public override {className} Build(bool useObjectInitializer)");
        output.AppendLine(8, @"{");

        output.AppendLine(8, @"    if (Instance is null)");
        output.AppendLine(8, @"    {");
        output.AppendLine(8, $"        Instance = new Lazy<{className}>(() =>");
        output.AppendLine(8, @"        {");

        output.AppendLine(20, $"{className} instance;");

        output.AppendLine(20, @"if (useObjectInitializer)");
        output.AppendLine(20, @"{");

        if (!hasParameterLessConstructor)
        {
            output.AppendLine(20, $"    throw new NotSupportedException(\"Unable to use the ObjectInitializer for the class '{classSymbol.NamedTypeSymbol}' because no public parameterless constructor is defined.\");");
        }
        else
        {
            output.AppendLine(20, $"    instance = new {className}");
            output.AppendLine(20, @"    {");
            output.AppendLines(20, propertiesPublicSettable.Select(property => $@"        {property.Name} = _{CamelCase(property.Name)}.Value"), ",");
            output.AppendLine(20, @"    };");

            output.AppendLines(20, propertiesPrivateSettable.Select(property => $@"    if (_{CamelCase(property.Name)}IsSet) {{ Set{property.Name}(instance, _{CamelCase(property.Name)}.Value); }}"));

            output.AppendLine(20, @"    return instance;");
        }

        output.AppendLine(20, @"}");
        output.AppendLine();

        foreach (var x in publicConstructors.Select((publicConstructor, idx) => new { publicConstructor, idx }))
        {
            var constructorHashCode = x.publicConstructor.GetDeterministicHashCodeAsString();

            output.AppendLine(20, $"{(x.idx > 0).IIf("else ")}if (_Constructor{constructorHashCode}_IsSet) {{ instance = _Constructor{constructorHashCode}.Value; }}");
        }
        output.AppendLine(20, "else { instance = Default(); }");
        output.AppendLine();
        
        output.AppendLine(20, "return instance;");

        output.AppendLine(8, @"        });");
        output.AppendLine(8, @"    }");

        output.AppendLine();
        output.AppendLines(12, propertiesPublicSettable.Where(p => !p.IsInitOnly()).Select(property => $"if (_{CamelCase(property.Name)}IsSet) {{ Instance.Value.{property.Name} = _{CamelCase(property.Name)}.Value; }}"));
        output.AppendLines(12, propertiesPrivateSettable.Where(p => !p.IsInitOnly()).Select(property => $"if (_{CamelCase(property.Name)}IsSet) {{ Set{property.Name}(Instance.Value, _{CamelCase(property.Name)}.Value); }}"));
        
        output.AppendLine(8, @"    PostBuild(Instance.Value);");

        output.AppendLine();
        output.AppendLine(8, @"    return Instance.Value;");
        output.AppendLine(8, @"}");

        output.AppendLine();

        var defaultValues = new List<string>();
        foreach (var p in GetConstructorParameters(publicConstructors.First()))
        {
            var (defaultValue, _) = DefaultValueHelper.GetDefaultValue(p.Symbol, p.Symbol.Type);
            defaultValues.Add(defaultValue);
        }
        output.AppendLine(8, $"public static {className} Default() => new {className}({string.Join(", ", defaultValues)});");

        return output.ToString();
    }

    private static void BuildPrivateSetMethod(StringBuilder output, string className, IPropertySymbol property)
    {
        output.AppendLine(8, $"private void Set{property.Name}({className} instance, {property.Type} value)");
        output.AppendLine(8, @"{");
        output.AppendLine(8, $"    InstanceType.GetProperty(\"{property.Name}\")?.SetValue(instance, value);");
        output.AppendLine(8, @"}");
        output.AppendLine();
    }

    private IReadOnlyList<(ClassSymbol ClassSymbol, FluentData FluentData)> GetClassSymbols()
    {
        var classSymbols = new List<(ClassSymbol ClassSymbol, FluentData FluentData)>();
        foreach (var fluentDataItem in _receiver.CandidateFluentDataItems)
        {
            if (_context.TryGetNamedTypeSymbolByFullMetadataName(fluentDataItem, out var classSymbol))
            {
                classSymbols.Add((classSymbol, fluentDataItem));
            }
        }

        return classSymbols;
    }

    private static string CamelCase(string value) => $"{value.Substring(0, 1).ToLowerInvariant()}{value.Substring(1)}";
}