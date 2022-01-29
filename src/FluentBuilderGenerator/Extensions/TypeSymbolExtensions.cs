using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;

namespace FluentBuilderGenerator.Extensions
{
    internal static class TypeSymbolExtensions
    {
        //public static string GenerateClassName(this ITypeSymbol typeSymbol)
        //{
        //    var className = $"{namedTypeSymbol.Name}{(addBuilderPostFix ? "Builder" : string.Empty)}";
        //    return !namedTypeSymbol.IsGenericType ?
        //        $"{typeSymbol.Name}" :
        //        $"{typeSymbol.Name}<{string.Join(", ", typeSymbol.TypeArguments.Select(ta => ta.Name))}>";
        //}
    }
}
