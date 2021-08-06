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
            var path = "./DTO/User.cs";
            var sourceFile = new SourceFile
            {
                Path = path,
                Text = File.ReadAllText(path),
                AttributeToAddToClasses = "FluentBuilder.AutoGenerateBuilder"
            };

            // Act
            var result = _sut.Execute(new[] { sourceFile });

            // Asert
            result.Valid.Should().BeTrue();
            result.SyntaxTrees.Should().HaveCount(3);

            result.SyntaxTrees[2].FilePath.Should().EndWith("FluentBuilderGeneratorTests.DTO.User_Builder.g.cs");
        }

        [Fact]
        public void GenerateFiles_For1GenericClass_Should_GenerateCorrectFiles()
        {
            // Arrange
            var path = "./DTO/UserT.cs";
            var sourceFile = new SourceFile
            {
                Path = path,
                Text = File.ReadAllText(path),
                AttributeToAddToClasses = "FluentBuilder.AutoGenerateBuilder"
            };

            // Act
            var result = _sut.Execute(new[] { sourceFile });

            // Asert
            result.Valid.Should().BeTrue();
            result.SyntaxTrees.Should().HaveCount(3);

            var builder = result.SyntaxTrees[2];

            builder.FilePath.Should().EndWith("FluentBuilderGeneratorTests_DTO_UserT_T__1_Builder.g.cs");

            var code = builder.ToString();
            code.Should().Be("");
        }
    }
}