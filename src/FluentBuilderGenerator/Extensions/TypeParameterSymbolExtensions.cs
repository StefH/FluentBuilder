using Microsoft.CodeAnalysis;

namespace FluentBuilderGenerator.Extensions;

internal static class TypeParameterSymbolExtensions
{
    /// <summary>
    /// https://www.codeproject.com/Articles/871704/Roslyn-Code-Analysis-in-Easy-Samples-Part-2
    /// </summary>
    public static string GetWhereStatement(this ITypeParameterSymbol typeParameterSymbol)
    {
        var constraints = new List<string>();
        if (typeParameterSymbol.HasReferenceTypeConstraint)
        {
            constraints.Add("class");
        }

        if (typeParameterSymbol.HasValueTypeConstraint)
        {
            constraints.Add("struct");
        }

        if (typeParameterSymbol.HasConstructorConstraint)
        {
            constraints.Add("new()");
        }

        constraints.AddRange(typeParameterSymbol.ConstraintTypes.OfType<INamedTypeSymbol>().Select(constraintType => constraintType.GetFullType()));

        if (!constraints.Any())
        {
            return string.Empty;
        }

        return $" where {typeParameterSymbol.Name} : {string.Join(", ", constraints)}";
    }
}