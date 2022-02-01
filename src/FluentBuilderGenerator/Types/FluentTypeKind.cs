// ReSharper disable InconsistentNaming
namespace FluentBuilderGenerator.Types;

internal enum FluentTypeKind : byte
{
    None,

    String,

    Array,

    IEnumerable,

    ICollection,

    IList,

    IDictionary,

    Other
}