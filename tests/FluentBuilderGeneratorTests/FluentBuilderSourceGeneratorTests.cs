using System.IO;
using CSharp.SourceGenerators.Extensions;
using CSharp.SourceGenerators.Extensions.Models;
using FluentAssertions;
using FluentBuilderGenerator;
using Xunit;
using AnyOfTypes;

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
                AttributeToAddToClass = "FluentBuilder.AutoGenerateBuilder"
            };

            // Act
            var result = _sut.Execute(new[] { sourceFile });

            // Assert
            result.Valid.Should().BeTrue();
            result.Files.Should().HaveCount(3);

            var builder = result.Files[2];
            builder.Path.Should().EndWith("FluentBuilderGeneratorTests.DTO.User_Builder.g.cs");

            builder.Text.Should().NotBeNullOrEmpty();
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
                AttributeToAddToClass = new ExtraAttribute
                {
                    Name = "FluentBuilder.AutoGenerateBuilder"
                }
            };

            // Act
            var result = _sut.Execute(new[] { sourceFile });

            // Assert
            result.Valid.Should().BeTrue();
            result.Files.Should().HaveCount(3);

            var builder = result.Files[2];
            builder.Path.Should().EndWith(builderFileName);

            // File.WriteAllText($"../../../DTO/{builderFileName}", code);
            builder.Text.Should().Be(File.ReadAllText($"../../../DTO/{builderFileName}"));
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
                AttributeToAddToClass = new ExtraAttribute
                {
                    Name = "FluentBuilder.AutoGenerateBuilder",
                    ArgumentList = "dummy"
                }
            };

            var path2 = "./DTO/AddressT.cs";
            var sourceFile2 = new SourceFile
            {
                Path = path2,
                Text = File.ReadAllText(path2),
                AttributeToAddToClass = new ExtraAttribute
                {
                    Name = "FluentBuilder.AutoGenerateBuilder",
                    ArgumentList = new[] { "dummy1", "dummy2" }
                }
            };

            // Act
            var result = _sut.Execute(new[] { sourceFile1, sourceFile2 });

            // Assert
            result.Valid.Should().BeTrue();
            result.Files.Should().HaveCount(4);

            var builderForUserTWithAddressT = result.Files[2];
            builderForUserTWithAddressT.Path.Should().EndWith(builder1FileName);

            // File.WriteAllText($"../../../DTO/{builder1FileName}", code);
            builderForUserTWithAddressT.Text.Should().Be(File.ReadAllText($"../../../DTO/{builder1FileName}"));

            //var builderForAddressT = result.SyntaxTrees[3];
            //builderForAddressT.FilePath.Should().EndWith("FluentBuilderGeneratorTests_DTO_Address_T__1_Builder.g.cs");
        }
    }
}