// ReSharper disable InconsistentNaming
namespace FluentBuilderGenerator.Types;

internal enum FileDataType : byte
{
    None,

    ArrayBuilder,

    Attribute,

    Base,

    Builder,

    ICollectionBuilder,

    IDictionaryBuilder,

    IEnumerableBuilder,

    IListBuilder,

    IReadOnlyCollectionBuilder,

    IReadOnlyListBuilder,
}