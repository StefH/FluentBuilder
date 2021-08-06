using Microsoft.CodeAnalysis;

namespace FluentBuilderGeneratorTests.Utils
{
    public class FileResult
    {
        public SyntaxTree SyntaxTree { get; init; }

        public string Path { get; init; }

        public string Text { get; init; }
    }
}