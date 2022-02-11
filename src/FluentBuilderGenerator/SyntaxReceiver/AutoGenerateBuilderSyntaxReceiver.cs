// This source code is based on https://justsimplycode.com/2020/12/06/auto-generate-builders-using-source-generator-in-net-5

using System.Diagnostics.CodeAnalysis;
using FluentBuilderGenerator.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FluentBuilderGenerator.SyntaxReceiver;

internal class AutoGenerateBuilderSyntaxReceiver : IAutoGenerateBuilderSyntaxReceiver
{
    public IList<ClassDeclarationSyntax> CandidateClasses { get; } = new List<ClassDeclarationSyntax>();

    public IList<FluentData> CandidateFluentDataItems { get; } = new List<FluentData>();

    /// <summary>
    /// Called for every syntax node in the compilation, we can inspect the nodes and save any information useful for generation
    /// </summary>
    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax)
        {
            if (TryGet(classDeclarationSyntax, out var data))
            {
                CandidateFluentDataItems.Add(data);
            }
            else
            {
                CandidateClasses.Add(classDeclarationSyntax);
            }
        }
    }

    private static bool TryGet(ClassDeclarationSyntax classDeclarationSyntax, [NotNullWhen(true)] out FluentData? data)
    {
        data = null;

        var attributeLists = classDeclarationSyntax.AttributeLists.FirstOrDefault(x => x.Attributes.Any(a => a.Name.ToString().Equals("FluentBuilder.AutoGenerateBuilder")));
        if (attributeLists is null)
        {
            return false;
        }

        var usings = new List<string>();

        string ns = string.Empty;
        if (classDeclarationSyntax.TryGetParentSyntax(out NamespaceDeclarationSyntax? namespaceDeclarationSyntax))
        {
            ns = namespaceDeclarationSyntax.Name.ToString();
            usings.Add(ns);
        }

        if (classDeclarationSyntax.TryGetParentSyntax(out CompilationUnitSyntax? cc))
        {
            foreach (var @using in cc.Usings)
            {
                usings.Add(@using.Name.ToString());
            }
        }

        var argumentList = attributeLists.Attributes.FirstOrDefault()?.ArgumentList;
        if (argumentList != null && argumentList.Arguments.Any())
        {
            var typeSyntax = ((TypeOfExpressionSyntax)argumentList.Arguments[0].Expression).Type;
            string rawTypeName = typeSyntax.ToString();

            data = new
            (
                ns, // NameSpace
                classDeclarationSyntax.Identifier.ToString(), // ShortInterfaceName
                $"{ns}.{classDeclarationSyntax.Identifier}", // FullInterfaceName
                rawTypeName, // RawTypeName
                ConvertTypeName(rawTypeName).Split('.').Last(), // ShortTypeName
                ConvertTypeName(rawTypeName), // FullTypeName
                usings
            );

            return true;
        }

        return false;
    }

    private static string ConvertTypeName(string typeName)
    {
        return !(typeName.Contains('<') && typeName.Contains('>')) ?
            typeName :
            $"{typeName.Replace("<", string.Empty).Replace(">", string.Empty).Replace(",", string.Empty).Trim()}`{typeName.Count(c => c == ',') + 1}";
    }
}