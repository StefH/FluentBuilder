This package provides an `Execute` extension method on a `ISourceGenerator` which can be directly called in a Unit Test.

**Example**
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
        AttributeToAddToClasses = "FluentBuilder.AutoGenerateBuilder"
    };

    // Act
    var result = _sut.Execute(new[] { sourceFile });

    // Asert
    result.Valid.Should().BeTrue();
    result.Files.Should().HaveCount(3);

    var builder = result.Files[2];
    builder.Path.Should().EndWith("FluentBuilderGeneratorTests.DTO.User_Builder.g.cs");
    builder.Text.Should().NotBeNullOrEmpty();
}
```