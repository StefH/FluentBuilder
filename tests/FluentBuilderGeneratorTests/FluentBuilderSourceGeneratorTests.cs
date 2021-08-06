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

            var builder = result.SyntaxTrees[2];
            builder.FilePath.Should().EndWith("FluentBuilderGeneratorTests.DTO.User_Builder.g.cs");

            var code = builder.ToString();
            code.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GenerateFiles_For1GenericClass_Should_GenerateCorrectFiles()
        {
            // Arrange
            var builderFileName = "FluentBuilderGeneratorTests_DTO_UserT_T__1_Builder.g.cs";
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
            builder.FilePath.Should().EndWith(builderFileName);

            var code = builder.ToString();
            // File.WriteAllText($"../../../DTO/{builderFileName}", code);
            code.Should().Be(File.ReadAllText($"../../../DTO/{builderFileName}"));
        }

        [Fact]
        public void GenerateFiles_For2GenericClasses_Should_GenerateCorrectFiles()
        {
            // Arrange
            var builder1FileName = "FluentBuilderGeneratorTests_DTO_UserTWithAddressT_T__1_Builder.g.cs";
            var path1 = "./DTO/UserTWithAddressT.cs";
            var sourceFile1 = new SourceFile
            {
                Path = path1,
                Text = File.ReadAllText(path1),
                AttributeToAddToClasses = "FluentBuilder.AutoGenerateBuilder"
            };

            var path2 = "./DTO/AddressT.cs";
            var sourceFile2 = new SourceFile
            {
                Path = path2,
                Text = File.ReadAllText(path2),
                AttributeToAddToClasses = "FluentBuilder.AutoGenerateBuilder"
            };

            // Act
            var result = _sut.Execute(new[] { sourceFile1, sourceFile2 });

            // Asert
            result.Valid.Should().BeTrue();
            result.SyntaxTrees.Should().HaveCount(4);

            var builderForUserTWithAddressT = result.SyntaxTrees[2];
            builderForUserTWithAddressT.FilePath.Should().EndWith(builder1FileName);

            var code = builderForUserTWithAddressT.ToString();
            // File.WriteAllText($"../../../DTO/{builder1FileName}", code);
            code.Should().Be(File.ReadAllText($"../../../DTO/{builder1FileName}"));

            //var builderForAddressT = result.SyntaxTrees[3];
            //builderForAddressT.FilePath.Should().EndWith("FluentBuilderGeneratorTests_DTO_Address_T__1_Builder.g.cs");
        }
    }
}