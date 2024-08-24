// ReSharper disable InconsistentNaming
namespace FluentBuilderGenerator.Types;

internal enum FluentTypeKind : byte
{
    None,

    Array,

    ICollection,

    IDictionary,

    IEnumerable,

    IList,

    IReadOnlyCollection,

    IReadOnlyList,
    
    ReadOnlyCollection,

    String,

    Other
}