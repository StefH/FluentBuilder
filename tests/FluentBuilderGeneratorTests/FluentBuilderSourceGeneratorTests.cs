using System.IO;
using FluentAssertions;
using FluentBuilderGenerator;
using FluentBuilderGeneratorTests.Utils;
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
                Text = File.ReadAllText("./DTO/User.cs"),
                AttributeToAddToClasses = "FluentBuilder.AutoGenerateBuilder"
            };

            // Act
            var result = _sut.Execute(new[] { sourceFile });

            // Asert
            result.Valid.Should().BeTrue();
            result.SyntaxTrees.Should().HaveCount(3);
        }
    }
}