using System.ComponentModel;
using FluentBuilderGenerator.Types;

namespace FluentBuilderGenerator.Extensions;

internal static class EnumExtensions
{
    private static readonly IDictionary<FluentTypeKind, FileDataType> KindToFile = new Dictionary<FluentTypeKind, FileDataType>
    {
        { FluentTypeKind.Array, FileDataType.ArrayBuilder },
        { FluentTypeKind.IEnumerable, FileDataType.IEnumerableBuilder },
        { FluentTypeKind.ICollection, FileDataType.ICollectionBuilder },
        { FluentTypeKind.IList, FileDataType.IListBuilder },
        { FluentTypeKind.IReadOnlyCollection, FileDataType.IReadOnlyCollectionBuilder },
        { FluentTypeKind.IReadOnlyList, FileDataType.IReadOnlyListBuilder },
    };
    private static readonly IDictionary<FileDataType, FluentTypeKind> FileToKind = KindToFile.ToDictionary(x => x.Value, x => x.Key);

    public static FileDataType ToFileDataType(this FluentTypeKind kind)
    {
        if (KindToFile.TryGetValue(kind, out var value))
        {
            return value;
        }

        throw new InvalidEnumArgumentException();
    }

    public static FluentTypeKind ToTypeKind(this FileDataType fileDataType)
    {
        if (FileToKind.TryGetValue(fileDataType, out var value))
        {
            return value;
        }

        throw new InvalidEnumArgumentException();
    }
}