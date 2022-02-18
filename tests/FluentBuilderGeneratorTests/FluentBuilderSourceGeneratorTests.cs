using System;
using System.IO;
using System.Linq;
using CSharp.SourceGenerators.Extensions;
using CSharp.SourceGenerators.Extensions.Models;
using FluentAssertions;
using FluentBuilderGenerator;
using Xunit;

namespace FluentBuilderGeneratorTests
{
    public class FluentBuilderSourceGeneratorTests
    {
        private const string Namespace = "FluentBuilderGeneratorTests";

        private const bool Write = true;

        private readonly FluentBuilderSourceGenerator _sut;

        public FluentBuilderSourceGeneratorTests()
        {
            _sut = new FluentBuilderSourceGenerator();
        }

        [Fact]
        public void GenerateFiles_ForAClassWithoutAPublicConstructor_Should_Create_ErrorFile()
        {
            // Arrange
            var path = "./DTO2/MyAppDomainBuilder.cs";
            var sourceFile = new SourceFile
            {
                Path = path,
                Text = File.ReadAllText(path),
                AttributeToAddToClass = new ExtraAttribute
                {
                    Name = "AutoGenerateBuilder",
                    ArgumentList = "typeof(AppDomain)"
                }
            };

            // Act
            var result = _sut.Execute(Namespace, new[] { sourceFile });

            // Assert
            result.Files.Should().HaveCount(8);
            result.Files[7].Path.Should().EndWith("Error.g.cs");
        }

        [Fact]
        public void GenerateFiles_ForAClassWithout_Public_Parameterless_Constructor_Should_Create_ErrorFile()
        {
            // Arrange
            var path = "./DTO2/MyDateTimeBuilder.cs";
            var sourceFile = new SourceFile
            {
                Path = path,
                Text = File.ReadAllText(path),
                AttributeToAddToClass = new ExtraAttribute
                {
                    Name = "AutoGenerateBuilder",
                    ArgumentList = "typeof(DateTime)"
                }
            };

            // Act
            var result = _sut.Execute(Namespace, new[] { sourceFile });

            // Assert
            result.Files.Should().HaveCount(8);
            result.Files[7].Path.Should().EndWith("Error.g.cs");
        }

        [Fact]
        public void GenerateFiles_For2Classes_Should_GenerateCorrectFiles()
        {
            // Arrange
            var pathUser = "./DTO/User.cs";
            var sourceFileUser = new SourceFile
            {
                Path = pathUser,
                Text = File.ReadAllText(pathUser),
                AttributeToAddToClass = "FluentBuilder.AutoGenerateBuilder"
            };

            var pathBuilder = "./DTO/MyDummyClassBuilder.cs";
            var sourceFileBuilder = new SourceFile
            {
                Path = pathBuilder,
                Text = File.ReadAllText(pathBuilder),
                AttributeToAddToClass = new ExtraAttribute
                {
                    Name = "AutoGenerateBuilder",
                    ArgumentList = "typeof(DummyClass)"
                }
            };

            // Act
            var result = _sut.Execute(Namespace, new[] { sourceFileUser, sourceFileBuilder });

            // Assert
            result.Valid.Should().BeTrue();
            result.Files.Should().HaveCount(9);

            for (int i = 7; i < 9; i++)
            {
                var builder = result.Files[i];

                var filename = Path.GetFileName(builder.Path);

                if (Write) File.WriteAllText($"../../../DTO/{filename}", builder.Text);
                builder.Text.Should().Be(File.ReadAllText($"../../../DTO/{filename}"));
            }
        }

        [Fact]
        public void GenerateFiles_ForClassWithArrayAndDictionaryProperty_Should_GenerateCorrectFiles()
        {
            // Arrange
            var fileNames = new[]
            {
                "FluentBuilder.AutoGenerateBuilderAttribute.g.cs",
                "FluentBuilder.BaseBuilder.g.cs",

                "FluentBuilder.ArrayBuilder.g.cs",
                "FluentBuilder.IEnumerableBuilder.g.cs",
                "FluentBuilder.IListBuilder.g.cs",
                "FluentBuilder.ICollectionBuilder.g.cs",
                "FluentBuilder.IDictionaryBuilder.g.cs",

                "BuilderGeneratorTests.DTO.AddressBuilder.g.cs",
                "BuilderGeneratorTests.DTO.Address_ArrayBuilder.g.cs",
                "BuilderGeneratorTests.DTO.Address_IEnumerableBuilder.g.cs",
                "BuilderGeneratorTests.DTO.Address_IListBuilder.g.cs",
                "BuilderGeneratorTests.DTO.Address_ICollectionBuilder.g.cs"
            };

            var path = "./DTO/Address.cs";
            var sourceFile = new SourceFile
            {
                Path = path,
                Text = File.ReadAllText(path),
                AttributeToAddToClass = "FluentBuilder.AutoGenerateBuilder"
            };

            // Act
            var result = _sut.Execute(Namespace, new[] { sourceFile });

            // Assert
            result.Valid.Should().BeTrue();
            result.Files.Should().HaveCount(fileNames.Length);

            foreach (var x in fileNames.Select((fileName, index) => new { fileName, index }))
            {
                var builder = result.Files[x.index];
                builder.Path.Should().EndWith(x.fileName);
                if (Write) File.WriteAllText($"../../../DTO/{x.fileName}", builder.Text);
                builder.Text.Should().Be(File.ReadAllText($"../../../DTO/{x.fileName}"));
            }
        }

        [Fact]
        public void GenerateFiles_For1GenericClass_Should_GenerateCorrectFiles()
        {
            // Arrange
            var builderFileName = "FluentBuilderGeneratorTests.DTO.UserTBuilder_T_.g.cs";
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
            var result = _sut.Execute(Namespace, new[] { sourceFile });

            // Assert
            result.Valid.Should().BeTrue();
            result.Files.Should().HaveCount(8);

            var builder = result.Files[7];
            builder.Path.Should().EndWith(builderFileName);

            if (Write) File.WriteAllText($"../../../DTO/{builderFileName}", builder.Text);
            builder.Text.Should().Be(File.ReadAllText($"../../../DTO/{builderFileName}"));
        }

        [Fact]
        public void GenerateFiles_For1GenericClassTT_Should_GenerateCorrectFiles()
        {
            // Arrange
            var builderFileName = "FluentBuilderGeneratorTests.DTO.AddressTTBuilder_T1,T2_.g.cs";
            var path = "./DTO/AddressTT.cs";
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
            var result = _sut.Execute(Namespace, new[] { sourceFile });

            // Assert
            result.Valid.Should().BeTrue();
            result.Files.Should().HaveCount(8);

            var builder = result.Files[7];
            builder.Path.Should().EndWith(builderFileName);

            if (Write) File.WriteAllText($"../../../DTO/{builderFileName}", builder.Text);
            builder.Text.Should().Be(File.ReadAllText($"../../../DTO/{builderFileName}"));
        }

        [Fact]
        public void GenerateFiles_For2GenericClasses_Should_GenerateCorrectFiles()
        {
            // Arrange
            var path1 = "./DTO/UserTWithAddressT.cs";
            var sourceFile1 = new SourceFile
            {
                Path = path1,
                Text = File.ReadAllText(path1),
                AttributeToAddToClass = new ExtraAttribute
                {
                    Name = "FluentBuilder.AutoGenerateBuilder"
                }
            };

            var path2 = "./DTO/AddressT.cs";
            var sourceFile2 = new SourceFile
            {
                Path = path2,
                Text = File.ReadAllText(path2),
                AttributeToAddToClass = new ExtraAttribute
                {
                    Name = "FluentBuilder.AutoGenerateBuilder"
                }
            };

            // Act
            var result = _sut.Execute(Namespace, new[] { sourceFile1, sourceFile2 });

            // Assert
            result.Valid.Should().BeTrue();
            result.Files.Should().HaveCount(9);

            for (int i = 7; i < 9; i++)
            {
                var builder = result.Files[i];
                //builder.Path.Should().EndWith(x.fileName);

                var filename = Path.GetFileName(builder.Path);

                if (Write) File.WriteAllText($"../../../DTO/{filename}", builder.Text);
                builder.Text.Should().Be(File.ReadAllText($"../../../DTO/{filename}"));
            }
        }

        [Fact]
        public void GenerateFiles_For2GenericClasses_WithDefaultConstructor_Should_GenerateCorrectFiles()
        {
            // Arrange
            var builder1FileName = "FluentBuilderGeneratorTests.DTO.UserTWithAddressAndConstructorBuilder_T_.g.cs";
            var path1 = "./DTO/UserTWithAddressAndConstructor.cs";
            var sourceFile1 = new SourceFile
            {
                Path = path1,
                Text = File.ReadAllText(path1),
                AttributeToAddToClass = new ExtraAttribute
                {
                    Name = "FluentBuilder.AutoGenerateBuilder"
                }
            };

            var path2 = "./DTO/AddressT.cs";
            var sourceFile2 = new SourceFile
            {
                Path = path2,
                Text = File.ReadAllText(path2),
                AttributeToAddToClass = new ExtraAttribute
                {
                    Name = "FluentBuilder.AutoGenerateBuilder"
                }
            };

            // Act
            var result = _sut.Execute(Namespace, new[] { sourceFile1, sourceFile2 });

            // Assert
            result.Valid.Should().BeTrue();
            result.Files.Should().HaveCount(9);

            var builderForUserTWithAddressAndConstructor = result.Files[7];
            builderForUserTWithAddressAndConstructor.Path.Should().EndWith(builder1FileName);

            if (Write) File.WriteAllText($"../../../DTO/{builder1FileName}", builderForUserTWithAddressAndConstructor.Text);
            builderForUserTWithAddressAndConstructor.Text.Should().Be(File.ReadAllText($"../../../DTO/{builder1FileName}"));
        }

        [Fact]
        public void GenerateFiles_ForFluentBuilder_Should_GenerateCorrectFiles()
        {
            // Arrange
            var path = "./DTO2/MyAddressBuilder.cs";
            var sourceFile = new SourceFile
            {
                Path = path,
                Text = File.ReadAllText(path),
                AttributeToAddToClass = new ExtraAttribute
                {
                    Name = "FluentBuilder.AutoGenerateBuilder",
                    ArgumentList = "typeof(FluentBuilderGeneratorTests.DTO.Address)"
                }
            };

            // Act
            var result = _sut.Execute(Namespace, new[] { sourceFile });

            // Assert
            result.Valid.Should().BeTrue();
            result.Files.Should().HaveCount(12);

            for (int i = 7; i < 12; i++)
            {
                var builder = result.Files[i];
                //builder.Path.Should().EndWith(x.fileName);

                var filename = Path.GetFileName(builder.Path);

                if (Write) File.WriteAllText($"../../../DTO2/{filename}", builder.Text);
                builder.Text.Should().Be(File.ReadAllText($"../../../DTO2/{filename}"));
            }
        }
    }
}