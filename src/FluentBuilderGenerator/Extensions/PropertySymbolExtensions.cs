using FluentBuilderGenerator.Types;
using Microsoft.CodeAnalysis;

namespace FluentBuilderGenerator.Extensions;

internal static class PropertySymbolExtensions
{
    internal static bool TryGetIEnumerableElementType(this IPropertySymbol property, out INamedTypeSymbol? elementNamedTypeSymbol)
    {
        elementNamedTypeSymbol = null;

        var type = property.Type.GetFluentTypeKind();

        if (type == FluentTypeKind.Array)
        {
            var elementTypeSymbol = (IArrayTypeSymbol)property.Type;
            if ((elementTypeSymbol.ElementType.IsClass() || elementTypeSymbol.ElementType.IsStruct()) && elementTypeSymbol.ElementType is INamedTypeSymbol n)
            {
                elementNamedTypeSymbol = n;
                return true;
            }
        }
        else if (type == FluentTypeKind.IEnumerable && property.Type is INamedTypeSymbol namedTypeSymbol)
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