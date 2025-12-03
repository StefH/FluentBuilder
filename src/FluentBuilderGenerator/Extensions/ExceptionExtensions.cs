using FluentBuilderGenerator.Constants;
using Microsoft.CodeAnalysis;

namespace FluentBuilderGenerator.Extensions;

internal static class ExceptionExtensions
{
    internal static Diagnostic ToDiagnostic(this Exception exception)
    {
        return Diagnostic.Create(DiagnosticDescriptors.Error, Location.None, exception);
    }
}