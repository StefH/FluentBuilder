using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using FluentAssertions;
using FluentBuilderGenerator.FileGenerators;
using FluentBuilderGenerator.Models;
using FluentBuilderGenerator.SyntaxReceiver;
using FluentBuilderGenerator.Types;
using FluentBuilderGenerator.Wrappers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Moq;
using Xunit;

namespace FluentBuilderGeneratorTests.FileGenerator;

public class FluentBuilderClassesGeneratorTests
{
    private const bool Write = true;

    private readonly Mock<IGeneratorExecutionContextWrapper> _contextMock;
    private readonly Mock<IAutoGenerateBuilderSyntaxReceiver> _syntaxReceiverMock;

    private readonly FluentBuilderClassesGenerator _sut;

    public FluentBuilderClassesGeneratorTests()
    {
        _contextMock = new Mock<IGeneratorExecutionContextWrapper>();
        _contextMock.SetupGet(c => c.AssemblyName).Returns("FluentBuilderGeneratorTests");
        _contextMock.SetupGet(c => c.SupportsNullable).Returns(true);
        _contextMock.SetupGet(c => c.NullableEnabled).Returns(true);

        _syntaxReceiverMock = new Mock<IAutoGenerateBuilderSyntaxReceiver>();

        _sut = new FluentBuilderClassesGenerator(_contextMock.Object, _syntaxReceiverMock.Object);
    }

    [Fact]
    public void GenerateFiles_WhenNoClassesFoundByReceiver_Should_NotGenerateFiles()
    {
        // Arrange
        _syntaxReceiverMock.SetupGet(r => r.CandidateFluentDataItems).Returns(new List<FluentData>());

        // Act
        var result = _sut.GenerateFiles();

        // Assert
        result.Should().BeEmpty();

        // Verify
        _syntaxReceiverMock.Verify(r => r.CandidateFluentDataItems, Times.Once());
        _syntaxReceiverMock.VerifyNoOtherCalls();

        _contextMock.VerifyNoOtherCalls();
    }

    [Fact(Skip = "needs fix")]
    public void GenerateFiles_WhenOneClassIsFoundByReceiver_Should_GenerateOneFile()
    {
        // Arrange : SyntaxReceiverMock
        var syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText("./DTO/User.cs"));
        var root = syntaxTree.GetRoot();
        var @namespace = root.ChildNodes().OfType<NamespaceDeclarationSyntax>().Single();
        var @class = @namespace.ChildNodes().OfType<ClassDeclarationSyntax>().Single();
        var properties = @class.ChildNodes().OfType<PropertyDeclarationSyntax>().ToList();

        var fluentData = new FluentData
        {
            BuilderType = BuilderType.Generated,
            Namespace = "FluentBuilderGeneratorTests.DTO",
            ShortBuilderClassName = "UserBuilder",
            FullBuilderClassName = "FluentBuilderGeneratorTests.DTO.UserBuilder",
            FullRawTypeName = "FluentBuilderGeneratorTests.DTO.User",
            ShortTypeName = "User",
            MetadataName = "FluentBuilderGeneratorTests.DTO.User",
            Usings = new List<string>()
        };
        _syntaxReceiverMock.SetupGet(r => r.CandidateFluentDataItems).Returns(new[] { fluentData });

        // Arrange : ContextMock
        var namespaceSymbolMock = new Mock<INamespaceSymbol>();
        namespaceSymbolMock.Setup(n => n.ToString()).Returns(@namespace.Name.ToString());

        var namedTypeSymbolMock = new Mock<INamedTypeSymbol>();
        namedTypeSymbolMock.SetupGet(n => n.Name).Returns("User");
        namedTypeSymbolMock.SetupGet(n => n.TypeArguments).Returns(ImmutableArray.Create<ITypeSymbol>());

        var membersMock = properties.Select(p =>
            {
                var setMethodMock = new Mock<IMethodSymbol>();

                var typeSymbol = new Mock<ITypeSymbol>();
                typeSymbol.SetupGet(t => t.Name).Returns(p.Type.ToString());
                typeSymbol.Setup(t => t.ToString()).Returns(p.Type.ToString());
                typeSymbol.SetupGet(ts => ts.AllInterfaces).Returns(ImmutableArray.Create<INamedTypeSymbol>());

                var propertySymbolMock = new Mock<IPropertySymbol>();
                propertySymbolMock.SetupGet(pt => pt.CanBeReferencedByName).Returns(true);
                propertySymbolMock.SetupGet(pt => pt.Name).Returns(p.Identifier.ValueText);
                propertySymbolMock.SetupGet(pt => pt.SetMethod).Returns(setMethodMock.Object);
                propertySymbolMock.SetupGet(pt => pt.Type).Returns(typeSymbol.Object);

                return propertySymbolMock;
            })
            .Select(p => (ISymbol)p.Object)
            .ToImmutableArray();

        var originalDefinitionMock = new Mock<INamedTypeSymbol>();
        originalDefinitionMock.Setup(o => o.ToString()).Returns("FluentBuilderGeneratorTests.DTO.User");

        var classSymbolMock = new Mock<INamedTypeSymbol>();
        classSymbolMock.SetupGet(n => n.ContainingNamespace).Returns(namespaceSymbolMock.Object);
        classSymbolMock.SetupGet(n => n.Name).Returns("User");
        classSymbolMock.SetupGet(n => n.OriginalDefinition).Returns(originalDefinitionMock.Object);
        classSymbolMock.Setup(n => n.GetMembers()).Returns(membersMock);
        classSymbolMock.SetupGet(n => n.TypeArguments).Returns(ImmutableArray.Create<ITypeSymbol>());

        var classSymbolConstructorMock = new Mock<IMethodSymbol>();
        classSymbolConstructorMock.SetupGet(c => c.DeclaredAccessibility).Returns(Accessibility.Public);
        classSymbolConstructorMock.SetupGet(c => c.Parameters).Returns(ImmutableArray<IParameterSymbol>.Empty);
        classSymbolMock.SetupGet(n => n.Constructors).Returns(ImmutableArray.Create(new[] { classSymbolConstructorMock.Object }));

        var classSymbol = new ClassSymbol
        {
            Type = FileDataType.Builder,
            FluentData = fluentData,
            //BuilderNamespace = fluentData.Namespace,
            //BuilderClassName = fluentData.ShortBuilderClassName,
            //FullBuilderClassName = fluentData.FullBuilderClassName,
            NamedTypeSymbol = classSymbolMock.Object
        };
        _contextMock.Setup(c => c.TryGetNamedTypeSymbolByFullMetadataName(It.IsAny<FluentData>(), out classSymbol)).Returns(true);

        // Act
        var result = _sut.GenerateFiles().ToList();

        // Assert
        result.Should().HaveCount(1);
        result[0].FileName.Should().Be("FluentBuilderGeneratorTests.DTO.UserBuilder.g.cs");

        var generated = result[0].Text;
        generated.Should().NotBeEmpty();

        if (Write) File.WriteAllText("../../../DTO/FluentBuilderGeneratorTests.DTO.UserBuilder.g.cs", generated);

        var generatedCode = CSharpSyntaxTree.ParseText(generated);
        var expectedCode = CSharpSyntaxTree.ParseText(File.ReadAllText("../../../DTO/FluentBuilderGeneratorTests.DTO.UserBuilder.g.cs"));
        generatedCode.Should().BeEquivalentTo(expectedCode);

        // Verify
        _syntaxReceiverMock.Verify(r => r.CandidateFluentDataItems, Times.Once());
        _syntaxReceiverMock.VerifyNoOtherCalls();

        _contextMock.Verify(c => c.TryGetNamedTypeSymbolByFullMetadataName(It.IsAny<FluentData>(), out It.Ref<ClassSymbol?>.IsAny), Times.Once());
        _contextMock.Verify(c => c.AssemblyName, Times.Once());
        _contextMock.Verify(c => c.SupportsNullable, Times.AtLeast(1));
        _contextMock.VerifyNoOtherCalls();
    }
}