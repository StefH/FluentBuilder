using FluentBuilderGenerator.Types;

namespace FluentBuilderGenerator.Helpers;

// ReSharper disable once InconsistentNaming
internal static class IEnumerableBuilderHelper
{
    public static (string GenericType, string ToArray) GetGenericTypeAndToArray(FileDataType dataType, string t = "T")
    {
        return dataType switch
        {
            FileDataType.ArrayBuilder => ($"{t}[]", ".ToArray()"),
            FileDataType.ICollectionBuilder => ($"ICollection<{t}>", string.Empty),
            FileDataType.IEnumerableBuilder => ($"IEnumerable<{t}>", string.Empty),
            FileDataType.IListBuilder => ($"IList<{t}>", string.Empty),
            FileDataType.IReadOnlyCollectionBuilder => ($"IReadOnlyCollection<{t}>", string.Empty),
            FileDataType.IReadOnlyListBuilder => ($"IReadOnlyList<{t}>", string.Empty),
            _ => throw new ArgumentException()
        };
    }
}