using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FluentBuilderGenerator.SyntaxReceiver
{
    internal interface IAutoGenerateBuilderSyntaxReceiver
    {
        public IList<ClassDeclarationSyntax> CandidateClasses { get; }
    }
}