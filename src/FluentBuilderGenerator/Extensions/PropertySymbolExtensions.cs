using Microsoft.CodeAnalysis;

namespace FluentBuilderGenerator.Extensions;

internal static class PropertySymbolExtensions
{
    internal static bool TryGetIEnumerableElementType(this IPropertySymbol property, out INamedTypeSymbol? elementClassName)
    {
        elementClassName = null;

        if (property.Type is INamedTypeSymbol nt && nt.IsDictionary())
        {
            return false;
        }

        if (property.Type.IsIEnumerable() && property.Type is INamedTypeSymbol namedTypeSymbol)
        {
            if (namedTypeSymbol.IsGenericType)
            {
                if (namedTypeSymbol.TypeArguments.FirstOrDefault() is INamedTypeSymbol genericNamedTypeSymbol)
                {
                    elementClassName = genericNamedTypeSymbol;
                    return true;
                }
            }

            elementClassName = namedTypeSymbol;
            return true;
        }

        if (property.Type.TypeKind == TypeKind.Array)
        {
            var elementTypeSymbol = (IArrayTypeSymbol)property.Type;

            if ((elementTypeSymbol.ElementType.IsClass() || elementTypeSymbol.ElementType.IsStruct()) && elementTypeSymbol.ElementType is INamedTypeSymbol n)
            {
                elementClassName = n;
                return true;
            }
        }

        return false;
    }
}