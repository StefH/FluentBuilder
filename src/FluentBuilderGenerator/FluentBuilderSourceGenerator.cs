// This source code is based on https://justsimplycode.com/2020/12/06/auto-generate-builders-using-source-generator-in-net-5

using FluentBuilderGenerator.FileGenerators;
using FluentBuilderGenerator.Models;
using FluentBuilderGenerator.SyntaxReceiver;
using FluentBuilderGenerator.Types;
using FluentBuilderGenerator.Wrappers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace FluentBuilderGenerator;

[Generator(LanguageNames.CSharp)]
internal class FluentBuilderSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
#if DEBUGATTACH
        if (!System.Diagnostics.Debugger.IsAttached)
        {
            System.Diagnostics.Debugger.Launch();
        }
#endif

        var languageDataProvider = context.ParseOptionsProvider.Select(static (options, _) =>
        {
            if (options is not CSharpParseOptions csParseOptions)
            {
                throw new NotSupportedException($"Only {LanguageNames.CSharp} is supported.");
            }

            return new LanguageData
            {
                SupportsNullable = csParseOptions.LanguageVersion >= LanguageVersion.CSharp8,
                SupportsGenericAttributes = csParseOptions.LanguageVersion >= LanguageVersion.CSharp11
            };
        });

        var compilationHelperProvider = context.CompilationProvider.Select(static (compilation, _) => new CompilationHelper(compilation));

        var commonProvider = languageDataProvider.Combine(compilationHelperProvider);

        context.RegisterSourceOutput(commonProvider, static (SourceProductionContext spc, (LanguageData LanguageData, CompilationHelper CompilationHelper) x) =>
        {
            var assemblyName = x.CompilationHelper.AssemblyName;
            var supportsNullable = x.LanguageData.SupportsNullable;

            var generators = new IFileGenerator[]
            {
                new BaseBuilderGenerator(assemblyName, x.LanguageData.SupportsNullable),

                new ExtraFilesGenerator(supportsNullable, x.LanguageData.SupportsGenericAttributes),

                new IDictionaryBuilderGenerator(assemblyName, supportsNullable),

                new IEnumerableBuilderGenerator(assemblyName, FileDataType.ArrayBuilder, supportsNullable),
                new IEnumerableBuilderGenerator(assemblyName, FileDataType.ICollectionBuilder, supportsNullable),
                new IEnumerableBuilderGenerator(assemblyName, FileDataType.IEnumerableBuilder, supportsNullable),
                new IEnumerableBuilderGenerator(assemblyName, FileDataType.IListBuilder, supportsNullable),
                new IEnumerableBuilderGenerator(assemblyName, FileDataType.IReadOnlyCollectionBuilder, supportsNullable),
                new IEnumerableBuilderGenerator(assemblyName, FileDataType.IReadOnlyListBuilder, supportsNullable),
            };

            foreach (var generator in generators)
            {
                var fileData = generator.GenerateFile();
                spc.AddSource(fileData.FileName, SourceText.From(fileData.Text, Encoding.UTF8));
            }
        });

        var fluentBuilderClassesProvider = context.SyntaxProvider
            .CreateSyntaxProvider(ShouldHandle, Transform)
            .Collect();

        var combined2 = fluentBuilderClassesProvider.Combine(commonProvider).Select((x, _) => new
        {
            Items = x.Left,
            LanguageData = x.Right.Left,
            CompilationHelper = x.Right.Right
        });

        context.RegisterSourceOutput(combined2, static (spc, data) =>
        {
            var generator = new FluentBuilderClassesGenerator(data.Items, data.CompilationHelper, data.LanguageData.SupportsNullable);

            try
            {
                foreach (var fileData in generator.GenerateFiles())
                {
                    spc.AddSource(fileData.FileName, SourceText.From(fileData.Text, Encoding.UTF8));
                }
            }
            catch (Exception exception)
            {
                var message = $"/*\r\n{nameof(FluentBuilderSourceGenerator)}\r\n\r\n[Exception]\r\n{exception}\r\n\r\n[StackTrace]\r\n{exception.StackTrace}*/";
                spc.AddSource("Error.g.cs", SourceText.From(message, Encoding.UTF8));
            }
        });
    }

    private static bool ShouldHandle(SyntaxNode syntaxNode, CancellationToken cancellationToken)
    {
        return AutoGenerateBuilderSyntaxReceiver.CheckSyntaxNode(syntaxNode);
    }

    private static FluentData Transform(GeneratorSyntaxContext gsc, CancellationToken cancellationToken)
    {
        return AutoGenerateBuilderSyntaxReceiver.HandleSyntaxNode(gsc.Node, gsc.SemanticModel);
    }
}