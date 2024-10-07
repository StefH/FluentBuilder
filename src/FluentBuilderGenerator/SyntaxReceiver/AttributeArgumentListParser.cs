using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using FluentBuilderGenerator.Types;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FluentBuilderGenerator.SyntaxReceiver;

internal static class AttributeArgumentListParser
{
    private static readonly Regex AutoGenerateBuilderAttributesRegex = new(@"^.*AutoGenerateBuilder(?:<([^>]+)>)?$", RegexOptions.Compiled, TimeSpan.FromSeconds(1));

    public static bool IsMatch(AttributeSyntax attributeSyntax)
    {
        return AutoGenerateBuilderAttributesRegex.IsMatch(attributeSyntax.Name.ToString());
    }

    public static FluentBuilderAttributeArguments ParseAttributeArguments(AttributeSyntax? attributeSyntax)
    {
        var result = new FluentBuilderAttributeArguments();

        if (attributeSyntax == null)
        {
            return result;
        }

        int argumentsParsed = 0;

        var matchesGenericAttribute = AutoGenerateBuilderAttributesRegex.Match(attributeSyntax.Name.ToString());
        var genericType = matchesGenericAttribute.Groups[1].Value;
        var isGenericAttribute = !string.IsNullOrEmpty(genericType);
        if (isGenericAttribute)
        {
            if (attributeSyntax.ArgumentList?.Arguments.Count is > 3)
            {
                throw new ArgumentException("The AutoGenerateBuilderAttribute<T> requires 0, 1, 2, 3 arguments.");
            }

            result = result with { RawTypeName = genericType };
            argumentsParsed++;
        }
        else
        {
            if (attributeSyntax.ArgumentList?.Arguments.Count is > 4)
            {
                throw new ArgumentException("The AutoGenerateBuilderAttribute requires 0, 1, 2, 3 or 4 arguments.");
            }
        }

        if (attributeSyntax.ArgumentList == null)
        {
            return result;
        }

        foreach (var argument in attributeSyntax.ArgumentList.Arguments)
        {
            if (!isGenericAttribute && TryParseAsType(argument.Expression, out var rawTypeValue))
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

        if (!matchesGenericAttribute.Success && attributeSyntax.ArgumentList.Arguments.Count == 1 && argumentsParsed == 0)
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
        value = default;

        if (expressionSyntax is MemberAccessExpressionSyntax memberAccessExpressionSyntax)
        {
            var enumAsStringValue = memberAccessExpressionSyntax.Name.ToString();
            return Enum.TryParse(enumAsStringValue, out value);
        }

        return false;
    }
}