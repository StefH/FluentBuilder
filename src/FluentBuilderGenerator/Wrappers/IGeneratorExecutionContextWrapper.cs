using System.Diagnostics.CodeAnalysis;
using FluentBuilderGenerator.Models;

namespace FluentBuilderGenerator.Wrappers;

internal interface IGeneratorExecutionContextWrapper
{
    bool TryGetNamedTypeSymbolByFullMetadataName(FluentData fluentDataItem, [NotNullWhen(true)] out ClassSymbol? classSymbol);
}