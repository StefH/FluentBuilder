using Microsoft.CodeAnalysis;

namespace FluentBuilderGenerator.Constants;

internal static class DiagnosticDescriptors
{
    internal static readonly DiagnosticDescriptor ErrorException = new(
        "SGERR0001",
        "Source generator exception",
        "An exception occurred during source generation: {0}",
        nameof(FluentBuilderGenerator),
        DiagnosticSeverity.Error,
        true,
        "An unhandled exception occurred while generating source code."
    );

    internal static readonly DiagnosticDescriptor Information = new(
        "SGINF0001",
        "Information",
        "{0}",
        nameof(FluentBuilderGenerator),
        DiagnosticSeverity.Info,
        true
    );
}