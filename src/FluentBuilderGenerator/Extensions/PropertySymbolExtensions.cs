using FluentBuilderGenerator.Types;
using FluentBuilderGenerator.Wrappers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FluentBuilderGenerator.Extensions;

internal static class PropertySymbolExtensions
{
    // ReSharper disable once InconsistentNaming
    private static readonly FluentTypeKind[] IEnumerableKinds =
    {
        FluentTypeKind.IEnumerable,
        FluentTypeKind.IList,
        FluentTypeKind.ICollection
    };

    internal static bool IsSettable(this IPropertySymbol property)
    {
        return property.SetMethod is { IsInitOnly: false };
    }

    /// <summary>
    /// Check if the <see cref="IPropertySymbol"/> has a value set, in that case try to get that value and return the using if needed.
    ///
    /// If no value is set, just return the default.
    /// </summary>
    internal static (string DefaultValue, IReadOnlyList<string>? ExtraUsings) GetDefault(this IPropertySymbol property, IGeneratorExecutionContextWrapper context)
    {
        var location = property.Locations.FirstOrDefault();
        if (location != null)
        {
            var rootSyntaxNode = location.SourceTree?.GetRoot();
            if (rootSyntaxNode != null)
            {
                var propertyDeclarationSyntax = rootSyntaxNode.DescendantNodes()
                    .OfType<PropertyDeclarationSyntax>()
                    .FirstOrDefault(p => p.Identifier.ValueText == property.Name && p.Type.ToString() == property.Type.Name);

                if (propertyDeclarationSyntax is { Initializer: { } })
                {
                    var thisUsings = rootSyntaxNode.DescendantNodes().OfType<UsingDirectiveSyntax>().Select(ud => ud.Name.ToString());

                    var ancestorUsings = rootSyntaxNode.GetAncestorsUsings().Select(ud => ud.Name.ToString());

                    var extraUsings = thisUsings.Union(ancestorUsings).Distinct().ToList();

                    var value = propertyDeclarationSyntax.Initializer.Value.ToString();

                    return (value, extraUsings);

                    //return context.TryGetUsing(propertyDeclarationSyntax.Type.ToString(), extraUsings, out var extraUsing) ?
                    //    (value, extraUsings) :
                    //    (value, null);
                }
            }
        }

        return (property.Type.GetDefault(), null);
    }

    internal static bool TryGetIDictionaryElementTypes(this IPropertySymbol property, out (INamedTypeSymbol key, INamedTypeSymbol value)? tuple)
    {
        var type = property.Type.GetFluentTypeKind();

        if (type == FluentTypeKind.IDictionary && property.Type is INamedTypeSymbol namedTypeSymbol)
        {
            if (namedTypeSymbol.IsGenericType && namedTypeSymbol.TypeArguments.Length == 2)
            {
                if (namedTypeSymbol.TypeArguments[0] is INamedTypeSymbol key && namedTypeSymbol.TypeArguments[1] is INamedTypeSymbol value)
                {
                    tuple = new(key, value);
                    return true;
                }
            }
        }

        tuple = default;
        return false;
    }

    internal static bool TryGetIEnumerableElementType(
        this IPropertySymbol property,
        out INamedTypeSymbol? elementNamedTypeSymbol,
        out FluentTypeKind kind)
    {
        elementNamedTypeSymbol = null;
        kind = property.Type.GetFluentTypeKind();

        if (kind == FluentTypeKind.Array)
        {
            var elementTypeSymbol = (IArrayTypeSymbol)property.Type;
            if ((elementTypeSymbol.ElementType.IsClass() || elementTypeSymbol.ElementType.IsStruct()) && elementTypeSymbol.ElementType is INamedTypeSymbol arrayNamedTypedSymbol)
            {
                elementNamedTypeSymbol = arrayNamedTypedSymbol;
                return true;
            }
        }
        else if (IEnumerableKinds.Contains(kind) && property.Type is INamedTypeSymbol namedTypeSymbol)
        {
            if (namedTypeSymbol.IsGenericType)
            {
                if (namedTypeSymbol.TypeArguments.FirstOrDefault() is INamedTypeSymbol genericNamedTypeSymbol)
                {
                    elementNamedTypeSymbol = genericNamedTypeSymbol;
                    return true;
                }
            }

            elementNamedTypeSymbol = namedTypeSymbol;
            return true;
        }

        return false;
    }
}