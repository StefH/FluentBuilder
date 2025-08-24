using Microsoft.CodeAnalysis;

namespace FluentBuilderGenerator.Constants;

internal static class DiagnosticDescriptors
{
    internal static readonly DiagnosticDescriptor Error = new(
        "FBGERR0001",
        "Source generator exception",
        "An exception occurred during source generation: {0}",
        nameof(FluentBuilderGenerator),
        DiagnosticSeverity.Error,
        true,
        "An unhandled exception occurred while generating source code."
    );

    internal static readonly DiagnosticDescriptor ClassOrRecordModifierShouldBeInternalOrPublic = new(
        "FBGINF0001",
        "Information",
        "Class or Record modifier should be 'public' or 'internal'",
        nameof(FluentBuilderGenerator),
        DiagnosticSeverity.Info,
        true
    );

    internal static readonly DiagnosticDescriptor CustomBuilderClassModifierShouldBePartialAndInternalOrPublic = new(
        "FBGINF0002",
        "Information",
        "Custom builder class should be 'partial' and 'public' or 'internal'",
        nameof(FluentBuilderGenerator),
        DiagnosticSeverity.Info,
        true
    );
}