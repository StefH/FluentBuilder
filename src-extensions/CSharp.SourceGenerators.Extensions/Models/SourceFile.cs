using AnyOfTypes;

namespace CSharp.SourceGenerators.Extensions.Models
{
    public class SourceFile
    {
        /// <summary>
        /// The source-file path.
        /// </summary>
        public string Path { get; init; } = default!;

        /// <summary>
        /// The source-file C# code.
        /// </summary>
        public string Text { get; init; } = default!;

        /// <summary>
        /// Optionally add this attribute to the source-classes.
        /// This is useful when your SourceGenerator uses a ISyntaxReceiver to process only files with a certain attribute.
        ///
        /// This can be a
        /// - <see cref="string"/> which defines the attribute-name
        /// - <see cref="ClassAttribute"/> which is an class which defines the attribute-name and optionally some parameters
        /// </summary>
        public AnyOf<string, ClassAttribute>? AttributeToAddToClasses { get; init; }
    }
}