using System.Diagnostics.CodeAnalysis;
using FluentBuilderGenerator.Types;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FluentBuilderGenerator.SyntaxReceiver;

internal static class AttributeArgumentListParser
{
    public static FluentBuilderAttributeArguments ParseAttributeArguments(AttributeArgumentListSyntax? argumentList)
    {
        var result = new FluentBuilderAttributeArguments();

        if (argumentList == null)
        {
            return result;
        }

        if (argumentList.Arguments.Count is < 0 or > 4)
        {
            throw new ArgumentException("The AutoGenerateBuilderAttribute requires 0, 1, 2, 3 or 4 arguments.");
        }

        //if (argumentList.Arguments.Count == 1)
        //{
        //    if (TryParseAsBoolean(argumentList.Arguments[0].Expression, out var handleBaseClasses))
        //    {
        //        return result with { HandleBaseClasses = handleBaseClasses };
        //    }

        //    if (TryParseAsType(argumentList.Arguments[0].Expression, out var rawTypeValue))
        //    {
        //        return result with { RawTypeName = rawTypeValue };
        //    }

        //    if (TryParseAsEnum<FluentBuilderAccessibility>(argumentList.Arguments[0].Expression, out var accessibility))
        //    {
        //        return result with { Accessibility = accessibility };
        //    }

        //    if (TryParseAsEnum<FluentBuilderMethods>(argumentList.Arguments[0].Expression, out var methods))
        //    {
        //        return result with { Methods = methods };
        //    }

        //    throw new ArgumentException($"When the AutoGenerateBuilderAttribute is used with 1 argument, the only argument should be a Type, bool, {nameof(FluentBuilderAccessibility)} or {nameof(FluentBuilderMethods)}.");
        //}

        int argumentsParsed = 0;
        foreach (var argument in argumentList.Arguments)
        {
            if (TryParseAsType(argument.Expression, out var rawTypeValue))
            {
                result = result with { RawTypeName = rawTypeValue };
                argumentsParsed++;
            }

            if (TryParseAsBoolean(argument.Expression, out var handleBaseClasses))
            {
                result = result with { HandleBaseClasses = handleBaseClasses };
                argumentsParsed++;
            }

            if (TryParseAsEnum<FluentBuilderAccessibility>(argument.Expression, out var accessibility))
            {
                result = result with { Accessibility = accessibility };
                argumentsParsed++;
            }

            if (TryParseAsEnum<FluentBuilderMethods>(argument.Expression, out var methods))
            {
                result = result with { Methods = methods };
                argumentsParsed++;
            }
        }

        if (argumentList.Arguments.Count == 1 & argumentsParsed == 0)
        {
            throw new ArgumentException($"When the AutoGenerateBuilderAttribute is used with 1 argument, the only argument should be a Type, bool, {nameof(FluentBuilderAccessibility)} or {nameof(FluentBuilderMethods)}.");
        }

        return result;
    }

    private static bool TryParseAsBoolean(ExpressionSyntax expressionSyntax, out bool value)
    {
        value = default;

        if (expressionSyntax is LiteralExpressionSyntax literalExpressionSyntax)
        {
            value = literalExpressionSyntax.Kind() == SyntaxKind.TrueLiteralExpression;
            return true;
        }

        return false;
    }

    private static bool TryParseAsType(ExpressionSyntax expressionSyntax, [NotNullWhen(true)] out string? rawTypeName)
    {
        rawTypeName = null;

        if (expressionSyntax is TypeOfExpressionSyntax typeOfExpressionSyntax)
        {
            rawTypeName = typeOfExpressionSyntax.Type.ToString();
            return true;
        }

        return false;
    }

    private static bool TryParseAsEnum<TEnum>(ExpressionSyntax expressionSyntax, out TEnum value)
        where TEnum : struct
    {
        return Enum.TryParse(expressionSyntax.ToString().Substring(typeof(TEnum).Name.Length + 1), out value);
    }
}