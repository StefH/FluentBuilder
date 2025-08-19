using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AnyOfTypes;
using CSharp.SourceGenerators.Extensions.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharp.SourceGenerators.Extensions;

/// <summary>
/// Based on https://notanaverageman.github.io/2020/12/07/cs-source-generators-cheatsheet.html
/// </summary>
public static class SourceGeneratorExtensions
{
    /// <summary>
    /// Executes and runs the specified <see cref="ISourceGenerator"/>.
    /// </summary>
    /// <param name="sourceGenerator">The SourceGenerator to execute.</param>
    /// <param name="sources">Provide a list of sources which need to be analyzed and processed.</param>
    /// <param name="additionalTextPaths">A list of additional files.</param>
    /// <returns><see cref="ExecuteResult"/></returns>
    public static ExecuteResult Execute(
        this ISourceGenerator sourceGenerator,
        IReadOnlyList<SourceFile> sources,
        IReadOnlyList<string>? additionalTextPaths = null
    )
    {
        return Execute(sourceGenerator, $"GeneratedNamespace_{Guid.NewGuid().ToString().Replace("-", "")}", sources, additionalTextPaths);
    }

    /// <summary>
    /// Executes and runs the specified <see cref="ISourceGenerator"/>.
    /// </summary>
    /// <param name="sourceGenerator">The SourceGenerator to execute.</param>
    /// <param name="assemblyName">The assembly name.</param>
    /// <param name="sources">Provide a list of sources which need to be analyzed and processed.</param>
    /// <param name="additionalTextPaths">A list of additional files.</param>
    /// <returns><see cref="ExecuteResult"/></returns>
    public static ExecuteResult Execute(
        this ISourceGenerator sourceGenerator,
        string assemblyName,
        IReadOnlyList<SourceFile> sources,
        IReadOnlyList<string>? additionalTextPaths = null
    )
    {
        var metadataReferences = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => !a.IsDynamic)
            .Select(a => MetadataReference.CreateFromFile(a.Location))
            .ToArray();

        var sourceSyntaxTrees = sources.Select(GetSyntaxTree).ToArray();

        var additionalTexts = additionalTextPaths?.Select(tp => new CustomAdditionalText(tp)).ToArray() ?? Enumerable.Empty<AdditionalText>();

        var compilation = CSharpCompilation.Create(
            assemblyName,
            sourceSyntaxTrees,
            metadataReferences,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var driver = CSharpGeneratorDriver
            .Create(sourceGenerator)
            .AddAdditionalTexts(ImmutableArray.CreateRange(additionalTexts));

        var executedDriver = driver.RunGeneratorsAndUpdateCompilation(
            compilation,
            out Compilation outputCompilation,
            out ImmutableArray<Diagnostic> diagnostics);

        return new ExecuteResult
        {
            GeneratorDriver = executedDriver,
            WarningMessages = diagnostics.Where(d => d.Severity == DiagnosticSeverity.Warning).Select(d => d.GetMessage()).ToList(),
            ErrorMessages = diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error).Select(d => d.GetMessage()).ToList(),
            Files = outputCompilation.SyntaxTrees
                .Where(st => !sources.Any(s => s.Path == st.FilePath))
                .Select(st => new FileResult
                {
                    SyntaxTree = st,
                    Path = st.FilePath,
                    Text = st.ToString()
                })
                .ToList()
        };
    }

    private static SyntaxTree GetSyntaxTree(SourceFile source)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(source.Text, null, source.Path);
        if (source.AttributeToAddToClass is null && source.AttributeToAddToInterface is null)
        {
            // Both not defined, just return the SyntaxTree.
            return syntaxTree;
        }

        SyntaxNode rootSyntaxNode;
        if (source.AttributeToAddToClass is not null)
        {
            if (TryAddExtraAttribute<ClassDeclarationSyntax>(syntaxTree, source.AttributeToAddToClass.Value, out var classNode))
            {
                rootSyntaxNode = classNode;
            }
            else if (TryAddExtraAttribute<RecordDeclarationSyntax>(syntaxTree, source.AttributeToAddToClass.Value, out var recordNode))
            {
                rootSyntaxNode = recordNode;
            }
            else
            {
                throw new InvalidOperationException("If AttributeToAddToClass is defined, the target must be a record or class.");
            }
        }
        else if (source.AttributeToAddToInterface is not null && TryAddExtraAttribute<InterfaceDeclarationSyntax>(syntaxTree, source.AttributeToAddToInterface.Value, out var interfaceNode))
        {
            rootSyntaxNode = interfaceNode;
        }
        else
        {
            throw new InvalidOperationException();
        }

        // https://stackoverflow.com/questions/21754908/cant-update-changes-to-tree-roslyn
        var updatedText = rootSyntaxNode.GetText().ToString();
        return CSharpSyntaxTree.ParseText(updatedText, null, source.Path);
    }

    private static bool TryAddExtraAttribute<T>(SyntaxTree syntaxTree, AnyOf<string, ExtraAttribute> attributeToAdd, [NotNullWhen(true)] out SyntaxNode? syntaxNode)
        where T : TypeDeclarationSyntax
    {
        var rootSyntaxNode = syntaxTree.GetRoot();
        var descendantNodes = rootSyntaxNode.DescendantNodes().OfType<T>().ToArray();
        if (!descendantNodes.Any())
        {
            syntaxNode = null;
            return false; // No class, record or interface found to add the attribute.
        }

        foreach (var classDeclaration in descendantNodes)
        {
            var name = attributeToAdd.IsFirst ? attributeToAdd.First : attributeToAdd.Second.Name;

            // https://stackoverflow.com/questions/35927427/how-to-create-an-attributesyntax-with-a-parameter
            var nameSyntax = SyntaxFactory.ParseName(name);
            if (nameSyntax is QualifiedNameSyntax { Right: GenericNameSyntax genericNameSyntax })
            {
                nameSyntax = genericNameSyntax;
            }

            AttributeSyntax attributeSyntax;
            if (TryParseArguments(attributeToAdd, out var attributeArgumentListSyntax))
            {
                attributeSyntax = SyntaxFactory.Attribute(nameSyntax, attributeArgumentListSyntax);
            }
            else
            {
                attributeSyntax = SyntaxFactory.Attribute(nameSyntax);
            }

            var separatedSyntaxList = new SeparatedSyntaxList<AttributeSyntax>().Add(attributeSyntax);

            var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
            var newClassDeclarationSyntax = classDeclaration.AddAttributeLists(attributeListSyntax);

            // https://stackoverflow.com/questions/44060524/why-does-syntaxnode-replacenode-change-the-syntaxtree-options
            rootSyntaxNode = rootSyntaxNode.ReplaceNode(classDeclaration, newClassDeclarationSyntax);
        }

        syntaxNode = rootSyntaxNode;
        return true;
    }

    private static bool TryParseArguments(AnyOf<string, ExtraAttribute> attributeToAdd, [NotNullWhen(true)] out AttributeArgumentListSyntax? attributeArgumentListSyntax)
    {
        if (attributeToAdd.IsSecond && attributeToAdd.Second.ArgumentList is not null)
        {
            var arguments = attributeToAdd.Second.ArgumentList.Value.IsFirst ?
                attributeToAdd.Second.ArgumentList.Value.First :
                string.Join(",", attributeToAdd.Second.ArgumentList.Value.Second);

            attributeArgumentListSyntax = SyntaxFactory.ParseAttributeArgumentList($"({arguments})");
            return attributeArgumentListSyntax != null;
        }

        attributeArgumentListSyntax = null;
        return false;
    }
}