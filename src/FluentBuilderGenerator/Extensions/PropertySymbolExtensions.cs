using Microsoft.CodeAnalysis;

namespace FluentBuilderGenerator.Extensions;

/// <summary>
/// Some extensions copied from:
/// - https://github.com/explorer14/SourceGenerators
/// - https://github.com/icsharpcode/RefactoringEssentials
/// </summary>
internal static class PropertySymbolExtensions
{
    internal static bool TryGetIEnumerableElementType(this IPropertySymbol property, out INamedTypeSymbol? elementClassName)
    {
        if (property.Type.IsEnumerable() && property.Type is INamedTypeSymbol namedTypeSymbol)
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

        elementClassName = null;
        return false;
    }

    public static bool IsEnumerable(this ITypeSymbol typeSymbol)
    {
        return typeSymbol.SpecialType != SpecialType.System_String &&
               typeSymbol.AllInterfaces.Any(i => i.SpecialType == SpecialType.System_Collections_IEnumerable);
    }

    public static bool CanSupportCollectionInitializer(this ITypeSymbol typeSymbol)
    {
        if (typeSymbol.AllInterfaces.Any(i => i.SpecialType == SpecialType.System_Collections_IEnumerable))
        {
            var curType = typeSymbol;
            while (curType != null)
            {
                if (HasAddMethod(curType))
                {
                    return true;
                }

                curType = curType.BaseType;
            }
        }

        return false;
    }

    private static bool HasAddMethod(ITypeSymbol typeSymbol)
    {
        return typeSymbol
            .GetMembers(WellKnownMemberNames.CollectionInitializerAddMethodName)
            .OfType<IMethodSymbol>().Any(m => m.Parameters.Any());
    }

    internal static bool IsClass(this ITypeSymbol namedType) =>
        namedType.IsReferenceType && namedType.TypeKind == TypeKind.Class;

    internal static bool IsStruct(this ITypeSymbol namedType) =>
        namedType.IsValueType && namedType.TypeKind == TypeKind.Struct;
}