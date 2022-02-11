using FluentBuilderGenerator.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FluentBuilderGenerator.SyntaxReceiver;

internal interface IAutoGenerateBuilderSyntaxReceiver : ISyntaxReceiver
{
    // public IList<ClassDeclarationSyntax> CandidateClasses { get; }

    public IList<FluentData> CandidateFluentDataItems { get; }
}