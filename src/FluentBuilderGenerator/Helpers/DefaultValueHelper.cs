using FluentBuilderGenerator.Extensions;
using FluentBuilderGenerator.Models;
using FluentBuilderGenerator.Types;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FluentBuilderGenerator.Helpers;

internal static class DefaultValueHelper
{
    private static readonly SyntaxKind[] ExcludedSyntaxKinds = { SyntaxKind.SuppressNullableWarningExpression };

    /// <summary>
    /// Check if the <see cref="IPropertySymbol"/> has a value set, in that case try to get that value and return it, and return the usings.
    /// If no value is set, just return the default value.
    /// </summary>
    internal static (string DefaultValue, IReadOnlyList<string>? ExtraUsings) GetDefaultValue(ISymbol property, ITypeSymbol typeSymbol)
    {
        var location = property.Locations.FirstOrDefault();
        if (location != null)
        {
            var rootSyntaxNode = location.SourceTree?.GetRoot();
            if (rootSyntaxNode != null)
            {
                var propertyDeclarationSyntax = rootSyntaxNode.FindDescendantNode<PropertyDeclarationSyntax>(p => p.Identifier.ValueText == property.Name);

                if (propertyDeclarationSyntax?.Initializer != null && !ExcludedSyntaxKinds.Contains(propertyDeclarationSyntax.Initializer.Value.Kind()))
                {
                    var thisUsings = rootSyntaxNode.FindDescendantNodes<UsingDirectiveSyntax>().Select(ud => ud.Name.ToString());

                    var ancestorUsings = rootSyntaxNode.GetAncestorsUsings().Select(ud => ud.Name.ToString());

                    var extraUsings = thisUsings.Union(ancestorUsings).Distinct().ToList();

                    var value = propertyDeclarationSyntax.Initializer.Value.ToString();

                    return (value, extraUsings);
                }
            }
        }

        return (GetDefault(typeSymbol), null);
    }

    private static string GetDefault(ITypeSymbol typeSymbol)
    {
        if (typeSymbol.IsValueType || typeSymbol.NullableAnnotation == NullableAnnotation.Annotated)
        {
            return $"default({typeSymbol})";
        }

        var kind = typeSymbol.GetFluentTypeKind();
        switch (kind)
        {
            case FluentTypeKind.Other:
                return GetNewConstructor(typeSymbol);

            case FluentTypeKind.String:
                return "string.Empty";

            case FluentTypeKind.Array:
                var arrayTypeSymbol = (IArrayTypeSymbol)typeSymbol;
                return $"new {arrayTypeSymbol.ElementType}[0]";

            case FluentTypeKind.IEnumerable:
                // https://stackoverflow.com/questions/41466062/how-to-get-underlying-type-for-ienumerablet-with-roslyn
                var namedTypeSymbol = (INamedTypeSymbol)typeSymbol;
                return $"new {namedTypeSymbol.TypeArguments[0]}[0]";

            case FluentTypeKind.ReadOnlyCollection:
                var readOnlyCollectionSymbol = (INamedTypeSymbol)typeSymbol;
                return $"new {typeSymbol}(new List<{readOnlyCollectionSymbol.TypeArguments[0]}>())";

            case FluentTypeKind.IList:
            case FluentTypeKind.ICollection:
            case FluentTypeKind.IReadOnlyCollection:
                var listSymbol = (INamedTypeSymbol)typeSymbol;
                return $"new List<{listSymbol.TypeArguments[0]}>()";

            case FluentTypeKind.IDictionary:
                var dictionarySymbol = (INamedTypeSymbol)typeSymbol;
                return dictionarySymbol.TypeArguments.Any() ?
                    $"new Dictionary<{dictionarySymbol.TypeArguments[0]}, {dictionarySymbol.TypeArguments[1]}>()" :
                    "new Dictionary<object, object>()";
        }

        return $"default({typeSymbol})";
    }

    private static string GetNewConstructor(ITypeSymbol typeSymbol)
    {
        if (typeSymbol is not INamedTypeSymbol namedTypeSymbol)
        {
            return typeSymbol.NullableAnnotation == NullableAnnotation.Annotated
                ? $"default({typeSymbol})!"
                : $"default({typeSymbol})";
        }

        if (!namedTypeSymbol.Constructors.Any())
        {
            return $"default({typeSymbol})!";
        }

        // Check if it's a Func or Action
        if (namedTypeSymbol.DelegateInvokeMethod != null)
        {
            var delegateParameters = Enumerable.Repeat("_", namedTypeSymbol.DelegateInvokeMethod.Parameters.Length);

            var body = namedTypeSymbol.DelegateInvokeMethod.ReturnsVoid
                ? "{ }" // It's an Action
                : GetDefault(namedTypeSymbol.DelegateInvokeMethod.ReturnType); // It's an Func

            return $"new {typeSymbol}(({string.Join(", ", delegateParameters)}) => {body})";
        }

        var publicConstructorsWithMatch = new List<(IMethodSymbol PublicConstructor, int Match)>();

        foreach (var publicConstructor in namedTypeSymbol.Constructors.OrderBy(c => c.Parameters.Length).ToArray())
        {
            var match = 100 - publicConstructor.Parameters.Length;
            foreach (var parameter in publicConstructor.Parameters)
            {
                if (parameter.Type.OriginalDefinition.ToString() == typeSymbol.OriginalDefinition.ToString())
                {
                    // Prefer a public constructor which does not use itself
                    match -= 10;
                }
            }

            publicConstructorsWithMatch.Add((publicConstructor, match));
        }

        var bestMatchingConstructor = publicConstructorsWithMatch.OrderByDescending(x => x.Match).First().PublicConstructor;

        var constructorParameters = bestMatchingConstructor.Parameters.Select(parameter => GetDefault(parameter.Type));

        return $"new {typeSymbol}({string.Join(", ", constructorParameters)})";
    }
}