using FluentBuilderGenerator.Types;
using Microsoft.CodeAnalysis;

namespace FluentBuilderGenerator.Interfaces;

internal interface IPropertyOrParameterSymbol
{
    PropertyType PropertyType { get; }

    ISymbol Symbol { get; }

    string Name { get; }

    ITypeSymbol Type { get; }

    bool ExcludeFromIsSetLogic { get; }
}