using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace CSharp.SourceGenerators.Extensions.Models;

public class ExecuteResult
{
    /// <summary>
    /// The internally used GeneratorDriver is also returned here to support using https://github.com/VerifyTests/Verify.SourceGenerators.
    /// </summary>
    public required GeneratorDriver GeneratorDriver { get; init; }

    /// <summary>
    /// A list of generated files.
    /// </summary>
    public required IReadOnlyList<FileResult> Files { get; init; }

    /// <summary>
    /// Gets the collection of diagnostics associated with the current operation or state.
    /// </summary>
    public required IReadOnlyList<Diagnostic> Diagnostics { get; init; }

    /// <summary>
    /// A list of Error messages
    /// </summary>
    public required IReadOnlyList<string> ErrorMessages { get; init; }

    /// <summary>
    /// A list of Warning messages
    /// </summary>
    public required IReadOnlyList<string> WarningMessages { get; init; }

    /// <summary>
    /// A list of Information messages
    /// </summary>
    public required IReadOnlyList<string> InformationMessages { get; init; }

    /// <summary>
    /// Is the result valid.
    /// </summary>
    public bool Valid => ErrorMessages.Count == 0;
}