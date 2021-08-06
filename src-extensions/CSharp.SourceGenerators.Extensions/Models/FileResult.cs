using Microsoft.CodeAnalysis;

namespace CSharp.SourceGenerators.Extensions.Models
{
    public class FileResult
    {
        /// <summary>
        /// The result-file path.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// The result-file C# code.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The complete <see cref="SyntaxTree"/> from the result-file.
        /// </summary>
        public SyntaxTree SyntaxTree { get; set; }
    }
}