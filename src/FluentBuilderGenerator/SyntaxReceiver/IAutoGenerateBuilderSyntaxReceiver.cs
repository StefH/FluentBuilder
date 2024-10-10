using FluentBuilderGenerator.Models;
using Microsoft.CodeAnalysis;

namespace FluentBuilderGenerator.SyntaxReceiver;

internal interface IAutoGenerateBuilderSyntaxReceiver : ISyntaxContextReceiver
{
    public IList<FluentData> CandidateFluentDataItems { get; }
}