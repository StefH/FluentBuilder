// This source code is based on https://justsimplycode.com/2020/12/06/auto-generate-builders-using-source-generator-in-net-5
using System.Text;
using FluentBuilderGenerator.Extensions;
using FluentBuilderGenerator.Models;
using FluentBuilderGenerator.SyntaxReceiver;
using FluentBuilderGenerator.Types;
using FluentBuilderGenerator.Wrappers;
using Microsoft.CodeAnalysis;

namespace FluentBuilderGenerator.FileGenerators;

internal partial class FluentBuilderClassesGenerator : IFilesGenerator
{
    private static readonly FileDataType[] ExtraBuilders =
    {
        FileDataType.ArrayBuilder,
        FileDataType.IEnumerableBuilder,
        FileDataType.IListBuilder,
        FileDataType.ICollectionBuilder
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
        var extraClassSymbols = applicableClassSymbols.ToList();

        var classes = applicableClassSymbols.Select(classSymbol => new FileData
        (
            FileDataType.Builder,
            $"{classSymbol.FullBuilderClassName.Replace('<', '_').Replace('>', '_')}.g.cs",
            CreateClassBuilderCode(classSymbol, extraClassSymbols)
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

    private string CreateClassBuilderCode(ClassSymbol classSymbol, List<ClassSymbol> allClassSymbols)
    {
        return $@"//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by https://github.com/StefH/FluentBuilder.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

{(_context.SupportsNullable ? "#nullable enable" : string.Empty)}
using System;
using System.Collections;
using System.Collections.Generic;
using FluentBuilder;
using {classSymbol.NamedTypeSymbol.ContainingNamespace};

namespace {classSymbol.BuilderNamespace}
{{
    public partial class {classSymbol.BuilderClassName} : Builder<{classSymbol.NamedTypeSymbol}>{classSymbol.NamedTypeSymbol.GetWhereStatement()}
    {{
{GenerateWithPropertyCode(classSymbol, allClassSymbols)}
{GenerateBuildMethod(classSymbol)}
    }}
}}
{(_context.SupportsNullable ? "#nullable disable" : string.Empty)}";
    }

    private StringBuilder GenerateWithPropertyCode(ClassSymbol classSymbol, List<ClassSymbol> allClassSymbols)
    {
        var properties = GetProperties(classSymbol);
        var className = classSymbol.BuilderClassName;

        var sb = new StringBuilder();
        foreach (var property in properties)
        {
            sb.AppendLine($"        private bool _{CamelCase(property.Name)}IsSet;");

            sb.AppendLine($"        private Lazy<{property.Type}> _{CamelCase(property.Name)} = new Lazy<{property.Type}>(() => default({property.Type}));");

            sb.AppendLine($"        public {className} With{property.Name}({property.Type} value) => With{property.Name}(() => value);");

            sb.Append(GenerateWithPropertyFuncMethod(classSymbol, property));

            sb.Append(GeneratePropertyActionMethodIfApplicable(classSymbol, property, allClassSymbols));

            sb.AppendLine($"        public {className} Without{property.Name}()");
            sb.AppendLine("        {");
            sb.AppendLine($"            With{property.Name}(() => default({property.Type}));");
            sb.AppendLine($"            _{CamelCase(property.Name)}IsSet = false;");
            sb.AppendLine("            return this;");
            sb.AppendLine("        }");
            sb.AppendLine();
        }

        return sb;
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
            .AppendLine($"            _{CamelCase(property.Name)}IsSet = true;")
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
            .AppendLine("        {")
            .AppendLine($"            var builder = new {builderName}();")
            .AppendLine("            action(builder);")
            .AppendLine("            return builder.Build(useObjectInitializer);")
            .AppendLine("        });");
    }

    private static StringBuilder GenerateWithIEnumerableBuilderActionMethod(
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
                var fileDataType = kind.ToFileDataType();
                allClassSymbols.Add(new ClassSymbol
                {
                    Type = fileDataType,
                    BuilderNamespace = existingClassSymbol.BuilderNamespace,
                    BuilderClassName = shortBuilderName,
                    FullBuilderClassName = fullBuilderName,
                    NamedTypeSymbol = typeSymbol
                });
            }
        }
        else
        {
            // Normal
            fullBuilderName = $"{kind}Builder{(typeSymbol == null ? string.Empty : "<" + typeSymbol.GenerateFullTypeName() + ">")}";
        }

        // If the property.Type is an interface or array, no cast is needed. Else cast the interface to the real type.
        var cast = property.Type.TypeKind is TypeKind.Interface or TypeKind.Array ?
            string.Empty :
            $"({property.Type}) ";

        return new StringBuilder()
            .AppendLine($"        public {className} With{property.Name}(Action<{fullBuilderName}> action, bool useObjectInitializer = true) => With{property.Name}(() =>")
            .AppendLine("        {")
            .AppendLine($"            var builder = new {fullBuilderName}();")
            .AppendLine("            action(builder);")
            .AppendLine($"            return {cast}builder.Build(useObjectInitializer);")
            .AppendLine("        });");
    }

    private static IEnumerable<IPropertySymbol> GetProperties(ClassSymbol classSymbol)
    {
        var properties = classSymbol.NamedTypeSymbol.GetMembers().OfType<IPropertySymbol>()
            .Where(x => x.SetMethod is not null)
            .Where(x => x.CanBeReferencedByName)
            .ToList();

        var propertyNames = properties.Select(x => x.Name);

        var baseType = classSymbol.NamedTypeSymbol.BaseType;

        while (baseType != null)
        {
            properties.AddRange(baseType.GetMembers().OfType<IPropertySymbol>()
                .Where(x => x.CanBeReferencedByName)
                .Where(x => x.SetMethod is not null)
                .Where(x => !propertyNames.Contains(x.Name)));

            baseType = baseType.BaseType;
        }

        return properties;
    }

    private static string GenerateBuildMethod(ClassSymbol classSymbol)
    {
        if (classSymbol.NamedTypeSymbol.Constructors.IsEmpty || classSymbol.NamedTypeSymbol.Constructors.Any(c => !c.Parameters.IsEmpty))
        {
            throw new NotSupportedException($"Unable to generate a FluentBuilder for the class '{classSymbol.NamedTypeSymbol}' because no public parameterless constructor is defined.");
        }

        var properties = GetProperties(classSymbol).ToArray();
        var output = new StringBuilder();
        var className = classSymbol.NamedTypeSymbol.GenerateShortTypeName();

        output.AppendLine($@"        public override {className} Build(bool useObjectInitializer = true)
        {{
            if (Object?.IsValueCreated != true)
            {{
                Object = new Lazy<{className}>(() =>
                {{
                    if (useObjectInitializer)
                    {{
                        return new {className}
                        {{");
        output.AppendLine(string.Join(",\r\n", properties.Select(property => $@"                            {property.Name} = _{CamelCase(property.Name)}.Value")));
        output.AppendLine($@"                        }};
                    }}

                    var instance = new {className}();");
        output.AppendLine(string.Join("\r\n", properties.Select(property => $@"                    if (_{CamelCase(property.Name)}IsSet) {{ instance.{property.Name} = _{CamelCase(property.Name)}.Value; }}")));
        output.AppendLine($@"                    return instance;
                }});
            }}

            PostBuild(Object.Value);

            return Object.Value;
        }}

        public static {className} Default() => new {className}();");

        return output.ToString();
    }

    private IReadOnlyList<ClassSymbol> GetClassSymbols()
    {
        var classSymbols = new List<ClassSymbol>();
        foreach (var fluentDataItem in _receiver.CandidateFluentDataItems)
        {
            if (_context.TryGetNamedTypeSymbolByFullMetadataName(fluentDataItem, out var classSymbol))
            {
                classSymbols.Add(classSymbol);
            }
        }

        return classSymbols;
    }

    private static string CamelCase(string value) => $"{value.Substring(0, 1).ToLowerInvariant()}{value.Substring(1)}";
}