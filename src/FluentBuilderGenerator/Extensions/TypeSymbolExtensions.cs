using Microsoft.CodeAnalysis;

namespace FluentBuilderGenerator.Extensions
{
    internal static class TypeSymbolExtensions
    {
        public static bool IsValueType(this ITypeSymbol ts)
        {
            return ts.IsValueType || ts.IsString();
        }

        public static bool IsString(this ITypeSymbol ts)
        {
            return ts.ToString() == "string" || ts.ToString() == "string?";
        }
    }
}