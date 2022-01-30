// This source code is based on https://justsimplycode.com/2020/12/06/auto-generate-builders-using-source-generator-in-net-5
using System;
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
    private static readonly IFileGenerator BaseBuilderGenerator = new BaseBuilderGenerator();
    private static readonly IFileGenerator FluentIEnumerableBuilderGenerator = new IEnumerableBuilderGenerator();
    private static readonly IFileGenerator AutoGenerateBuilderAttributeGenerator = new AutoGenerateBuilderAttributeGenerator();

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

        InjectAutoGenerateBuilderAttributeClass(context);
        InjectBaseBuilderClass(context);
        InjectFluentIEnumerableBuilderClass(context);

        if (context.SyntaxReceiver is not AutoGenerateBuilderSyntaxReceiver receiver)
        {
            return;
        }

        // https://github.com/reactiveui/refit/blob/main/InterfaceStubGenerator.Core/InterfaceStubGenerator.cs
        var supportsNullable = csharpParseOptions.LanguageVersion >= LanguageVersion.CSharp8;
        // var nullableEnabled = context.Compilation.Options.NullableContextOptions == NullableContextOptions.Enable;

        InjectFluentBuilderClasses(context, receiver, supportsNullable);
    }

    private static void InjectAutoGenerateBuilderAttributeClass(GeneratorExecutionContext context)
    {
        var data = AutoGenerateBuilderAttributeGenerator.GenerateFile();
        context.AddSource(data.FileName, SourceText.From(data.Text, Encoding.UTF8));
    }

    private static void InjectBaseBuilderClass(GeneratorExecutionContext context)
    {
        var data = BaseBuilderGenerator.GenerateFile();
        context.AddSource(data.FileName, SourceText.From(data.Text, Encoding.UTF8));
    }

    private static void InjectFluentIEnumerableBuilderClass(GeneratorExecutionContext context)
    {
        var data = FluentIEnumerableBuilderGenerator.GenerateFile();
        context.AddSource(data.FileName, SourceText.From(data.Text, Encoding.UTF8));
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