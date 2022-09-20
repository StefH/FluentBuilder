using System.Diagnostics.CodeAnalysis;
using FluentBuilderGenerator.Models;
using FluentBuilderGenerator.Types;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace FluentBuilderGenerator.Wrappers;

internal class GeneratorExecutionContextWrapper : IGeneratorExecutionContextWrapper
{
    private readonly GeneratorExecutionContext _context;

    public GeneratorExecutionContextWrapper(GeneratorExecutionContext context)
    {
        _context = context;

        if (context.ParseOptions is not CSharpParseOptions csharpParseOptions)
        {
            throw new NotSupportedException("Only C# is supported.");
        }

        // https://github.com/reactiveui/refit/blob/main/InterfaceStubGenerator.Core/InterfaceStubGenerator.cs
        SupportsNullable = csharpParseOptions.LanguageVersion >= LanguageVersion.CSharp8;
        NullableEnabled = context.Compilation.Options.NullableContextOptions == NullableContextOptions.Enable;
    }

    public string AssemblyName => _context.Compilation.AssemblyName ?? "FluentBuilder";

    public bool SupportsNullable { get; }

    public bool NullableEnabled { get; }

    public void AddSource(string hintName, SourceText sourceText) => _context.AddSource(hintName, sourceText);

    public bool TryGetNamedTypeSymbolByFullMetadataName(FluentData fluentDataItem, [NotNullWhen(true)] out ClassSymbol? classSymbol)
    {
        classSymbol = null;

        // The GetTypeByMetadataName method returns null if no type matches the full name or if 2 or more types (in different assemblies) match the full name.
        var symbol = _context.Compilation.GetTypeByMetadataName(fluentDataItem.MetadataName);
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
            symbol = _context.Compilation.GetTypeByMetadataName($"{@using}.{fluentDataItem.MetadataName}");
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