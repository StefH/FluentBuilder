using Microsoft.CodeAnalysis;

namespace FluentBuilderGenerator.Helpers;

internal static class TypeHelper
{
    public static bool IsRunTimeType(this ITypeSymbol typeSymbol)
    {
        return typeSymbol.ContainingAssembly.Name == nameof(System.Runtime);
    }

    public static string GetFixedType(this ITypeSymbol typeSymbol)
    {
        return typeSymbol.IsRunTimeType() ? typeSymbol.ToString() : $"global::{typeSymbol}";
    }
}