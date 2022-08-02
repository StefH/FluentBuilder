// This source code is based on https://justsimplycode.com/2020/12/06/auto-generate-builders-using-source-generator-in-net-5

using System.Text;
using FluentBuilderGenerator.FileGenerators;
using FluentBuilderGenerator.SyntaxReceiver;
using FluentBuilderGenerator.Types;
using FluentBuilderGenerator.Wrappers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace FluentBuilderGenerator;

[Generator]
internal class FluentBuilderSourceGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
#if DEBUGATTACH
        if (!System.Diagnostics.Debugger.IsAttached)
        {
            System.Diagnostics.Debugger.Launch();
        }
#endif
        context.RegisterForSyntaxNotifications(() => new AutoGenerateBuilderSyntaxReceiver());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        var contextWrapper = new GeneratorExecutionContextWrapper(context);

        try
        {
            InjectGeneratedClasses(contextWrapper);

            if (context.SyntaxReceiver is not AutoGenerateBuilderSyntaxReceiver receiver)
            {
                return;
            }

            InjectFluentBuilderClasses(contextWrapper, receiver);
        }
        catch (Exception exception)
        {
            GenerateError(contextWrapper, exception);
        }
    }

    private static void GenerateError(IGeneratorExecutionContextWrapper context, Exception exception)
    {
        var message = $"/*\r\n{nameof(FluentBuilderSourceGenerator)}\r\n\r\n[Exception]\r\n{exception}\r\n\r\n[StackTrace]\r\n{exception.StackTrace}*/";
        context.AddSource("Error.g.cs", SourceText.From(message, Encoding.UTF8));
    }

    private static void InjectGeneratedClasses(IGeneratorExecutionContextWrapper context)
    {
        var generators = new IFileGenerator[]
        {
            new ExtraFilesGenerator(context.AssemblyName, context.SupportsNullable),
            new BaseBuilderGenerator(context.AssemblyName),
            new IEnumerableBuilderGenerator(context.AssemblyName, FileDataType.ArrayBuilder, context.SupportsNullable),
            new IEnumerableBuilderGenerator(context.AssemblyName, FileDataType.IEnumerableBuilder, context.SupportsNullable),
            new IEnumerableBuilderGenerator(context.AssemblyName, FileDataType.IListBuilder, context.SupportsNullable),
            new IEnumerableBuilderGenerator(context.AssemblyName, FileDataType.IReadOnlyCollectionBuilder, context.SupportsNullable),
            new IEnumerableBuilderGenerator(context.AssemblyName, FileDataType.ICollectionBuilder, context.SupportsNullable),
            new IDictionaryBuilderGenerator(context.AssemblyName, context.SupportsNullable)
        };

        foreach (var generator in generators)
        {
            var data = generator.GenerateFile();
            context.AddSource(data.FileName, SourceText.From(data.Text, Encoding.UTF8));
        }
    }

    private static void InjectFluentBuilderClasses(IGeneratorExecutionContextWrapper context, IAutoGenerateBuilderSyntaxReceiver receiver)
    {
        var generator = new FluentBuilderClassesGenerator(context, receiver);

        foreach (var data in generator.GenerateFiles())
        {
            context.AddSource(data.FileName, SourceText.From(data.Text, Encoding.UTF8));
        }
    }
}