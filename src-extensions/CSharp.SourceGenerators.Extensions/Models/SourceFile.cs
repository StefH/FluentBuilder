namespace CSharp.SourceGenerators.Extensions.Models
{
    public class SourceFile
    {
        /// <summary>
        /// The source-file path.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// The source-file C# code.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Add this attribute to the source-classes.
        /// This is useful when your SourceGenerator uses a ISyntaxReceiver to process only files with a certain attribute.
        /// </summary>
        public string? AttributeToAddToClasses { get; set; }
    }
}