// This source code is based on https://justsimplycode.com/2020/12/06/auto-generate-builders-using-source-generator-in-net-5
using System.Text;
using FluentBuilderGenerator.FileGenerators;
using FluentBuilderGenerator.Wrappers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace FluentBuilderGenerator
{
    [Generator]
    public class FluentBuilderSourceGenerator : ISourceGenerator
    {
        private static readonly IFileGenerator BaseBuilderGenerator = new BaseBuilderGenerator();
        private static readonly IFileGenerator AutoGenerateBuilderAttributeGenerator = new AutoGenerateBuilderAttributeGenerator();

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new AutoGenerateBuilderSyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            InjectAutoGenerateBuilderAttributeClass(context);
            InjectBaseBuilderClass(context);

            if (context.SyntaxReceiver is not AutoGenerateBuilderSyntaxReceiver receiver)
            {
                return;
            }

            InjectFluentBuilderClasses(context, receiver);
        }

        private static void InjectBaseBuilderClass(GeneratorExecutionContext context)
        {
            var data = BaseBuilderGenerator.GenerateFile();
            context.AddSource(data.FileName, SourceText.From(data.Text, Encoding.UTF8));
        }

        private static void InjectAutoGenerateBuilderAttributeClass(GeneratorExecutionContext context)
        {
            var data = AutoGenerateBuilderAttributeGenerator.GenerateFile();
            context.AddSource(data.FileName, SourceText.From(data.Text, Encoding.UTF8));
        }

        private void InjectFluentBuilderClasses(GeneratorExecutionContext context, IAutoGenerateBuilderSyntaxReceiver receiver)
        {
            var contextWrapper = new GeneratorExecutionContextWrapper(context);

            var generator = new FluentBuilderClassesGenerator(contextWrapper, receiver);

            foreach (var data in generator.GenerateFiles())
            {
                context.AddSource(data.FileName, SourceText.From(data.Text, Encoding.UTF8));
            }
        }
    }
}