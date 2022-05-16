using FluentBuilderGenerator.Types;

namespace FluentBuilderGenerator.Helpers;

// ReSharper disable once InconsistentNaming
internal static class IEnumerableBuilderHelper
{
    public static (string GenericType, string ToArray) GetGenericTypeAndToArray(FileDataType dataType, string t = "T")
    {
        switch (dataType)
        {
            case FileDataType.ArrayBuilder:
                return ($"{t}[]", ".ToArray()");

            case FileDataType.IEnumerableBuilder:
                return ($"IEnumerable<{t}>", string.Empty);

            case FileDataType.IReadOnlyCollectionBuilder:
                return ($"IReadOnlyCollection<{t}>", string.Empty);

            case FileDataType.ICollectionBuilder:
                return ($"ICollection<{t}>", string.Empty);

            case FileDataType.IListBuilder:
                return ($"IList<{t}>", string.Empty);

            default:
                throw new ArgumentException();
        }
    }
}