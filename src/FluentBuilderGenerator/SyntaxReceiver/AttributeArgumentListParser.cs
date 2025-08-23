using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using FluentBuilderGenerator.Extensions;
using FluentBuilderGenerator.Types;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FluentBuilderGenerator.SyntaxReceiver;

internal static class AttributeArgumentListParser
{
    private static readonly Regex AutoGenerateBuilderAttributesRegex = new(@"^FluentBuilder\.AutoGenerateBuilder|AutoGenerateBuilder(?:<([^>]+)>)?$", RegexOptions.Compiled, TimeSpan.FromSeconds(1));

    public static bool IsMatch(AttributeSyntax attributeSyntax)
    {
        return AutoGenerateBuilderAttributesRegex.IsMatch(attributeSyntax.Name.ToString());
    }

    public static FluentBuilderAttributeArguments Parse(AttributeSyntax? attributeSyntax, SemanticModel semanticModel)
    {
        var result = new FluentBuilderAttributeArguments();

        if (attributeSyntax == null)
        {
            return result;
        }

        var argumentsParsed = 0;
        var skip = 0;
        var isGeneric = false;

        if (TryParseAsType(attributeSyntax.Name, semanticModel, out var infoGeneric))
        {
            result = result with { RawTypeName = infoGeneric.Value.MetadataName };
            isGeneric = true;
            argumentsParsed++;
        }
        else if (attributeSyntax.ArgumentList != null && TryParseAsType(attributeSyntax.ArgumentList.Arguments[0].Expression, semanticModel, out var info))
        {
            result = result with { RawTypeName = info.Value.MetadataName };
            skip = 1;
            argumentsParsed++;
        }

        var array = attributeSyntax.ArgumentList?.Arguments.ToArray() ?? [];

        foreach (var argument in array.Skip(skip))
        {
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

        if (!isGeneric && array.Length == 1 && argumentsParsed == 0)
        {
            throw new ArgumentException($"When the AutoGenerateBuilderAttribute is used with 1 argument, the only argument should be a Type, bool, {nameof(FluentBuilderAccessibility)} or {nameof(FluentBuilderMethods)}.");
        }

        return result;
    }

    private static bool TryParseAsBoolean(ExpressionSyntax expressionSyntax, out bool value)
    {
        if (expressionSyntax is LiteralExpressionSyntax literalExpressionSyntax)
        {
            value = literalExpressionSyntax.Kind() == SyntaxKind.TrueLiteralExpression;
            return true;
        }

        value = false;
        return false;
    }

    private static bool TryParseAsType(
        CSharpSyntaxNode? syntaxNode,
        SemanticModel semanticModel,
        [NotNullWhen(true)] out (string MetadataName, bool IsGeneric)? info
    )
    {
        info = null;

        bool isGeneric;
        TypeSyntax typeSyntax;
        switch (syntaxNode)
        {
            case TypeOfExpressionSyntax typeOfExpressionSyntax:
                typeSyntax = typeOfExpressionSyntax.Type;
                isGeneric = false;
                break;

            case GenericNameSyntax genericRightNameSyntax:
                typeSyntax = genericRightNameSyntax.TypeArgumentList.Arguments.First();
                isGeneric = true;
                break;

            default:
                return false;
        }

        var typeInfo = semanticModel.GetTypeInfo(typeSyntax);
        var typeSymbol = typeInfo.Type!;

        info = new(typeSymbol.GetFullMetadataName(), isGeneric);

        return true;
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