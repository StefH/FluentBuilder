using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using FluentAssertions;
using FluentBuilderGenerator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace FluentBuilderGeneratorTests
{
    public class FluentBuilderSourceGeneratorTests
    {
        private readonly FluentBuilderSourceGenerator _sut;

        public FluentBuilderSourceGeneratorTests()
        {
            _sut = new FluentBuilderSourceGenerator();
        }

        [Fact]
        public void GenerateFiles_For1Class_Should_GenerateCorrectFiles()
        {
            // Act
            var result = Execute(_sut, new[] { File.ReadAllText("./DTO/User.cs") });

            // Asert
            result.Should().HaveCount(3);
        }

        /// <summary>
        /// Based on https://notanaverageman.github.io/2020/12/07/cs-source-generators-cheatsheet.html
        /// </summary>
        private static IReadOnlyList<SyntaxTree> Execute(ISourceGenerator sourceGenerator, IEnumerable<string> sources, IEnumerable<string>? additionalTextPaths = null)
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

            var syntaxTrees = new List<SyntaxTree>();
            var additionalTexts = new List<AdditionalText>();

            foreach (string source in sources)
            {
                var syntaxTree = CSharpSyntaxTree.ParseText(source);
                syntaxTrees.Add(syntaxTree);
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
                syntaxTrees,
                metadataReferences,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            var driver = CSharpGeneratorDriver
                .Create(sourceGenerator)
                .AddAdditionalTexts(ImmutableArray.CreateRange(additionalTexts));

            driver.RunGeneratorsAndUpdateCompilation(
                compilation,
                out Compilation outputCompilation,
                out ImmutableArray<Diagnostic> diagnostics);

            bool hasError = false;
            foreach (var diagnostic in diagnostics.Where(x => x.Severity == DiagnosticSeverity.Error))
            {
                hasError = true;
                Console.WriteLine(diagnostic.GetMessage());
            }

            hasError.Should().BeFalse();

            return outputCompilation.SyntaxTrees.ToList();
        }
    }
}