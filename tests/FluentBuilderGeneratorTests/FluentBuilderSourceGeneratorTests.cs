using System.IO;
using CSharp.SourceGenerators.Extensions;
using CSharp.SourceGenerators.Extensions.Models;
using FluentAssertions;
using FluentBuilder;
using FluentBuilderGenerator;
using FluentBuilderGeneratorTests.DTO;
using Xunit;

namespace FluentBuilderGeneratorTests
{
    public class FluentBuilderSourceGeneratorTests
    {
        private const bool Write = false;

        private readonly FluentBuilderSourceGenerator _sut;

        public FluentBuilderSourceGeneratorTests()
        {
            _sut = new FluentBuilderSourceGenerator();

            var x = new AddressBuilder()
                .WithArray(ab => ab
                    .Add("a")
                    .Add(()=> "b")
                    .Build()
                )
                .WithIListAddress(x => x
                    .Add(new Address())
                    .Add(() => new Address())
                    .Add(e => e
                        .WithCity("c")
                        .Build())
                    .Build())
                .Build();
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
            result.Files.Should().HaveCount(5);

            var baseBuilder = result.Files[1];
            if (true) File.WriteAllText($"../../../DTO/Builder.cs", baseBuilder.Text);
            baseBuilder.Text.Should().Be(File.ReadAllText($"../../../DTO/Builder.cs"));

            var builder = result.Files[4];
            builder.Path.Should().EndWith("FluentBuilderGeneratorTests.DTO.User_Builder.g.cs");
            builder.Text.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GenerateFiles_ForClassWithArrayAndDictionaryProperty_Should_GenerateCorrectFiles()
        {
            // Arrange
            var builderFileName = "BuilderGeneratorTests.DTO.Address_Builder.g.cs";
            var addressIEnumerableBuilderFileName = "BuilderGeneratorTests.DTO.Address_IEnumerableBuilder.g.cs";
            var enumerableFileName = "FluentBuilder.IEnumerableBuilder.g.cs";
            var dictFileName = "FluentBuilder.IDictionaryBuilder.g.cs";
            var path = "./DTO/Address.cs";
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
            result.Files.Should().HaveCount(6);

            var ienumerableBuilder = result.Files[2];
            ienumerableBuilder.Path.Should().EndWith(enumerableFileName);
            if (Write) File.WriteAllText($"../../../DTO/{enumerableFileName}", ienumerableBuilder.Text);
            ienumerableBuilder.Text.Should().Be(File.ReadAllText($"../../../DTO/{enumerableFileName}"));

            var dictBuilder = result.Files[3];
            dictBuilder.Path.Should().EndWith(dictFileName);
            if (Write) File.WriteAllText($"../../../DTO/{dictFileName}", dictBuilder.Text);
            dictBuilder.Text.Should().Be(File.ReadAllText($"../../../DTO/{dictFileName}"));

            var classBuilder = result.Files[4];
            classBuilder.Path.Should().EndWith(builderFileName);
            if (Write) File.WriteAllText($"../../../DTO/{builderFileName}", classBuilder.Text);
            classBuilder.Text.Should().Be(File.ReadAllText($"../../../DTO/{builderFileName}"));
            
            var addressIEnumerableBuilder = result.Files[5];
            addressIEnumerableBuilder.Path.Should().EndWith(addressIEnumerableBuilderFileName);
            if (Write) File.WriteAllText($"../../../DTO/{addressIEnumerableBuilderFileName}", addressIEnumerableBuilder.Text);
            addressIEnumerableBuilder.Text.Should().Be(File.ReadAllText($"../../../DTO/{addressIEnumerableBuilderFileName}"));
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
            result.Files.Should().HaveCount(5);

            var builder = result.Files[4];
            builder.Path.Should().EndWith(builderFileName);

            if (Write) File.WriteAllText($"../../../DTO/{builderFileName}", builder.Text);
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
            result.Files.Should().HaveCount(6);

            var builderForUserTWithAddressT = result.Files[4];
            builderForUserTWithAddressT.Path.Should().EndWith(builder1FileName);

            if (Write) File.WriteAllText($"../../../DTO/{builder1FileName}", builderForUserTWithAddressT.Text);
            builderForUserTWithAddressT.Text.Should().Be(File.ReadAllText($"../../../DTO/{builder1FileName}"));

            //var builderForAddressT = result.SyntaxTrees[3];
            //builderForAddressT.FilePath.Should().EndWith("FluentBuilderGeneratorTests_DTO_Address_T__1_Builder.g.cs");
        }

        [Fact]
        public void GenerateFiles_For2GenericClasses_WithDefaultConstructor_Should_GenerateCorrectFiles()
        {
            // Arrange
            var builder1FileName = "FluentBuilderGeneratorTests_DTO_UserTWithAddressAndConstructor_T__1_Builder.g.cs";
            var path1 = "./DTO/UserTWithAddressAndConstructor.cs";
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
            result.Files.Should().HaveCount(6);

            var builderForUserTWithAddressAndConstructor = result.Files[4];
            builderForUserTWithAddressAndConstructor.Path.Should().EndWith(builder1FileName);

            if (Write) File.WriteAllText($"../../../DTO/{builder1FileName}", builderForUserTWithAddressAndConstructor.Text);
            builderForUserTWithAddressAndConstructor.Text.Should().Be(File.ReadAllText($"../../../DTO/{builder1FileName}"));
        }
    }
}