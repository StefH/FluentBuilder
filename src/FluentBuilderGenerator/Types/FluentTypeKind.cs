// ReSharper disable InconsistentNaming
namespace FluentBuilderGenerator.Types;

internal enum FluentTypeKind : byte
{
    None,

    String,

    Array,

    IEnumerable,

    ICollection,

    IReadOnlyCollection,
    
    IList,

    IDictionary,

    Other
}