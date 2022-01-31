// This source code is based on https://justsimplycode.com/2020/12/06/auto-generate-builders-using-source-generator-in-net-5

using System.Text;
using FluentBuilderGenerator.FileGenerators;
using FluentBuilderGenerator.SyntaxReceiver;
using FluentBuilderGenerator.Wrappers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace FluentBuilderGenerator;

[Generator]
internal class FluentBuilderSourceGenerator : ISourceGenerator
{
    private static readonly IFileGenerator[] Generators =
    {
        new AutoGenerateBuilderAttributeGenerator(),
        new BaseBuilderGenerator(),
        new IEnumerableBuilderGenerator(),
        // new ICollectionBuilderGenerator(),
        new IDictionaryBuilderGenerator()
    };

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

        InjectGeneratedClasses(context);

        if (context.SyntaxReceiver is not AutoGenerateBuilderSyntaxReceiver receiver)
        {
            return;
        }

        // https://github.com/reactiveui/refit/blob/main/InterfaceStubGenerator.Core/InterfaceStubGenerator.cs
        var supportsNullable = csharpParseOptions.LanguageVersion >= LanguageVersion.CSharp8;
        // var nullableEnabled = context.Compilation.Options.NullableContextOptions == NullableContextOptions.Enable;

        InjectFluentBuilderClasses(context, receiver, supportsNullable);
    }

    private static void InjectGeneratedClasses(GeneratorExecutionContext context)
    {
        foreach (var generator in Generators)
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