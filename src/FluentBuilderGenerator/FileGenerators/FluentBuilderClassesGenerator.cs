// This source code is based on https://justsimplycode.com/2020/12/06/auto-generate-builders-using-source-generator-in-net-5
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentBuilderGenerator.SyntaxReceiver;
using FluentBuilderGenerator.Wrappers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FluentBuilderGenerator.FileGenerators
{
    internal class FluentBuilderClassesGenerator : IFilesGenerator
    {
        private readonly IGeneratorExecutionContextWrapper _wrapper;
        private readonly IAutoGenerateBuilderSyntaxReceiver _receiver;

        public FluentBuilderClassesGenerator(IGeneratorExecutionContextWrapper wrapper, IAutoGenerateBuilderSyntaxReceiver receiver)
        {
            _wrapper = wrapper;
            _receiver = receiver;
        }

        public IEnumerable<FileData> GenerateFiles()
        {
            var allClassSymbols = GetClassSymbols();

            foreach (var classSymbol in allClassSymbols)
            {
                yield return new FileData
                {
                    FileName = $"{classSymbol.Name}Builder.cs",
                    Text = CreateBuilderCode(classSymbol, allClassSymbols)
                };
            }
        }

        private string CreateBuilderCode(INamedTypeSymbol classSymbol, IReadOnlyList<INamedTypeSymbol> allClassSymbols) => $@"using System;
using FluentBuilder;
using {classSymbol.ContainingNamespace};

namespace FluentBuilder
{{
    public partial class {classSymbol.Name}Builder : Builder<{classSymbol.Name}>
    {{
{GenerateWithPropertyCode(classSymbol, allClassSymbols)}
{GenerateBuildMethod(classSymbol)}
    }}
}}";

        private static string GenerateWithPropertyCode(INamedTypeSymbol classSymbol, IReadOnlyList<INamedTypeSymbol> allClassSymbols)
        {
            var allClassNames = allClassSymbols.Select(c => c.Name).ToList();
            var properties = GetProperties(classSymbol);
            var sb = new StringBuilder();

            foreach (var property in properties)
            {
                sb.AppendLine($"        private Lazy<{property.Type}> _{CamelCase(property.Name)} = new Lazy<{property.Type}>(() => default({property.Type}));");

                sb.AppendLine($"        public {classSymbol.Name}Builder With{property.Name}({property.Type} value) => With{property.Name}(() => value);");

                sb.Append(GenerateWithPropertyFuncMethod(classSymbol, property));

                if (allClassNames.Contains(property.Type.Name.ToString()))
                {
                    sb.Append(GenerateWithPropertyActionMethod(classSymbol, property));
                }

                sb.AppendLine($"        public {classSymbol.Name}Builder Without{property.Name}() => With{property.Name}(() => default({property.Type}));");
                sb.AppendLine();
            }

            return sb.ToString();
        }

        private static StringBuilder GenerateWithPropertyFuncMethod(INamedTypeSymbol classSymbol, IPropertySymbol property)
        {
            var output = new StringBuilder();
            output.AppendLine($"        public {classSymbol.Name}Builder With{property.Name}(Func<{property.Type}> func)");
            output.AppendLine("        {");
            output.AppendLine($"            _{CamelCase(property.Name)} = new Lazy<{property.Type}>(func);");
            output.AppendLine("            return this;");
            output.AppendLine("        }");
            return output;
        }

        private static StringBuilder GenerateWithPropertyActionMethod(INamedTypeSymbol classSymbol, IPropertySymbol property)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"        public {classSymbol.Name}Builder With{property.Name}(Action<FluentBuilder.{property.Type.Name}Builder> action) => With{property.Name}(() =>");
            sb.AppendLine("        {");
            sb.AppendLine($"            var builder = new FluentBuilder.{property.Type.Name}Builder();");
            sb.AppendLine("            action(builder);");
            sb.AppendLine("            return builder.Build();");
            sb.AppendLine("        });");
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
            var properties = GetProperties(classSymbol);
            var output = new StringBuilder();

            output.AppendLine($@"        public override {classSymbol.Name} Build()
        {{
            if (Object?.IsValueCreated != true)
            {{
                Object = new Lazy<{classSymbol.Name}>(() => new {classSymbol.Name}
                {{");

            output.AppendLine(string.Join(",\r\n", properties.Select(property => $@"                    {property.Name} = _{CamelCase(property.Name)}.Value")));
            output.AppendLine($@"                }});
            }}

            PostBuild(Object.Value);

            return Object.Value;
        }}

        public static {classSymbol.Name} Default() => new {classSymbol.Name}();");

            return output.ToString();
        }

        private IReadOnlyList<INamedTypeSymbol> GetClassSymbols()
        {
            var classSymbols = new List<INamedTypeSymbol>();
            foreach (var candidateClass in _receiver.CandidateClasses)
            {
                var namespaceName = GetNamespaceFrom(candidateClass);
                var fullClassName = string.IsNullOrWhiteSpace(namespaceName)
                    ? candidateClass.Identifier.ToString()
                    : $"{namespaceName}.{candidateClass.Identifier}";

                var classSymbol = _wrapper.GetTypeByMetadataName(fullClassName);
                if (classSymbol is not null)
                {
                    classSymbols.Add(classSymbol);
                }
            }

            return classSymbols;
        }

        private static string GetNamespaceFrom(SyntaxNode s) => s.Parent switch
        {
            NamespaceDeclarationSyntax namespaceDeclarationSyntax => namespaceDeclarationSyntax.Name.ToString(),

            null => string.Empty,

            _ => GetNamespaceFrom(s.Parent)
        };

        private static string CamelCase(string value) => $"{value.Substring(0, 1).ToLowerInvariant()}{value.Substring(1)}";
    }
}