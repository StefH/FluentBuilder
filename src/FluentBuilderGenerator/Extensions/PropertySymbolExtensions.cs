using Microsoft.CodeAnalysis;

namespace FluentBuilderGenerator.Extensions
{
    internal static class PropertySymbolExtensions
    {
        public static bool IsValueType(this IPropertySymbol p) =>
            p.Type.IsValueType();

    }
}