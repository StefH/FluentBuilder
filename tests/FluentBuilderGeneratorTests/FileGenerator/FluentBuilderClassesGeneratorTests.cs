using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using FluentAssertions;
using FluentBuilderGenerator;
using FluentBuilderGenerator.FileGenerators;
using FluentBuilderGenerator.Wrappers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Moq;
using Xunit;

namespace FluentBuilderGeneratorTests
{
    public class FluentBuilderClassesGeneratorTests
    {
        private readonly Mock<IGeneratorExecutionContextWrapper> _contextMock;
        private readonly Mock<IAutoGenerateBuilderSyntaxReceiver> _receiverMock;

        private readonly FluentBuilderClassesGenerator _sut;

        public FluentBuilderClassesGeneratorTests()
        {
            _contextMock = new Mock<IGeneratorExecutionContextWrapper>();
            _receiverMock = new Mock<IAutoGenerateBuilderSyntaxReceiver>();

            _sut = new FluentBuilderClassesGenerator(_contextMock.Object, _receiverMock.Object);
        }

        [Fact]
        public void GenerateFiles_WhenNoClassesFoundByReceiver_Should_NotGenerateFiles()
        {
            // Arrange
            _receiverMock.SetupGet(r => r.CandidateClasses).Returns(new List<ClassDeclarationSyntax>());

            // Act
            var result = _sut.GenerateFiles();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void GenerateFiles_WhenOneClassIsFoundByReceiver_Should_GenerateOneFile()
        {
            // Arrange : ReceiverMock
            var syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText("./DTO/UserDTO.cs"));
            var root = syntaxTree.GetRoot();
            var @namespace = root.ChildNodes().OfType<NamespaceDeclarationSyntax>().Single();
            var @class = @namespace.ChildNodes().OfType<ClassDeclarationSyntax>().Single();
            var @properties = @class.ChildNodes().OfType<PropertyDeclarationSyntax>().ToList();

            _receiverMock.SetupGet(r => r.CandidateClasses).Returns(new[] { @class });

            // Arrange : ContextMock
            var namespaceSymbolMock = new Mock<INamespaceSymbol>();
            namespaceSymbolMock.Setup(n => n.ToString()).Returns(@namespace.Name.ToString());

            var membersMock = @properties.Select(p =>
            {
                var setMethodMock = new Mock<IMethodSymbol>();

                var propertySymbolMock = new Mock<IPropertySymbol>();
                propertySymbolMock.SetupGet(p => p.CanBeReferencedByName).Returns(true);
                propertySymbolMock.SetupGet(p => p.Name).Returns(p.Identifier.ValueText);
                propertySymbolMock.SetupGet(p => p.SetMethod).Returns(setMethodMock.Object);

                return propertySymbolMock;
            })
            .Select(p => (ISymbol)p.Object)
            .ToImmutableArray();

            var classSymbolMock = new Mock<INamedTypeSymbol>();
            classSymbolMock.SetupGet(n => n.ContainingNamespace).Returns(namespaceSymbolMock.Object);
            classSymbolMock.SetupGet(n => n.Name).Returns(@class.Identifier.Value.ToString());
            classSymbolMock.Setup(n => n.GetMembers()).Returns(membersMock);

            _contextMock.Setup(c => c.GetTypeByMetadataName(It.IsAny<string>())).Returns(classSymbolMock.Object);

            // Act
            var result = _sut.GenerateFiles().ToList();

            // Assert
            result.Should().HaveCount(1);
            result[0].FileName.Should().Be("UserDto_Builder.cs");

            var generated = result[0].Text;
            generated.Should().NotBeEmpty();

            var generatedCode = CSharpSyntaxTree.ParseText(generated);
            var expectedCode = CSharpSyntaxTree.ParseText(File.ReadAllText("./DTO/UserDtoBuilder.txt"));
            generatedCode.Should().BeEquivalentTo(expectedCode);
        }
    }
}