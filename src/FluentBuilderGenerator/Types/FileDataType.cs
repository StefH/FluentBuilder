// ReSharper disable InconsistentNaming
namespace FluentBuilderGenerator.Types;

internal enum FileDataType : byte
{
    None,

    Attribute,

    Base,

    Builder,

    ArrayBuilder,

    IEnumerableBuilder,

    IListBuilder,

    ICollectionBuilder,

    IDictionaryBuilder,
}