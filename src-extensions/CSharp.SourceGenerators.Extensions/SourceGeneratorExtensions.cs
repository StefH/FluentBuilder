using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using CSharp.SourceGenerators.Extensions.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using AnyOfTypes;

namespace CSharp.SourceGenerators.Extensions
{
    /// <summary>
    /// Based on https://notanaverageman.github.io/2020/12/07/cs-source-generators-cheatsheet.html
    /// </summary>
    public static class SourceGeneratorExtensions
    {
        /// <summary>
        /// Executes and runs the specified <see cref="ISourceGenerator"/>.
        /// </summary>
        /// <param name="sourceGenerator">The SourceGenerator to execute.</param>
        /// <param name="sources">Provide a list of sources which need to be analyzed and processed.</param>
        /// <param name="additionalTextPaths">A list of additional files.</param>
        /// <returns><see cref="ExecuteResult"/></returns>
        public static ExecuteResult Execute(
            this ISourceGenerator sourceGenerator,
            IReadOnlyList<SourceFile> sources,
            IReadOnlyList<string>? additionalTextPaths = null
        )
        {
            return Execute(sourceGenerator, $"GeneratedNamespace_{Guid.NewGuid().ToString().Replace("-", "")}", sources, additionalTextPaths);
        }

        /// <summary>
        /// Executes and runs the specified <see cref="ISourceGenerator"/>.
        /// </summary>
        /// <param name="sourceGenerator">The SourceGenerator to execute.</param>
        /// <param name="assemblyName">The assembly name.</param>
        /// <param name="sources">Provide a list of sources which need to be analyzed and processed.</param>
        /// <param name="additionalTextPaths">A list of additional files.</param>
        /// <returns><see cref="ExecuteResult"/></returns>
        public static ExecuteResult Execute(
            this ISourceGenerator sourceGenerator,
            string assemblyName,
            IReadOnlyList<SourceFile> sources,
            IReadOnlyList<string>? additionalTextPaths = null
        )
        {
            var metadataReferences = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic)
                .Select(a => MetadataReference.CreateFromFile(a.Location));

            var sourceSyntaxTrees = sources.Select(GetSyntaxTree);

            var additionalTexts = additionalTextPaths?.Select(tp => new CustomAdditionalText(tp)) ?? Enumerable.Empty<AdditionalText>();

            var compilation = CSharpCompilation.Create(
                assemblyName,
                sourceSyntaxTrees,
                metadataReferences,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            var driver = CSharpGeneratorDriver
                .Create(sourceGenerator)
                .AddAdditionalTexts(ImmutableArray.CreateRange(additionalTexts));

            driver.RunGeneratorsAndUpdateCompilation(
                compilation,
                out Compilation outputCompilation,
                out ImmutableArray<Diagnostic> diagnostics);

            return new ExecuteResult
            {
                WarningMessages = diagnostics.Where(d => d.Severity == DiagnosticSeverity.Warning).Select(d => d.GetMessage()).ToList(),
                ErrorMessages = diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error).Select(d => d.GetMessage()).ToList(),
                Files = outputCompilation.SyntaxTrees
                    .Where(st => !sources.Any(s => s.Path == st.FilePath))
                    .Select(st => new FileResult
                    {
                        SyntaxTree = st,
                        Path = st.FilePath,
                        Text = st.ToString()
                    })
                    .ToList()
            };
        }

        private static SyntaxTree GetSyntaxTree(SourceFile source)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(source.Text, null, source.Path);
            if (source.AttributeToAddToClass is not null && source.AttributeToAddToInterface is not null)
            {
                return syntaxTree;
            }

            SyntaxNode rootSyntaxNode = default!;
            if (source.AttributeToAddToClass is not null)
            {
                rootSyntaxNode = AddExtraAttribute<ClassDeclarationSyntax>(syntaxTree, source.AttributeToAddToClass.Value);
            }
            else if (source.AttributeToAddToInterface is not null)
            {
                rootSyntaxNode = AddExtraAttribute<InterfaceDeclarationSyntax>(syntaxTree, source.AttributeToAddToInterface.Value);
            }

            // https://stackoverflow.com/questions/21754908/cant-update-changes-to-tree-roslyn
            return CSharpSyntaxTree.ParseText(rootSyntaxNode.GetText().ToString(), null, source.Path);
        }

        private static SyntaxNode AddExtraAttribute<T>(SyntaxTree syntaxTree, AnyOf<string, ExtraAttribute> attributeToAdd)
            where T : TypeDeclarationSyntax
        {
            var rootSyntaxNode = syntaxTree.GetRoot();
            foreach (var classDeclaration in rootSyntaxNode.DescendantNodes().OfType<T>())
            {
                var name = attributeToAdd.IsFirst ? attributeToAdd.First : attributeToAdd.Second.Name;

                // https://stackoverflow.com/questions/35927427/how-to-create-an-attributesyntax-with-a-parameter
                var nameSyntax = SyntaxFactory.ParseName(name);
                var attributeArgumentListSyntax = ParseArguments(attributeToAdd);

                var attributeSyntax = SyntaxFactory.Attribute(nameSyntax, attributeArgumentListSyntax);
                var separatedSyntaxList = new SeparatedSyntaxList<AttributeSyntax>().Add(attributeSyntax);

                var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
                var newClassDeclarationSyntax = classDeclaration.AddAttributeLists(attributeListSyntax);

                // https://stackoverflow.com/questions/44060524/why-does-syntaxnode-replacenode-change-the-syntaxtree-options
                rootSyntaxNode = rootSyntaxNode.ReplaceNode(classDeclaration, newClassDeclarationSyntax);
            }

            return rootSyntaxNode;
        }

        private static AttributeArgumentListSyntax ParseArguments(AnyOf<string, ExtraAttribute> attributeToAdd)
        {
            string arguments = string.Empty;

            if (attributeToAdd.IsSecond && attributeToAdd.Second.ArgumentList is not null)
            {
                arguments = attributeToAdd.Second.ArgumentList.Value.IsFirst ?
                    attributeToAdd.Second.ArgumentList.Value.First :
                    string.Join(",", attributeToAdd.Second.ArgumentList.Value.Second);
            }

            return SyntaxFactory.ParseAttributeArgumentList($"({arguments})");
        }
    }
}