// This source file is based on https://justsimplycode.com/2020/12/06/auto-generate-builders-using-source-generator-in-net-5
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace FluentBuilderGenerator
{
    [Generator]
    public class FluentBuilderSourceGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new AutoGenerateBuilderSyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            InjectAutoGenerateBuilderAttribute(context);
            InjectBaseBuilderClass(context);

            if (context.SyntaxReceiver is not AutoGenerateBuilderSyntaxReceiver receiver)
            {
                return;
            }

            var classSymbols = GetClassSymbols(context, receiver);
            foreach (var classSymbol in classSymbols)
            {
                context.AddSource($"{classSymbol.Name}_Builder.cs", SourceText.From(CreateBuilderCode(classSymbol), Encoding.UTF8));
            }
        }

        private static void InjectBaseBuilderClass(GeneratorExecutionContext context)
        {
            const string baseBuilderText = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace FluentBuilder
{
    public abstract class Builder<T> where T : class
    {
        protected Lazy<T> Object;

        public abstract T Build();

        public Builder<T> WithObject(T value) => WithObject(() => value);

        public Builder<T> WithObject(Func<T> func)
        {
            Object = new Lazy<T>(func);
            return this;
        }
    
        protected virtual void PostBuild(T value) {}
    }
}";
            context.AddSource("BaseBuilder.cs", SourceText.From(baseBuilderText, Encoding.UTF8));
        }

        private static void InjectAutoGenerateBuilderAttribute(GeneratorExecutionContext context)
        {
            const string attributeText = @"
 using System;

 namespace FluentBuilder
 {
     [AttributeUsage(AttributeTargets.Class)]
     sealed class AutoGenerateBuilderAttribute : Attribute
     {
         public AutoGenerateBuilderAttribute() {}
     }
 }
 ";
            context.AddSource("AutoGenerateBuilderAttribute.cs", SourceText.From(attributeText, Encoding.UTF8));
        }

        private static List<INamedTypeSymbol> GetClassSymbols(GeneratorExecutionContext context, AutoGenerateBuilderSyntaxReceiver receiver)
        {
            var classSymbols = new List<INamedTypeSymbol>();
            foreach (var candidateClass in receiver.CandidateClasses)
            {
                var namespaceName = GetNamespaceFrom(candidateClass);
                var fullClassName = string.IsNullOrWhiteSpace(namespaceName)
                    ? candidateClass.Identifier.ToString()
                    : $"{namespaceName}.{candidateClass.Identifier}";

                var classSymbol = context.Compilation.GetTypeByMetadataName(fullClassName);
                if (classSymbol != null)
                {
                    classSymbols.Add(classSymbol);
                }
            }

            return classSymbols;
        }

        public static string GetNamespaceFrom(SyntaxNode s) => s.Parent switch
        {
            NamespaceDeclarationSyntax namespaceDeclarationSyntax => namespaceDeclarationSyntax.Name.ToString(),

            null => string.Empty,

            _ => GetNamespaceFrom(s.Parent)
        };

        private static IEnumerable<IPropertySymbol> GetProperties(INamedTypeSymbol classSymbol)
        {
            var properties = classSymbol.GetMembers().OfType<IPropertySymbol>()
                .Where(x => x.SetMethod is not null)
                .Where(x => x.CanBeReferencedByName).ToList();

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

        private string CreateBuilderCode(INamedTypeSymbol classSymbol) => $@"
 using System;
 using FluentBuilder;
 using {classSymbol.ContainingNamespace};

 namespace FluentBuilder
 {{
     public partial class {classSymbol.Name}Builder : Builder<{classSymbol.Name}>
     {{
 {GeneratePropertiesCode(classSymbol)}
 {GenerateBuildsCode(classSymbol)}
     }}
 }}";
        private static string GeneratePropertiesCode(INamedTypeSymbol classSymbol)
        {
            var properties = GetProperties(classSymbol);
            var output = new StringBuilder();

            foreach (var property in properties)
            {
                output.AppendLine($@"
         private Lazy<{property.Type}> _{CamelCase(property.Name)} = new Lazy<{property.Type}>(() => default({property.Type}));

         public {classSymbol.Name}Builder With{property.Name}({property.Type} value) => With{property.Name}(() => value);

         public {classSymbol.Name}Builder With{property.Name}(Func<{property.Type}> func)
         {{
             _{CamelCase(property.Name)} = new Lazy<{property.Type}>(func);

             return this;
         }}

         public {classSymbol.Name}Builder Without{property.Name}() => With{property.Name}(() => default({property.Type}));");

            }

            return output.ToString();
        }

        private static string GenerateBuildsCode(INamedTypeSymbol classSymbol)
        {
            var properties = GetProperties(classSymbol);
            var output = new StringBuilder();

            output.AppendLine($@"       public override {classSymbol.Name} Build()
        {{
            if (Object?.IsValueCreated != true)
            {{
                Object = new Lazy<{classSymbol.Name}>(() => new {classSymbol.Name}
                {{");

            foreach (var property in properties)
            {
                output.AppendLine($@"                        {property.Name} = _{CamelCase(property.Name)}.Value,");
            }

            output.AppendLine($@"
                }});
            }}

            PostBuild(Object.Value);

            return Object.Value;
        }}

        public static {classSymbol.Name} Default() => new {classSymbol.Name}();");

            return output.ToString();
        }

        private static string CamelCase(string value) => $"{value.Substring(0, 1).ToLowerInvariant()}{value.Substring(1)}";
    }
}