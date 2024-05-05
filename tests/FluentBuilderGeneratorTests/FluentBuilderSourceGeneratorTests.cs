using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CSharp.SourceGenerators.Extensions;
using CSharp.SourceGenerators.Extensions.Models;
using FluentAssertions;
using FluentBuilderGenerator;
using FluentBuilderGeneratorTests.DTO;
using VerifyTests;
using VerifyXunit;
using Xunit;

namespace FluentBuilderGeneratorTests;

[UsesVerify]
public class FluentBuilderSourceGeneratorTests
{
    private const string Namespace = "FluentBuilderGeneratorTests";

    private const bool Write = true;

    private readonly FluentBuilderSourceGenerator _sut;

    public FluentBuilderSourceGeneratorTests()
    {
        _sut = new FluentBuilderSourceGenerator();
    }

    [ModuleInitializer]
    public static void ModuleInitializer() => VerifySourceGenerators.Enable();

    [Fact]
    public Task GenerateFiles_ForAClassWithoutAPublicConstructor_Should_Create_ErrorFile()
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
        result.Files.Should().HaveCount(9);
        result.Files[8].Path.Should().EndWith("Error.g.cs");

        // Verify
        var errorResult = result.GeneratorDriver.GetRunResult().Results.First().GeneratedSources.First(s => s.HintName.Contains("Error.g.cs"));
        return Verifier.Verify(errorResult);
    }

    [Fact]
    public void GenerateFiles_ForAClassWithPublicParameterlessConstructors_Should_GenerateCorrectFiles()
    {
        // Arrange
        var sourceFilePath = "./DTO/ThingWithParameterizedConstructor.cs";
        var sourceFile = new SourceFile
        {
            Path = sourceFilePath,
            Text = File.ReadAllText(sourceFilePath),
            AttributeToAddToClass = "FluentBuilder.AutoGenerateBuilder"
        };

        // Act
        var result = _sut.Execute(Namespace, new[] { sourceFile });

        // Assert
        result.Valid.Should().BeTrue();
        result.Files.Should().HaveCount(9);
        result.Files.Should().NotContain(r => r.Path.EndsWith("Error.g.cs"));

        for (int i = 8; i < 9; i++)
        {
            var builder = result.Files[i];

            var filename = Path.GetFileName(builder.Path);

            if (Write) File.WriteAllText($"../../../DTO/{filename}", builder.Text);
            builder.Text.Should().Be(File.ReadAllText($"../../../DTO/{filename}"));
        }
    }

    [Fact]
    public void GenerateFiles_ForAClassWithOnlyParameterizedConstructors_Should_GenerateCorrectFiles()
    {
        // Arrange
        var sourceFilePath = "./DTO/ThingWithOnlyParameterizedConstructors.cs";
        var sourceFile = new SourceFile
        {
            Path = sourceFilePath,
            Text = File.ReadAllText(sourceFilePath),
            AttributeToAddToClass = "FluentBuilder.AutoGenerateBuilder"
        };

        // Act
        var result = _sut.Execute(Namespace, new[] { sourceFile });

        // Assert
        result.Valid.Should().BeTrue();
        result.Files.Should().HaveCount(9);
        result.Files.Should().NotContain(r => r.Path.EndsWith("Error.g.cs"));

        for (int i = 8; i < 9; i++)
        {
            var builder = result.Files[i];

            var filename = Path.GetFileName(builder.Path);

            if (Write) File.WriteAllText($"../../../DTO/{filename}", builder.Text);
            builder.Text.Should().Be(File.ReadAllText($"../../../DTO/{filename}"));
        }

        var b0 = new ThingWithOnlyParameterizedConstructorsBuilder()
            .WithL(10000)
            .Build();

        var b1 = new ThingWithOnlyParameterizedConstructorsBuilder()
            .UsingConstructor(1, 2, "drie")
            .WithL(10000)
            .Build();

        var b2 = new ThingWithOnlyParameterizedConstructorsBuilder()
            .UsingConstructor(42)
            .WithL(10000)
            .Build();

        long init = 999;
        var instance = new ThingWithOnlyParameterizedConstructors(init);
        var b3 = new ThingWithOnlyParameterizedConstructorsBuilder()
            .UsingInstance(instance)
            .Build();
        b3.L.Should().Be(999);

        instance = new ThingWithOnlyParameterizedConstructors(init);
        var b4 = new ThingWithOnlyParameterizedConstructorsBuilder()
            .UsingInstance(instance)
            .WithL(888)
            .Build();
        b4.L.Should().Be(888);

        instance = new ThingWithOnlyParameterizedConstructors(1, 2, "three");
        instance.AsBuilder().WithL(12345).Build();
        instance.L.Should().Be(12345);
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
        result.Files.Should().HaveCount(10);

        for (int i = 8; i < 10; i++)
        {
            var builder = result.Files[i];

            var filename = Path.GetFileName(builder.Path);

            if (Write) File.WriteAllText($"../../../DTO/{filename}", builder.Text);
            builder.Text.Should().Be(File.ReadAllText($"../../../DTO/{filename}"));
        }
    }

    [Fact]
    public void GenerateFiles_ClassWithPrivateSetter_And_AccessibilityPublicAndPrivate_Should_GeneratePrivateSetMethodUsingReflection()
    {
        // Arrange
        var path = "./DTO/ClassWithPrivateSetter1.cs";
        var sourceFile = new SourceFile
        {
            Path = path,
            Text = File.ReadAllText(path),
            AttributeToAddToClass = new ExtraAttribute
            {
                Name = "FluentBuilder.AutoGenerateBuilder",
                ArgumentList = new[] { "FluentBuilderAccessibility.PublicAndPrivate" }
            }
        };

        // Act
        var result = _sut.Execute(Namespace, new[] { sourceFile });

        // Assert
        result.Valid.Should().BeTrue();
        result.Files.Should().HaveCount(9);

        var fileResult = result.Files[8];
        var filename = Path.GetFileName(fileResult.Path);

        File.WriteAllText($"../../../DTO/{filename}", fileResult.Text);

        var instance = new ClassWithPrivateSetter1Builder()
            .WithValue1(100)
            .WithValue2(42)
            .Build();

        instance.Value1.Should().Be(100);
        instance.Value2.Should().Be(42);
    }

    [Fact]
    public void GenerateFiles_ClassWithPrivateSetter_And_AccessibilityPublic_Should_Not_GeneratePrivateSetMethod()
    {
        // Arrange
        var path = "./DTO/ClassWithPrivateSetter2.cs";
        var sourceFile = new SourceFile
        {
            Path = path,
            Text = File.ReadAllText(path),
            AttributeToAddToClass = new ExtraAttribute
            {
                Name = "FluentBuilder.AutoGenerateBuilder",
                ArgumentList = new[] { "FluentBuilderAccessibility.Public" }
            }
        };

        // Act
        var result = _sut.Execute(Namespace, new[] { sourceFile });

        // Assert
        result.Valid.Should().BeTrue();
        result.Files.Should().HaveCount(9);

        var fileResult = result.Files[8];
        var filename = Path.GetFileName(fileResult.Path);

        fileResult.Text.Should().NotContain("InstanceType.GetProperty");

        File.WriteAllText($"../../../DTO/{filename}", fileResult.Text);

        //var instance = new ClassWithPrivateSetter2Builder()
        //    .WithValue2(42)
        //    .Build();

        //instance.Value1.Should().Be(default);
        //instance.Value2.Should().Be(42);
    }

    [Fact]
    public Task GenerateFiles_ClassWithFluentBuilderIgnore_Should_GenerateCorrectFiles()
    {
        // Arrange
        var path = "./DTO/ClassWithFluentBuilderIgnore.cs";
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
        result.Files.Should().HaveCount(9);

        // Verify
        var errorResult = result.GeneratorDriver.GetRunResult().Results.First().GeneratedSources[8];
        return Verifier.Verify(errorResult);
    }

    [Fact]
    public Task GenerateFiles_ForSimpleClass_MethodsIsWithOnly_Should_GenerateCorrectFiles()
    {
        // Arrange
        var path = "./DTO/SimpleClass.cs";
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
        result.Files.Should().HaveCount(9);

        // Verify
        var errorResult = result.GeneratorDriver.GetRunResult().Results.First().GeneratedSources[8];
        return Verifier.Verify(errorResult);
    }

    [Fact]
    public Task GenerateFiles_ForSimpleClass_MethodsIsWithAndWithout_Should_GenerateCorrectFiles()
    {
        // Arrange
        var path = "./DTO/SimpleClass.cs";
        var sourceFile = new SourceFile
        {
            Path = path,
            Text = File.ReadAllText(path),
            AttributeToAddToClass = new ExtraAttribute
            {
                Name = "FluentBuilder.AutoGenerateBuilder",
                ArgumentList = new[] { "FluentBuilderMethods.WithAndWithout" }
            }
        };

        // Act
        var result = _sut.Execute(Namespace, new[] { sourceFile });

        // Assert
        result.Valid.Should().BeTrue();
        result.Files.Should().HaveCount(9);

        // Verify
        var errorResult = result.GeneratorDriver.GetRunResult().Results.First().GeneratedSources[8];
        return Verifier.Verify(errorResult);
    }

    [Fact]
    public void GenerateFiles_ForClassWithArrayAndDictionaryProperty_Should_GenerateCorrectFiles()
    {
        // Arrange
        var fileNames = new[]
        {
            "FluentBuilder.Extra.g.cs",
            "FluentBuilder.BaseBuilder.g.cs",

            "FluentBuilder.ArrayBuilder.g.cs",
            "FluentBuilder.IEnumerableBuilder.g.cs",
            "FluentBuilder.IListBuilder.g.cs",
            "FluentBuilder.IReadOnlyCollectionBuilder.g.cs",
            "FluentBuilder.ICollectionBuilder.g.cs",
            "FluentBuilder.IDictionaryBuilder.g.cs",

            "BuilderGeneratorTests.DTO.AddressBuilder.g.cs",
            "BuilderGeneratorTests.DTO.Address_ArrayBuilder.g.cs",
            "BuilderGeneratorTests.DTO.Address_IEnumerableBuilder.g.cs",
            "BuilderGeneratorTests.DTO.Address_IListBuilder.g.cs",

            "BuilderGeneratorTests.DTO.Address_ICollectionBuilder.g.cs",
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
        result.Files.Should().HaveCount(9);

        var builder = result.Files[8];
        builder.Path.Should().EndWith(builderFileName);

        if (Write) File.WriteAllText($"../../../DTO/{builderFileName}", builder.Text);
        builder.Text.Should().Be(File.ReadAllText($"../../../DTO/{builderFileName}"));
    }

    [Fact]
    public void GenerateFiles_For1GenericClassTT_Should_GenerateCorrectFiles()
    {
        // Arrange
        var builderFileName = "FluentBuilderGeneratorTests.DTO.AddressTTBuilder_T1_T2_.g.cs";
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
        result.Files.Should().HaveCount(9);

        var builder = result.Files[8];
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
        result.Files.Should().HaveCount(10);

        for (int i = 8; i < 10; i++)
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
        result.Files.Should().HaveCount(10);

        var builderForUserTWithAddressAndConstructor = result.Files[8];
        builderForUserTWithAddressAndConstructor.Path.Should().EndWith(builder1FileName);

        if (Write) File.WriteAllText($"../../../DTO/{builder1FileName}", builderForUserTWithAddressAndConstructor.Text);
        builderForUserTWithAddressAndConstructor.Text.Should().Be(File.ReadAllText($"../../../DTO/{builder1FileName}"));
    }

    [Fact]
    public void GenerateFiles_ClassWithReferenceToItself_Should_GenerateCorrectFiles()
    {
        // Arrange
        var builderFileName = "FluentBuilderGeneratorTests.DTO.ThingBuilder.g.cs";
        var path = "./DTO/Thing.cs";
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
        result.Files.Should().HaveCount(9);

        var builder = result.Files[8];
        builder.Path.Should().EndWith(builderFileName);

        if (Write) File.WriteAllText($"../../../DTO/{builderFileName}", builder.Text);
        builder.Text.Should().Be(File.ReadAllText($"../../../DTO/{builderFileName}"));
    }

    [Fact]
    public void GenerateFiles_ClassWithFuncAndAction_Should_GenerateCorrectFiles()
    {
        // Arrange
        var builderFileName = "FluentBuilderGeneratorTests.DTO.ClassWithFuncAndActionBuilder.g.cs";
        var path = "./DTO/ClassWithFuncAndAction.cs";
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
        result.Files.Should().HaveCount(9);

        var builder = result.Files[8];
        builder.Path.Should().EndWith(builderFileName);

        if (Write) File.WriteAllText($"../../../DTO/{builderFileName}", builder.Text);
        builder.Text.Should().Be(File.ReadAllText($"../../../DTO/{builderFileName}"));
    }

    [Fact]
    public void GenerateFiles_ClassWithInit_Should_GenerateCorrectFiles()
    {
        // Arrange
        var builderFileName = "FluentBuilderGeneratorTests.DTO.ClassWithInitBuilder.g.cs";
        var path = "./DTO/ClassWithInit.cs";
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
        result.Files.Should().HaveCount(9);

        var builder = result.Files[8];
        builder.Path.Should().EndWith(builderFileName);

        if (Write) File.WriteAllText($"../../../DTO/{builderFileName}", builder.Text);
        builder.Text.Should().Be(File.ReadAllText($"../../../DTO/{builderFileName}"));
    }

    [Fact]
    public void GenerateFiles_ClassWithPropertyWhichHasAValue_Should_GenerateCorrectFiles()
    {
        // Arrange
        var builderFileName = "FluentBuilderGeneratorTests.DTO.ClassWithPropertyValueSetBuilder.g.cs";
        var path = "./DTO/ClassWithPropertyValueSet.cs";
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
        result.Files.Should().HaveCount(9);

        var builder = result.Files[8];
        builder.Path.Should().EndWith(builderFileName);

        if (Write) File.WriteAllText($"../../../DTO/{builderFileName}", builder.Text);
        builder.Text.Should().Be(File.ReadAllText($"../../../DTO/{builderFileName}"));

        //var b = new ClassWithCultureInfoBuilder();
        //var c = b.Build();
    }

    [Fact]
    public void GenerateFiles_ForFluentBuilders_Should_GenerateCorrectFiles()
    {
        // Arrange
        var pathUser = "./DTO2/MyUserBuilder.cs";
        var sourceFileUser = new SourceFile
        {
            Path = pathUser,
            Text = File.ReadAllText(pathUser),
            AttributeToAddToClass = new ExtraAttribute
            {
                Name = "FluentBuilder.AutoGenerateBuilder",
                ArgumentList = "typeof(FluentBuilderGeneratorTests.DTO.User)"
            }
        };

        var pathOption = "./DTO2/MyOptionBuilder.cs";
        var sourceFileOption = new SourceFile
        {
            Path = pathOption,
            Text = File.ReadAllText(pathOption),
            AttributeToAddToClass = new ExtraAttribute
            {
                Name = "FluentBuilder.AutoGenerateBuilder",
                ArgumentList = "typeof(FluentBuilderGeneratorTests.DTO.Option)"
            }
        };

        // Act
        var result = _sut.Execute(Namespace, new[] { sourceFileUser, sourceFileOption });

        // Assert
        result.Valid.Should().BeTrue();
        result.Files.Should().HaveCount(11);

        for (int i = 8; i < result.Files.Count; i++)
        {
            var builder = result.Files[i];
            //builder.Path.Should().EndWith(x.fileName);

            var filename = Path.GetFileName(builder.Path);

            if (Write) File.WriteAllText($"../../../DTO2/{filename}", builder.Text);
            builder.Text.Should().Be(File.ReadAllText($"../../../DTO2/{filename}"));
        }
    }

    [Fact]
    public void GenerateFiles_For2ClassesWithListBuilder_Should_GenerateCorrectFiles()
    {
        // Arrange
        var pathOther = "./DTO_OtherNamespace/ClassOnOtherNamespace.cs";
        var sourceFileOther = new SourceFile
        {
            Path = pathOther,
            Text = File.ReadAllText(pathOther),
            AttributeToAddToClass = "FluentBuilder.AutoGenerateBuilder"
        };

        var pathTest = "./DTO/Test.cs";
        var sourceFileTest = new SourceFile
        {
            Path = pathTest,
            Text = File.ReadAllText(pathTest),
            AttributeToAddToClass = "FluentBuilder.AutoGenerateBuilder"
        };

        // Act
        var result = _sut.Execute(Namespace, new[] { sourceFileTest, sourceFileOther });

        // Assert
        result.Valid.Should().BeTrue();
        result.Files.Should().HaveCount(11);

        for (int i = 8; i < result.Files.Count; i++)
        {
            var builder = result.Files[i];

            var filename = Path.GetFileName(builder.Path);

            if (Write) File.WriteAllText($"../../../DTO/{filename}", builder.Text);
            builder.Text.Should().Be(File.ReadAllText($"../../../DTO/{filename}"));
        }
    }

    // #60
    [Fact]
    public void GenerateFiles_UseGlobalForLookALikeNamespaces()
    {
        // Arrange
        const string builderFileName = "FluentBuilderGeneratorTests.Issue60.MyEntityBuilder.g.cs";
        const string path = "./Issue60/MyEntity.cs";
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
        result.Files.Should().HaveCount(9);

        var builder = result.Files[8];
        builder.Path.Should().EndWith(builderFileName);

        if (Write) File.WriteAllText($"../../../Issue60/Generated/{builderFileName}", builder.Text);
        // builder.Text.Should().Be(File.ReadAllText($"../../../Issue60/Generated/{builderFileName}"));
    }
}