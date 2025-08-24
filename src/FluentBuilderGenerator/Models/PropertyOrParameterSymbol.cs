using FluentBuilderGenerator.Interfaces;
using FluentBuilderGenerator.Types;
using Microsoft.CodeAnalysis;

namespace FluentBuilderGenerator.Models;

internal record PropertyOrParameterSymbol(ISymbol Symbol, ITypeSymbol Type, bool ExcludeFromIsSetLogic) : IPropertyOrParameterSymbol
{
    public PropertyType PropertyType => Symbol is IPropertySymbol ? PropertyType.Property : PropertyType.Parameter;

    public string Name => Symbol.Name;
}