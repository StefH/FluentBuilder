using System.Collections.Generic;
using AnyOfTypes;

namespace CSharp.SourceGenerators.Extensions.Models
{
    public class ExtraAttribute
    {
        /// <summary>
        /// The name of the attribute to add
        /// </summary>
        public string Name { get; init; } = default!;

        /// <summary>
        /// The arguments to add to the argument.
        ///
        /// This can be a <see cref="string"/> or an <see cref="IEnumerable<string>"/>.
        /// </summary>
        public AnyOf<string, IEnumerable<string>>? ArgumentList { get; init; } = default!;
    }
}