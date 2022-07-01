using System.Diagnostics.CodeAnalysis;
using FluentBuilderGenerator.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace FluentBuilderGenerator.Wrappers;

internal interface IGeneratorExecutionContextWrapper
{
    /// <see cref="Compilation.AssemblyName"/>
    public string AssemblyName { get; }

    public bool SupportsNullable { get; }

    public bool NullableEnabled { get; }

    /// <see cref="GeneratorExecutionContext.AddSource(string, SourceText)"/>
    public void AddSource(string hintName, SourceText sourceText);

    bool TryGetUsing(string shortName, IReadOnlyList<string> usings, [NotNullWhen(true)] out string? result);

    bool TryGetNamedTypeSymbolByFullMetadataName(FluentData fluentDataItem, [NotNullWhen(true)] out ClassSymbol? classSymbol);
}