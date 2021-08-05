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
            // Arrange
            var sourceFile = new SourceFile
            {
                Path = "./DTO/User.cs",
                Text = File.ReadAllText("./DTO/User.cs")
            };

            // Act
            var result = Execute(_sut, new[] { sourceFile });

            // Asert
            result.Should().HaveCount(2);
            //result[1]

            //File.ReadAllText("../../../DTO/UserBuilder.cs");
        }

        /// <summary>
        /// Based on https://notanaverageman.github.io/2020/12/07/cs-source-generators-cheatsheet.html
        /// </summary>
        private static ImmutableArray<SyntaxTree> Execute(ISourceGenerator sourceGenerator, IEnumerable<SourceFile> sources, IEnumerable<string>? additionalTextPaths = null)
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

            foreach (var source in sources)
            {
                var syntaxTree = CSharpSyntaxTree.ParseText(source.Text, null, source.Path);
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

            return ImmutableArray.CreateRange(outputCompilation.SyntaxTrees.Where(st => !sources.Any(s => s.Path == st.FilePath)));
        }
    }
}