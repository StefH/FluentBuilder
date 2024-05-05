using FluentBuilderGenerator.Constants;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace FluentBuilderGenerator.Extensions;

internal static class SymbolExtensions
{
    private const string GlobalPrefix = "global::";
    
    private static readonly string[] ExcludedAttributes =
    {
        InternalClassNames.AsyncStateMachineAttribute ,
        InternalClassNames.NullableAttribute
    };

    public static string GetDeterministicHashCodeAsString(this ISymbol symbol)
    {
        return symbol.ToString().GetDeterministicHashCodeAsString();
    }

    public static string GetAttributesPrefix(this ISymbol symbol)
    {
        var attributes = symbol.GetAttributesAsList();

        return attributes.Any().IIf($"{string.Join(" ", attributes)} ");
    }

    public static IReadOnlyList<string> GetAttributesAsList(this ISymbol symbol)
    {
        return symbol
            .GetAttributes()
            .Where(a => a.AttributeClass.IsPublic() && !ExcludedAttributes.Contains(a.AttributeClass?.ToString(), StringComparer.OrdinalIgnoreCase))
            .Select(a => $"[{a}]")
            .ToArray();
    }

    public static bool IsPublic(this ISymbol? symbol) =>
        symbol is { DeclaredAccessibility: Accessibility.Public };

    public static bool IsKeywordOrReserved(this ISymbol symbol) =>
        SyntaxFacts.GetKeywordKind(symbol.Name) != SyntaxKind.None || SyntaxFacts.GetContextualKeywordKind(symbol.Name) != SyntaxKind.None;

    public static string GetSanitizedName(this ISymbol symbol) =>
        symbol.IsKeywordOrReserved() ? $"@{symbol.Name}" : symbol.Name;

    public static string GetGlobalPrefix(this ISymbol symbol)
    {
        if (symbol is IPropertySymbol { Type.SpecialType: SpecialType.None })
        {
            return GlobalPrefix;
        }

        if (symbol is ITypeSymbol { SpecialType: SpecialType.None })
        {
            return GlobalPrefix;
        }

        return string.Empty;
    }
}