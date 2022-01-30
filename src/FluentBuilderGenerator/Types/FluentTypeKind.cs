using Microsoft.CodeAnalysis;

namespace FluentBuilderGenerator.Types;

internal enum FluentTypeKind
{
    Array,

    IEnumerable,

    IDictionary,

    Other
}