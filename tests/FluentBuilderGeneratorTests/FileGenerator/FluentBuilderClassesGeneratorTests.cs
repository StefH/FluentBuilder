using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using FluentAssertions;
using FluentBuilderGenerator.FileGenerators;
using FluentBuilderGenerator.SyntaxReceiver;
using FluentBuilderGenerator.Wrappers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Moq;
using Xunit;

namespace FluentBuilderGeneratorTests.FileGenerator
{
    public class FluentBuilderClassesGeneratorTests
    {
        private const bool Write = true;

        private readonly Mock<IGeneratorExecutionContextWrapper> _contextMock;
        private readonly Mock<IAutoGenerateBuilderSyntaxReceiver> _receiverMock;

        private readonly FluentBuilderClassesGenerator _sut;

        public FluentBuilderClassesGeneratorTests()
        {
            _contextMock = new Mock<IGeneratorExecutionContextWrapper>();
            _receiverMock = new Mock<IAutoGenerateBuilderSyntaxReceiver>();

            _sut = new FluentBuilderClassesGenerator(_contextMock.Object, _receiverMock.Object, true);
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

            // Verify
            _receiverMock.Verify(r => r.CandidateClasses, Times.Once());
            _receiverMock.VerifyNoOtherCalls();

            _contextMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void GenerateFiles_WhenOneClassIsFoundByReceiver_Should_GenerateOneFile()
        {
            // Arrange : ReceiverMock
            var syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText("./DTO/User.cs"));
            var root = syntaxTree.GetRoot();
            var @namespace = root.ChildNodes().OfType<NamespaceDeclarationSyntax>().Single();
            var @class = @namespace.ChildNodes().OfType<ClassDeclarationSyntax>().Single();
            var properties = @class.ChildNodes().OfType<PropertyDeclarationSyntax>().ToList();

            _receiverMock.SetupGet(r => r.CandidateClasses).Returns(new[] { @class });

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
            classSymbolMock.SetupGet(n => n.Name).Returns(@class.Identifier.Value.ToString());
            classSymbolMock.SetupGet(n => n.OriginalDefinition).Returns(originalDefinitionMock.Object);
            classSymbolMock.Setup(n => n.GetMembers()).Returns(membersMock);
            classSymbolMock.SetupGet(n => n.TypeArguments).Returns(ImmutableArray.Create<ITypeSymbol>());

            _contextMock.Setup(c => c.GetTypeByMetadataName(It.IsAny<string>())).Returns(classSymbolMock.Object);

            // Act
            var result = _sut.GenerateFiles().ToList();

            // Assert
            result.Should().HaveCount(1);
            result[0].FileName.Should().Be("FluentBuilderGeneratorTests.DTO.User_Builder.g.cs");

            var generated = result[0].Text;
            generated.Should().NotBeEmpty();

            if (Write) File.WriteAllText("../../../DTO/FluentBuilderGeneratorTests.DTO.User_Builder.g.cs", generated);

            var generatedCode = CSharpSyntaxTree.ParseText(generated);
            var expectedCode = CSharpSyntaxTree.ParseText(File.ReadAllText("../../../DTO/FluentBuilderGeneratorTests.DTO.User_Builder.g.cs"));
            generatedCode.Should().BeEquivalentTo(expectedCode);

            // Verify
            _receiverMock.Verify(r => r.CandidateClasses, Times.Once());
            _receiverMock.VerifyNoOtherCalls();

            //_contextMock.Verify(c => c.GetTypeByMetadataName(It.IsAny<string>()), Times.Once());
            //_contextMock.VerifyNoOtherCalls();
        }
    }
}