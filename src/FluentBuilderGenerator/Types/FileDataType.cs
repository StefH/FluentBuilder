namespace FluentBuilderGenerator.Types;

internal enum FileDataType
{
    Attribute,

    Base,

    Builder,

    // ReSharper disable once InconsistentNaming
    IEnumerableBuilder,
    
    // ReSharper disable once InconsistentNaming
    ICollectionBuilder,

    // ReSharper disable once InconsistentNaming
    IDictionaryBuilder,
}