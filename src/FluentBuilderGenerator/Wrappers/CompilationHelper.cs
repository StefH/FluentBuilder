using System.Diagnostics.CodeAnalysis;
using FluentBuilderGenerator.Models;
using FluentBuilderGenerator.Types;
using Microsoft.CodeAnalysis;

namespace FluentBuilderGenerator.Wrappers;

internal class CompilationHelper(Compilation compilation)
{
    public string AssemblyName { get; } = compilation.AssemblyName ?? "FluentBuilder";

    public bool TryGetNamedTypeSymbolByFullMetadataName(FluentData fluentDataItem, [NotNullWhen(true)] out ClassSymbol? classSymbol)
    {
        classSymbol = null;

        // The GetTypeByMetadataName method returns null if no type matches the full name or if 2 or more types (in different assemblies) match the full name.
        var symbol = compilation.GetTypeByMetadataName(fluentDataItem.MetadataName);
        if (symbol is not null)
        {
            classSymbol = new ClassSymbol
            {
                Type = FileDataType.Builder,
                FluentData = fluentDataItem,
                //BuilderNamespace = fluentDataItem.Namespace,
                //BuilderClassName = fluentDataItem.ShortBuilderClassName,
                //FullBuilderClassName = fluentDataItem.FullBuilderClassName,
                NamedTypeSymbol = symbol
            };
            return true;
        }

        foreach (var @using in fluentDataItem.Usings)
        {
            symbol = compilation.GetTypeByMetadataName($"{@using}.{fluentDataItem.MetadataName}");
            if (symbol is not null)
            {
                classSymbol = new ClassSymbol
                {
                    Type = FileDataType.Builder,
                    FluentData = fluentDataItem,
                    //BuilderNamespace = fluentDataItem.Namespace,
                    //BuilderClassName = fluentDataItem.ShortBuilderClassName,
                    //FullBuilderClassName = fluentDataItem.FullBuilderClassName,
                    NamedTypeSymbol = symbol
                };
                return true;
            }
        }

        return false;
    }
}