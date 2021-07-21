using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FluentBuilderGenerator
{
    internal interface IAutoGenerateBuilderSyntaxReceiver
    {
        public IList<ClassDeclarationSyntax> CandidateClasses { get; }
    }
}