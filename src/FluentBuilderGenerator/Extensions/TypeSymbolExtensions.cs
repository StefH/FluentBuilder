using System.Collections;
using System.Collections.ObjectModel;
using System.Text;
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
    internal static FluentTypeKind GetFluentTypeKind(this ITypeSymbol typeSymbol)
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

        if (typeSymbol.ImplementsInterfaceOrBaseClass(typeof(ReadOnlyCollection<>)))
        {
            return FluentTypeKind.ReadOnlyCollection;
        }

        if (typeSymbol.ImplementsInterfaceOrBaseClass(typeof(IList<>)) || typeSymbol.ImplementsInterfaceOrBaseClass(typeof(IList)))
        {
            return FluentTypeKind.IList;
        }

        if (typeSymbol.ImplementsInterfaceOrBaseClass(typeof(IReadOnlyList<>)))
        {
            return FluentTypeKind.IReadOnlyList;
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
    internal static bool ImplementsInterfaceOrBaseClass(this ITypeSymbol typeSymbol, Type typeToCheck)
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

    internal static bool CanSupportCollectionInitializer(this ITypeSymbol typeSymbol)
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

    internal static bool IsClass(this ITypeSymbol namedType) =>
        namedType is { IsReferenceType: true, TypeKind: TypeKind.Class };

    internal static bool IsStruct(this ITypeSymbol namedType) =>
        namedType is { IsValueType: true, TypeKind: TypeKind.Struct };

    internal static string ToFullyQualifiedDisplayString(this ITypeSymbol property) =>
        property.ToDisplayString(NullableFlowState.None, SymbolDisplayFormat.FullyQualifiedFormat);

    //https://stackoverflow.com/questions/27105909/get-fully-qualified-metadata-name-in-roslyn
    public static string GetFullMetadataName(this ISymbol? s)
    {
        if (s == null || IsRootNamespace(s))
        {
            return string.Empty;
        }

        var sb = new StringBuilder(s.MetadataName);
        var last = s;

        s = s.ContainingSymbol;

        while (!IsRootNamespace(s))
        {
            if (s is ITypeSymbol && last is ITypeSymbol)
            {
                sb.Insert(0, '+');
            }
            else
            {
                sb.Insert(0, '.');
            }

            sb.Insert(0, s.OriginalDefinition.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat));
            s = s.ContainingSymbol;
        }

        return sb.ToString();
    }

    private static bool IsRootNamespace(ISymbol symbol) =>
        symbol is INamespaceSymbol { IsGlobalNamespace: true };

    private static bool HasAddMethod(INamespaceOrTypeSymbol typeSymbol)
    {
        return typeSymbol
            .GetMembers(WellKnownMemberNames.CollectionInitializerAddMethodName)
            .OfType<IMethodSymbol>().Any(m => m.Parameters.Any());
    }
}