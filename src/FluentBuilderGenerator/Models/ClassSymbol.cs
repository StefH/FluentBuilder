using FluentBuilderGenerator.Types;
using Microsoft.CodeAnalysis;

namespace FluentBuilderGenerator.Models;

internal class ClassSymbol
{
    public FileDataType Type { get; init; }
    public string BuilderNamespace { get; init; } = null!;
    public string BuilderClassName { get; init; } = null!;
    public string FullBuilderClassName { get; init; } = null!;
    public INamedTypeSymbol NamedTypeSymbol { get; init; } = null!;
}