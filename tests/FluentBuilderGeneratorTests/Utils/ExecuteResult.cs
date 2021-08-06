using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace FluentBuilderGeneratorTests.Utils
{
    public class ExecuteResult
    {
        public IReadOnlyList<SyntaxTree>? SyntaxTrees { get; init; }

        public IReadOnlyList<string> ErrorMessages { get; init; } = new List<string>();

        public bool Valid => ErrorMessages.Count == 0;
    }
}