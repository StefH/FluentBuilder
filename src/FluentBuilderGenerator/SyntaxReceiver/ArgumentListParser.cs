using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FluentBuilderGenerator.SyntaxReceiver;

internal static class ArgumentListParser
{
    public static (string? RawTypeName, bool HandleBaseClasses) ParseAttributeArgumentList(AttributeArgumentListSyntax? argumentList)
    {
        if (argumentList == null || !argumentList.Arguments.Any() || argumentList.Arguments.Count == 0)
        {
            return (null, false);
        }

        if (argumentList.Arguments.Count == 1)
        {
            if (TryParseAsBoolean(argumentList.Arguments[0].Expression, out var booleanValue))
            {
                return (null, booleanValue);
            }

            if (TryParseAsType(argumentList.Arguments[0].Expression, out string? rawTypeValue))
            {
                return (rawTypeValue, false);
            }

            throw new ArgumentException("When the AutoGenerateBuilderAttribute is used with 1 argument, the only argument should be a Type or a boolean.");
        }

        if (argumentList.Arguments.Count == 2)
        {
            if (!TryParseAsType(argumentList.Arguments[0].Expression, out string? rawTypeValue))
            {
                throw new ArgumentException("When the AutoGenerateBuilderAttribute is used with 2 arguments, the first argument should be a Type.");
            }

            if (!TryParseAsBoolean(argumentList.Arguments[1].Expression, out bool booleanValue))
            {
                throw new ArgumentException("When the AutoGenerateBuilderAttribute is used with 2 arguments, the second argument should be a boolean.");
            }

            return (rawTypeValue, booleanValue);
        }

        throw new ArgumentException("The AutoGenerateBuilderAttribute requires 0, 1, or 2 arguments.");
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
}