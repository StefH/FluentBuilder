using FluentBuilderGenerator.Types;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace FluentBuilderGenerator.Extensions;

internal static class ParameterSymbolExtensions
{
    private const string ParameterValueNull = "null";

    public static string GetRefPrefix(this IParameterSymbol ps)
    {
        return ps.RefKind switch
        {
            RefKind.In => "in ",
            RefKind.Out => "out ",
            RefKind.Ref => "ref ",
            _ => string.Empty
        };
    }

    public static string GetParamsPrefix(this IParameterSymbol ps) => ps.IsParams ? "params " : string.Empty;

    public static string GetDefaultValue(this IParameterSymbol ps)
    {
        if (!ps.HasExplicitDefaultValue)
        {
            return string.Empty;
        }

        string defaultValue;
        if (ps.ExplicitDefaultValue == null)
        {
            if (ps.NullableAnnotation == NullableAnnotation.Annotated)
            {
                // The parameter is defined as Nullable, so always use "null".
                defaultValue = ParameterValueNull;
            }
            else
            {
                defaultValue = ps.Type.IsReferenceType
                    ? ParameterValueNull : // The parameter is a ReferenceType, so use "null".
                    $"default({ps.Type})"; // The parameter is not a ReferenceType, so use "default(T)".
            }
        }
        else
        {
            defaultValue = SymbolDisplay.FormatPrimitive(ps.ExplicitDefaultValue, true, false);
        }

        return $" = {defaultValue}";
    }

    //public static TypeEnum GetTypeEnum(this IParameterSymbol p) => p.Type.GetTypeEnum();
}