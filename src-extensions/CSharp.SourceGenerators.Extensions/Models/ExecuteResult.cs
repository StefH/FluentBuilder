using System.Collections.Generic;

namespace CSharp.SourceGenerators.Extensions.Models
{
    public class ExecuteResult
    {
        /// <summary>
        /// A list of generated files.
        /// </summary>
        public IReadOnlyList<FileResult> Files { get; set; } = new List<FileResult>();

        /// <summary>
        /// A list of Errors
        /// </summary>
        public IReadOnlyList<string> ErrorMessages { get; set; } = new List<string>();

        /// <summary>
        /// A list of Warnings
        /// </summary>
        public IReadOnlyList<string> WarningMessages { get; set; } = new List<string>();

        /// <summary>
        /// Is the result valid.
        /// </summary>
        public bool Valid => ErrorMessages.Count == 0;
    }
}