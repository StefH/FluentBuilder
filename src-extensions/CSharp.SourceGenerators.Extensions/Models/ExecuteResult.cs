using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace CSharp.SourceGenerators.Extensions.Models;

public class ExecuteResult
{
    /// <summary>
    /// The internally used GeneratorDriver is also returned here to support using https://github.com/VerifyTests/Verify.SourceGenerators.
    /// </summary>
    public GeneratorDriver GeneratorDriver { get; set; } = null!;

    /// <summary>
    /// A list of generated files.
    /// </summary>
    public IReadOnlyList<FileResult> Files { get; set; } = new List<FileResult>();

    /// <summary>
    /// A list of Errors
    /// </summary>
    public IReadOnlyList<string> ErrorMessages { get; set; } = new List<string>();

    /// <summary>
    /// A list of Warnings
    /// </summary>
    public IReadOnlyList<string> WarningMessages { get; set; } = new List<string>();

    /// <summary>
    /// Is the result valid.
    /// </summary>
    public bool Valid => ErrorMessages.Count == 0;
}