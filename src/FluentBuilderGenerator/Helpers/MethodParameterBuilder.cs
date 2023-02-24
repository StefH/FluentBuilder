using System.Text;
using FluentBuilderGenerator.Extensions;
using Microsoft.CodeAnalysis;

namespace FluentBuilderGenerator.Helpers;

internal static class MethodParameterBuilder
{
    public static string Build(IParameterSymbol parameterSymbol, string? type)
    {
        var stringBuilder = new StringBuilder();
        if (type is not null)
        {
            stringBuilder.Append(parameterSymbol.GetAttributesPrefix()); // "" or [NotNullWhen(true)]
            stringBuilder.Append(parameterSymbol.GetParamsPrefix()); // "" or "params "
            stringBuilder.Append(parameterSymbol.GetRefPrefix()); // "" or "out "
            stringBuilder.AppendFormat("{0} ", type); // string or another type
        }
        
        stringBuilder.Append(parameterSymbol.GetSanitizedName()); // "s" or "i" or ...
        stringBuilder.Append(parameterSymbol.GetDefaultValue()); // "" or the value

        return stringBuilder.ToString();
    }
}