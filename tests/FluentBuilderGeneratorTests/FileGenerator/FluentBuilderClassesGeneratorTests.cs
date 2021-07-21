using System.Collections.Generic;
using FluentAssertions;
using FluentBuilderGenerator;
using FluentBuilderGenerator.FileGenerators;
using FluentBuilderGenerator.Wrappers;
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
    }
}