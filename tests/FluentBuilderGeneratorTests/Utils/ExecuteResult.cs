using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace FluentBuilderGeneratorTests.Utils
{
    public class ExecuteResult
    {
        public ImmutableArray<SyntaxTree>? SyntaxTrees { get; init; }

        public IReadOnlyList<string> ErrorMessages { get; init; } = new List<string>();

        public bool Valid => ErrorMessages.Count == 0;
    }
}