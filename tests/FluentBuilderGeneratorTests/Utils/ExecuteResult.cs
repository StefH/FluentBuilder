using System.Collections.Generic;

namespace FluentBuilderGeneratorTests.Utils
{
    public class ExecuteResult
    {
        public IReadOnlyList<FileResult> Files { get; init; } = new List<FileResult>();

        public IReadOnlyList<string> ErrorMessages { get; init; } = new List<string>();

        public bool Valid => ErrorMessages.Count == 0;
    }
}