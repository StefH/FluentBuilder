using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using CSharp.SourceGenerators.Extensions.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
        public static ExecuteResult Execute(this ISourceGenerator sourceGenerator, IEnumerable<SourceFile> sources, IEnumerable<string>? additionalTextPaths = null)
        {
            var metadataReferences = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic)
                .Select(a => MetadataReference.CreateFromFile(a.Location));

            var sourceSyntaxTrees = sources.Select(GetSyntaxTree);

            var additionalTexts = additionalTextPaths?.Select(tp => new CustomAdditionalText(tp)) ?? Enumerable.Empty<AdditionalText>();

            var compilation = CSharpCompilation.Create(
                $"original-{Guid.NewGuid()}",
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
                    .Select(s => new FileResult
                    {
                        SyntaxTree = s,
                        Path = s.FilePath,
                        Text = s.ToString()
                    })
                    .ToList()
            };
        }

        private static SyntaxTree GetSyntaxTree(SourceFile source)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(source.Text, null, source.Path);
            if (string.IsNullOrEmpty(source.AttributeToAddToClasses))
            {
                return syntaxTree;
            }

            var root = syntaxTree.GetRoot();
            foreach (var classDeclaration in root.DescendantNodes().OfType<ClassDeclarationSyntax>())
            {
                // https://stackoverflow.com/questions/35927427/how-to-create-an-attributesyntax-with-a-parameter
                var name = SyntaxFactory.ParseName(source.AttributeToAddToClasses);
                var attribute = SyntaxFactory.Attribute(name);
                var attributeList = new SeparatedSyntaxList<AttributeSyntax>().Add(attribute);

                var list = SyntaxFactory.AttributeList(attributeList);
                var newClassDeclaration = classDeclaration.AddAttributeLists(list);

                // https://stackoverflow.com/questions/44060524/why-does-syntaxnode-replacenode-change-the-syntaxtree-options
                root = root.ReplaceNode(classDeclaration, newClassDeclaration);
            }

            // https://stackoverflow.com/questions/21754908/cant-update-changes-to-tree-roslyn
            return CSharpSyntaxTree.ParseText(root.GetText().ToString(), null, source.Path);
        }
    }
}