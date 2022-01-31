// This source code is based on https://justsimplycode.com/2020/12/06/auto-generate-builders-using-source-generator-in-net-5
using System.Text;
using FluentBuilderGenerator.Extensions;
using FluentBuilderGenerator.Models;
using FluentBuilderGenerator.SyntaxReceiver;
using FluentBuilderGenerator.Types;
using FluentBuilderGenerator.Wrappers;
using Microsoft.CodeAnalysis;

namespace FluentBuilderGenerator.FileGenerators;

internal class FluentBuilderClassesGenerator : IFilesGenerator
{
    private readonly IGeneratorExecutionContextWrapper _wrapper;
    private readonly IAutoGenerateBuilderSyntaxReceiver _receiver;
    private readonly bool _supportsNullable;

    public FluentBuilderClassesGenerator(IGeneratorExecutionContextWrapper wrapper, IAutoGenerateBuilderSyntaxReceiver receiver, bool supportsNullable)
    {
        _wrapper = wrapper;
        _receiver = receiver;
        _supportsNullable = supportsNullable;
    }

    public IReadOnlyList<FileData> GenerateFiles()
    {
        var applicableClassSymbols = GetClassSymbols();
        var extraClassSymbols = applicableClassSymbols.ToList();

        var classes = applicableClassSymbols.Select(classSymbol => new FileData
        (
            FileDataType.Builder,
            $"{classSymbol.NamedTypeSymbol.GenerateFileName()}_Builder.g.cs",
            CreateClassBuilderCode(classSymbol.NamedTypeSymbol, extraClassSymbols)
        ));

        // Extra
        var extra = extraClassSymbols
            .Where(e => e.Type is FileDataType.IEnumerableBuilder or FileDataType.ICollectionBuilder)
            .OrderBy(e => e.Type)
            .Select(classSymbol => new FileData
            (
                classSymbol.Type,
                $"{classSymbol.NamedTypeSymbol.GenerateFileName()}_{classSymbol.Type}.g.cs",
                CreateIEnumerableBuilderCode(classSymbol.ClassName, classSymbol.NamedTypeSymbol)
            ));

        return classes.Union(extra).ToList();
    }

    private string CreateIEnumerableBuilderCode(string className, INamedTypeSymbol itemClassSymbol) => $@"//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by https://github.com/StefH/FluentBuilder.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

{(_supportsNullable ? "#nullable enable" : string.Empty)}
using System;
using System.Collections;
using System.Collections.Generic;
using FluentBuilder;
using {itemClassSymbol.ContainingNamespace};

namespace FluentBuilder
{{
    public partial class {className} : IEnumerableBuilder<{itemClassSymbol.GenerateClassName()}>{itemClassSymbol.GetWhereStatement()}
    {{
{GenerateAddMethods(className, itemClassSymbol)}
    }}
}}
{(_supportsNullable ? "#nullable disable" : string.Empty)}";

    private string CreateClassBuilderCode(INamedTypeSymbol classSymbol, List<ClassSymbol> allClassSymbols) => $@"//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by https://github.com/StefH/FluentBuilder.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

{(_supportsNullable ? "#nullable enable" : string.Empty)}
using System;
using System.Collections;
using System.Collections.Generic;
using FluentBuilder;
using {classSymbol.ContainingNamespace};

namespace FluentBuilder
{{
    public partial class {classSymbol.GenerateClassName(true)} : Builder<{classSymbol.GenerateClassName()}>{classSymbol.GetWhereStatement()}
    {{
{GenerateWithPropertyCode(classSymbol, allClassSymbols)}
{GenerateBuildMethod(classSymbol)}
    }}
}}
{(_supportsNullable ? "#nullable disable" : string.Empty)}";

    private static string GenerateWithPropertyCode(INamedTypeSymbol classSymbol, List<ClassSymbol> allClassSymbols)
    {
        var properties = GetProperties(classSymbol);
        var className = classSymbol.GenerateClassName(true);

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

        return sb.ToString();
    }

    private static StringBuilder GeneratePropertyActionMethodIfApplicable(
        INamedTypeSymbol classSymbol,
        IPropertySymbol property,
        List<ClassSymbol> allClassSymbols)
    {
        var existingClassSymbol = allClassSymbols.FirstOrDefault(c => c.NamedTypeSymbol.Name == property.Type.Name);
        if (existingClassSymbol is not null)
        {
            return GenerateWithPropertyActionMethod(classSymbol, property);
        }

        if (property.TryGetIDictionaryElementTypes(out var dictionaryTypes))
        {
            return GenerateWithIDictionaryBuilderActionMethod(classSymbol, property, dictionaryTypes);
        }

        if (property.TryGetIEnumerableElementType(out var elementType, out var kind))
        {
            if (kind == FluentTypeKind.Array)
            {
                kind = FluentTypeKind.IEnumerable;
            }

            return GenerateWithIEnumerableBuilderActionMethod(kind, classSymbol, property, elementType, allClassSymbols);
        }

        return new StringBuilder();
    }

    private static StringBuilder GenerateWithPropertyFuncMethod(INamedTypeSymbol classSymbol, IPropertySymbol property)
    {
        var className = classSymbol.GenerateClassName(true);

        var output = new StringBuilder();
        output.AppendLine($"        public {className} With{property.Name}(Func<{property.Type}> func)");
        output.AppendLine("        {");
        output.AppendLine($"            _{CamelCase(property.Name)} = new Lazy<{property.Type}>(func);");
        output.AppendLine($"            _{CamelCase(property.Name)}IsSet = true;");
        output.AppendLine("            return this;");
        output.AppendLine("        }");
        return output;
    }

    private static StringBuilder GenerateWithPropertyActionMethod(INamedTypeSymbol classSymbol, IPropertySymbol property)
    {
        var className = classSymbol.GenerateClassName(true);
        var propertyName = property.Type is INamedTypeSymbol propertyNamedType ? propertyNamedType.GenerateClassName(true) : $"{property.Type.Name}Builder";

        var sb = new StringBuilder();
        sb.AppendLine($"        public {className} With{property.Name}(Action<FluentBuilder.{propertyName}> action, bool useObjectInitializer = true) => With{property.Name}(() =>");
        sb.AppendLine("        {");
        sb.AppendLine($"            var builder = new FluentBuilder.{propertyName}();");
        sb.AppendLine("            action(builder);");
        sb.AppendLine("            return builder.Build(useObjectInitializer);");
        sb.AppendLine("        });");
        return sb;
    }

    private static StringBuilder GenerateWithIDictionaryBuilderActionMethod(
        INamedTypeSymbol classSymbol,
        IPropertySymbol property,
        (INamedTypeSymbol key, INamedTypeSymbol value)? tuple
    )
    {
        var className = classSymbol.GenerateClassName(true);

        string types = string.Empty;
        if (tuple != null)
        {
            var keyClassName = tuple?.key?.GenerateClassName() ?? "object";
            var valueClassName = tuple?.value?.GenerateClassName() ?? "object";

            types = $"{keyClassName}, {valueClassName}";
        }

        string dictionaryBuilderName = $"IDictionaryBuilder{(tuple == null ? string.Empty : "<" + types + ">")}";

        var sb = new StringBuilder();
        sb.AppendLine($"        public {className} With{property.Name}(Action<FluentBuilder.{dictionaryBuilderName}> action, bool useObjectInitializer = true) => With{property.Name}(() =>");
        sb.AppendLine("        {");
        sb.AppendLine($"            var builder = new FluentBuilder.{dictionaryBuilderName}();");
        sb.AppendLine("            action(builder);");
        sb.AppendLine("            return builder.Build(useObjectInitializer);");
        sb.AppendLine("        });");
        return sb;
    }

    private static StringBuilder GenerateWithIEnumerableBuilderActionMethod(
        FluentTypeKind kind,
        INamedTypeSymbol classSymbol,
        IPropertySymbol property,
        INamedTypeSymbol? typeSymbol,
        List<ClassSymbol> allClassSymbols)
    {
        var className = classSymbol.GenerateClassName(true);
        var typeSymbolClassName = typeSymbol?.GenerateClassName();
        var existingClassSymbol = allClassSymbols.FirstOrDefault(c => c.NamedTypeSymbol.Name == typeSymbolClassName);

        string @class = string.Empty;
        string builderName;
        if (existingClassSymbol != null && typeSymbolClassName != null && typeSymbol != null)
        {
            builderName = $"{kind}{typeSymbolClassName}Builder";
            if (allClassSymbols.All(cs => cs.NamedTypeSymbol.Name != builderName))
            {
                var fileDataType = kind == FluentTypeKind.IEnumerable
                    ? FileDataType.IEnumerableBuilder
                    : FileDataType.ICollectionBuilder;
                allClassSymbols.Add(new ClassSymbol(fileDataType, builderName, typeSymbol));
            }
        }
        else
        {
            // Normal
            builderName = $"{kind}{@class}Builder{(typeSymbolClassName == null ? string.Empty : "<" + typeSymbolClassName + ">")}";
        }

        var sb = new StringBuilder();
        sb.AppendLine($"        public {className} With{property.Name}(Action<FluentBuilder.{builderName}> action, bool useObjectInitializer = true) => With{property.Name}(() =>");
        sb.AppendLine("        {");
        sb.AppendLine($"            var builder = new FluentBuilder.{builderName}();");
        sb.AppendLine("            action(builder);");
        sb.AppendLine($"            return ({property.Type}) builder.Build(useObjectInitializer);");
        sb.AppendLine("        });");
        return sb;
    }

    private static StringBuilder GenerateAddMethods(string className, INamedTypeSymbol itemClassSymbol)
    {
        var itemBuilderName = $"{itemClassSymbol.GenerateClassName(true)}";

        var sb = new StringBuilder();
        sb.AppendLine($"        public override {className} Add({itemClassSymbol.Name} item) => Add(() => item);");

        sb.AppendLine($"        public override {className} Add(Func<{itemClassSymbol.Name}> func)");
        sb.AppendLine("        {");
        sb.AppendLine("            _list.Value.Add(func());");
        sb.AppendLine("            return this;");
        sb.AppendLine("        }");

        sb.AppendLine($"        public {className} Add(Action<FluentBuilder.{itemBuilderName}> action, bool useObjectInitializer = true)");
        sb.AppendLine("        {");
        sb.AppendLine($"            var builder = new FluentBuilder.{itemBuilderName}();");
        sb.AppendLine("            action(builder);");
        sb.AppendLine("            Add(() => builder.Build(useObjectInitializer));");
        sb.AppendLine("            return this;");
        sb.AppendLine("        }");
        return sb;
    }

    private static IEnumerable<IPropertySymbol> GetProperties(INamedTypeSymbol classSymbol)
    {
        var properties = classSymbol.GetMembers().OfType<IPropertySymbol>()
            .Where(x => x.SetMethod is not null)
            .Where(x => x.CanBeReferencedByName)
            .ToList();

        var propertyNames = properties.Select(x => x.Name);

        var baseType = classSymbol.BaseType;

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

    private static string GenerateBuildMethod(INamedTypeSymbol classSymbol)
    {
        var properties = GetProperties(classSymbol).ToArray();
        var output = new StringBuilder();
        var className = classSymbol.GenerateClassName();

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
        foreach (var candidateClass in _receiver.CandidateClasses)
        {
            var fullClassName = candidateClass.GetFullName();

            var classSymbol = _wrapper.GetTypeByMetadataName(fullClassName);
            if (classSymbol is not null)
            {
                classSymbols.Add(new ClassSymbol(FileDataType.Builder, classSymbol.GenerateClassName(), classSymbol));
            }
        }

        return classSymbols;
    }

    private static string CamelCase(string value) => $"{value.Substring(0, 1).ToLowerInvariant()}{value.Substring(1)}";
}