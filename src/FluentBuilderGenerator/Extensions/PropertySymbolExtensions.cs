using FluentBuilderGenerator.Types;
using Microsoft.CodeAnalysis;

namespace FluentBuilderGenerator.Extensions;

internal static class PropertySymbolExtensions
{
    // ReSharper disable once InconsistentNaming
    private static readonly FluentTypeKind[] IEnumerableKinds =
    {
        FluentTypeKind.IEnumerable,
        FluentTypeKind.IList,
        FluentTypeKind.ICollection
    };

    internal static bool IsPrivateSettable(this IPropertySymbol property)
    {
        return property.SetMethod is { IsInitOnly: false, DeclaredAccessibility: Accessibility.Private };
    }

    internal static bool IsPublicSettable(this IPropertySymbol property)
    {
        return property.SetMethod is { IsInitOnly: false, DeclaredAccessibility: Accessibility.Public };
    }

    internal static bool TryGetIDictionaryElementTypes(this IPropertySymbol property, out (INamedTypeSymbol key, INamedTypeSymbol value)? tuple)
    {
        var type = property.Type.GetFluentTypeKind();

        if (type == FluentTypeKind.IDictionary && property.Type is INamedTypeSymbol namedTypeSymbol)
        {
            if (namedTypeSymbol.IsGenericType && namedTypeSymbol.TypeArguments.Length == 2)
            {
                if (namedTypeSymbol.TypeArguments[0] is INamedTypeSymbol key && namedTypeSymbol.TypeArguments[1] is INamedTypeSymbol value)
                {
                    tuple = new(key, value);
                    return true;
                }
            }
        }

        tuple = default;
        return false;
    }

    internal static bool TryGetIEnumerableElementType(
        this IPropertySymbol property,
        out INamedTypeSymbol? elementNamedTypeSymbol,
        out FluentTypeKind kind)
    {
        elementNamedTypeSymbol = null;
        kind = property.Type.GetFluentTypeKind();

        if (kind == FluentTypeKind.Array)
        {
            var elementTypeSymbol = (IArrayTypeSymbol)property.Type;
            if ((elementTypeSymbol.ElementType.IsClass() || elementTypeSymbol.ElementType.IsStruct()) && elementTypeSymbol.ElementType is INamedTypeSymbol arrayNamedTypedSymbol)
            {
                elementNamedTypeSymbol = arrayNamedTypedSymbol;
                return true;
            }
        }
        else if (IEnumerableKinds.Contains(kind) && property.Type is INamedTypeSymbol namedTypeSymbol)
        {
            if (namedTypeSymbol.IsGenericType)
            {
                if (namedTypeSymbol.TypeArguments.FirstOrDefault() is INamedTypeSymbol genericNamedTypeSymbol)
                {
                    elementNamedTypeSymbol = genericNamedTypeSymbol;
                    return true;
                }
            }

            elementNamedTypeSymbol = namedTypeSymbol;
            return true;
        }

        return false;
    }
}