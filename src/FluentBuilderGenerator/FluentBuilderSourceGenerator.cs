// This source code is based on https://justsimplycode.com/2020/12/06/auto-generate-builders-using-source-generator-in-net-5

using System.Text;
using FluentBuilderGenerator.FileGenerators;
using FluentBuilderGenerator.SyntaxReceiver;
using FluentBuilderGenerator.Types;
using FluentBuilderGenerator.Wrappers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace FluentBuilderGenerator;

[Generator]
internal class FluentBuilderSourceGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        //if (!System.Diagnostics.Debugger.IsAttached)
        //{
        //    System.Diagnostics.Debugger.Launch();
        //}

        context.RegisterForSyntaxNotifications(() => new AutoGenerateBuilderSyntaxReceiver());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        if (context.ParseOptions is not CSharpParseOptions csharpParseOptions)
        {
            throw new NotSupportedException("Only C# is supported.");
        }

        // https://github.com/reactiveui/refit/blob/main/InterfaceStubGenerator.Core/InterfaceStubGenerator.cs
        var supportsNullable = csharpParseOptions.LanguageVersion >= LanguageVersion.CSharp8;
        // var nullableEnabled = context.Compilation.Options.NullableContextOptions == NullableContextOptions.Enable;

        InjectGeneratedClasses(context, supportsNullable);

        if (context.SyntaxReceiver is not AutoGenerateBuilderSyntaxReceiver receiver)
        {
            return;
        }

        InjectFluentBuilderClasses(context, receiver, supportsNullable);
    }

    private static void InjectGeneratedClasses(GeneratorExecutionContext context, bool supportsNullable)
    {
        var generators = new IFileGenerator[]
        {
            new AutoGenerateBuilderAttributeGenerator(),
            new BaseBuilderGenerator(),
            new IEnumerableBuilderGenerator(FileDataType.ArrayBuilder, supportsNullable),
            new IEnumerableBuilderGenerator(FileDataType.IEnumerableBuilder, supportsNullable),
            new IEnumerableBuilderGenerator(FileDataType.IListBuilder, supportsNullable),
            new IEnumerableBuilderGenerator(FileDataType.ICollectionBuilder, supportsNullable),
            new IDictionaryBuilderGenerator(supportsNullable)
        };

        foreach (var generator in generators)
        {
            var data = generator.GenerateFile();
            context.AddSource(data.FileName, SourceText.From(data.Text, Encoding.UTF8));
        }
    }

    private static void InjectFluentBuilderClasses(GeneratorExecutionContext context, IAutoGenerateBuilderSyntaxReceiver receiver, bool supportsNullable)
    {
        var contextWrapper = new GeneratorExecutionContextWrapper(context);

        var generator = new FluentBuilderClassesGenerator(contextWrapper, receiver, supportsNullable);

        foreach (var data in generator.GenerateFiles())
        {
            context.AddSource(data.FileName, SourceText.From(data.Text, Encoding.UTF8));
        }
    }
}