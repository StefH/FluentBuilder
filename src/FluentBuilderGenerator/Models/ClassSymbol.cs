using FluentBuilderGenerator.Types;
using Microsoft.CodeAnalysis;

namespace FluentBuilderGenerator.Models;

internal class ClassSymbol
{
    public FileDataType Type { get; init; }

    public FluentData FluentData { get; init; }

    public string BuilderNamespace => FluentData.Namespace;

    public string BuilderClassName => FluentData.ShortBuilderClassName;

    public string FullBuilderClassName => FluentData.FullBuilderClassName;

    public INamedTypeSymbol NamedTypeSymbol { get; init; } = null!;

    public string ItemBuilderFullName { get; init; } = string.Empty;
}