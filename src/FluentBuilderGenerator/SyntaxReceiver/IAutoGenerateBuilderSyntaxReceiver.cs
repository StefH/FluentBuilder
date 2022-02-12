using FluentBuilderGenerator.Models;
using Microsoft.CodeAnalysis;

namespace FluentBuilderGenerator.SyntaxReceiver;

internal interface IAutoGenerateBuilderSyntaxReceiver : ISyntaxReceiver
{
    public IList<FluentData> CandidateFluentDataItems { get; }
}