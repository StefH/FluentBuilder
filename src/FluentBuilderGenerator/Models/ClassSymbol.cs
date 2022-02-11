using FluentBuilderGenerator.Types;
using Microsoft.CodeAnalysis;

namespace FluentBuilderGenerator.Models;

internal record ClassSymbol
(
    FileDataType Type,
    string BuilderNamespace,
    string BuilderClassName,
    string FullBuilderClassName,
    INamedTypeSymbol NamedTypeSymbol
);