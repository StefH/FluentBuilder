using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace FluentBuilderGenerator.Extensions;

internal static class StringExtensions
{
    private static readonly Regex ExtractValueBetween = new("(?<=<).*(?=>)", RegexOptions.Compiled);

    public static bool TryGetGenericTypeArguments(this string input, [NotNullWhen(true)] out string? genericTypeArgumentValue)
    {
        genericTypeArgumentValue = null;

        var match = ExtractValueBetween.Match(input);

        if (match.Success)
        {
            genericTypeArgumentValue = match.Value;
            return true;
        }

        return false;
    }
}