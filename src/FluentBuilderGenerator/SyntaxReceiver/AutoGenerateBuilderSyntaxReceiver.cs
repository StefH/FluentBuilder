// This source code is based on https://justsimplycode.com/2020/12/06/auto-generate-builders-using-source-generator-in-net-5

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FluentBuilderGenerator.SyntaxReceiver;

internal class AutoGenerateBuilderSyntaxReceiver : IAutoGenerateBuilderSyntaxReceiver
{
    public IList<ClassDeclarationSyntax> CandidateClasses { get; } = new List<ClassDeclarationSyntax>();

    /// <summary>
    /// Called for every syntax node in the compilation, we can inspect the nodes and save any information useful for generation
    /// </summary>
    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        // any field with at least AutoGenerateBuilder attribute is a candidate for property generation
        if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax &&
            classDeclarationSyntax.AttributeLists.Any(x => x.Attributes.Any(a => a.Name.ToString().Equals("FluentBuilder.AutoGenerateBuilder"))))
        {
            CandidateClasses.Add(classDeclarationSyntax);
        }
    }
}