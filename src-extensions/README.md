# CSharp.SourceGenerators.Extensions

[![NuGet Badge](https://buildstats.info/nuget/CSharp.SourceGenerators.Extensions)](https://www.nuget.org/packages/CSharp.SourceGenerators.Extensions)

## Info
This package provides an `Execute` extension method on a `ISourceGenerator` which can be directly called in a Unit Test.

### Example
``` c#
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
```

## :books: References
- https://github.com/dotnet/roslyn/blob/main/docs/features/source-generators.cookbook.md
- https://notanaverageman.github.io/2020/12/07/cs-source-generators-cheatsheet.html