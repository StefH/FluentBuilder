using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FluentBuilderGeneratorTests.Utils
{
    public static class SourceGeneratorExtensions
    {
        /// <summary>
        /// Executes the Source Generator
        /// 
        /// Based on https://notanaverageman.github.io/2020/12/07/cs-source-generators-cheatsheet.html
        /// </summary>
        public static ExecuteResult Execute(this ISourceGenerator sourceGenerator, IEnumerable<SourceFile> sources, IEnumerable<string>? additionalTextPaths = null)
        {
            var metadataReferences = new List<MetadataReference>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                if (!assembly.IsDynamic)
                {
                    metadataReferences.Add(MetadataReference.CreateFromFile(assembly.Location));
                }
            }

            var sourceSyntaxTrees = new List<SyntaxTree>();
            var additionalTexts = new List<AdditionalText>();
            foreach (var source in sources)
            {
                sourceSyntaxTrees.Add(GetSyntaxTree(source));
            }

            if (additionalTextPaths is not null)
            {
                foreach (string additionalTextPath in additionalTextPaths)
                {
                    var additionalText = new CustomAdditionalText(additionalTextPath);
                    additionalTexts.Add(additionalText);
                }
            }

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
                ErrorMessages = diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error).Select(d => d.GetMessage()).ToList(),
                SyntaxTrees = ImmutableArray.CreateRange(outputCompilation.SyntaxTrees.Where(st => !sources.Any(s => s.Path == st.FilePath)))
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