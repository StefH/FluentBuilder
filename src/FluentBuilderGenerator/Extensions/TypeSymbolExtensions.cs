using System.Collections;
using FluentBuilderGenerator.Types;
using Microsoft.CodeAnalysis;

namespace FluentBuilderGenerator.Extensions;

/// <summary>
/// Some extensions copied from:
/// - https://github.com/explorer14/SourceGenerators
/// - https://github.com/icsharpcode/RefactoringEssentials
/// </summary>
internal static class TypeSymbolExtensions
{
    public static FluentTypeKind GetFluentTypeKind(this ITypeSymbol typeSymbol)
    {
        if (typeSymbol.SpecialType == SpecialType.System_String)
        {
            return FluentTypeKind.String;
        }

        if (typeSymbol.TypeKind == TypeKind.Array)
        {
            return FluentTypeKind.Array;
        }

        if (typeSymbol.ImplementsInterfaceOrBaseClass(typeof(IDictionary<,>)) || typeSymbol.ImplementsInterfaceOrBaseClass(typeof(IDictionary)))
        {
            return FluentTypeKind.IDictionary;
        }

        if (typeSymbol.ImplementsInterfaceOrBaseClass(typeof(IList<>)) || typeSymbol.ImplementsInterfaceOrBaseClass(typeof(IList)))
        {
            return FluentTypeKind.IList;
        }

        if (typeSymbol.ImplementsInterfaceOrBaseClass(typeof(IReadOnlyCollection<>)))
        {
            return FluentTypeKind.IReadOnlyCollection;
        }

        if (typeSymbol.ImplementsInterfaceOrBaseClass(typeof(ICollection<>)) || typeSymbol.ImplementsInterfaceOrBaseClass(typeof(ICollection)))
        {
            return FluentTypeKind.ICollection;
        }

        if (typeSymbol.AllInterfaces.Any(i => i.SpecialType == SpecialType.System_Collections_IEnumerable))
        {
            return FluentTypeKind.IEnumerable;
        }

        return FluentTypeKind.Other;
    }

    // https://stackoverflow.com/questions/39708316/roslyn-is-a-inamedtypesymbol-of-a-class-or-subclass-of-a-given-type
    public static bool ImplementsInterfaceOrBaseClass(this ITypeSymbol typeSymbol, Type typeToCheck)
    {
        if (typeSymbol.MetadataName == typeToCheck.Name)
        {
            return true;
        }

        if (typeSymbol.BaseType?.MetadataName == typeToCheck.Name)
        {
            return true;
        }

        foreach (var @interface in typeSymbol.AllInterfaces)
        {
            if (@interface.MetadataName == typeToCheck.Name)
            {
                return true;
            }
        }

        return false;
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

    internal static string GetDefault(this ITypeSymbol typeSymbol)
    {
        if (typeSymbol.IsValueType || typeSymbol.NullableAnnotation == NullableAnnotation.Annotated)
        {
            return $"default({typeSymbol})";
        }

        var kind = typeSymbol.GetFluentTypeKind();
        switch (kind)
        {
            case FluentTypeKind.Other:
                return GetNewConstructor(typeSymbol);

            case FluentTypeKind.String:
                return "string.Empty";

            case FluentTypeKind.Array:
                var arrayTypeSymbol = (IArrayTypeSymbol)typeSymbol;
                return $"new {arrayTypeSymbol.ElementType}[0]";

            case FluentTypeKind.IEnumerable:
                // https://stackoverflow.com/questions/41466062/how-to-get-underlying-type-for-ienumerablet-with-roslyn
                var namedTypeSymbol = (INamedTypeSymbol)typeSymbol;
                return $"new {namedTypeSymbol.TypeArguments[0]}[0]";

            case FluentTypeKind.IList:
            case FluentTypeKind.ICollection:
                var listSymbol = (INamedTypeSymbol)typeSymbol;
                return $"new List<{listSymbol.TypeArguments[0]}>()";

            case FluentTypeKind.IDictionary:
                var dictionarySymbol = (INamedTypeSymbol)typeSymbol;
                return dictionarySymbol.TypeArguments.Any() ?
                    $"new Dictionary<{dictionarySymbol.TypeArguments[0]}, {dictionarySymbol.TypeArguments[1]}>()" :
                    "new Dictionary<object, object>()";
        }

        return $"default({typeSymbol})";
    }

    private static string GetNewConstructor(ITypeSymbol typeSymbol)
    {
        if (typeSymbol is INamedTypeSymbol namedTypeSymbol)
        {
            if (!namedTypeSymbol.Constructors.Any())
            {
                return $"default({typeSymbol})!";
            }

            var publicConstructor = namedTypeSymbol.Constructors.OrderBy(c => c.Parameters.Length).First();

            var list = publicConstructor.Parameters.Select(p => p.Type.GetDefault());

            return $"new {typeSymbol}({string.Join(", ", list)})";
        }

        return typeSymbol.NullableAnnotation == NullableAnnotation.Annotated ?
            $"default({typeSymbol})!" :
            $"default({typeSymbol})";
    }
}