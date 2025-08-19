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

internal record PropertyOrParameterSymbol(ISymbol Symbol, ITypeSymbol Type, bool ExcludeFromIsSetLogic) : IPropertyOrParameterSymbol
{
    public PropertyType PropertyType => Symbol is IPropertySymbol ? PropertyType.Property : PropertyType.Parameter;

    public string Name => Symbol.Name;
}