using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;

namespace FluentBuilderGenerator.Extensions;

internal static class StringExtensions
{
    private static readonly Regex ExtractValueBetween = new("(?<=<).*(?=>)", RegexOptions.Compiled);

    public static string If(this bool value, string valueTrue, string valueFalse = "")
    {
        return value ? valueTrue : valueFalse;
    }

    public static bool TryGetGenericTypeArguments(this string input, [NotNullWhen(true)] out string? genericTypeArgumentValue)
    {
        var match = ExtractValueBetween.Match(input);

        if (match.Success)
        {
            genericTypeArgumentValue = match.Value;
            return true;
        }

        genericTypeArgumentValue = null;
        return false;
    }

    // See https://andrewlock.net/why-is-string-gethashcode-different-each-time-i-run-my-program-in-net-core/
    public static string GetDeterministicHashCodeAsString(this string str)
    {
        unchecked
        {
            int hash1 = (5381 << 16) + 5381;
            int hash2 = hash1;

            for (int i = 0; i < str.Length; i += 2)
            {
                hash1 = ((hash1 << 5) + hash1) ^ str[i];
                if (i == str.Length - 1)
                {
                    break;
                }

                hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
            }

            int result = hash1 + hash2 * 1566083941;

            return result.ToString(CultureInfo.InvariantCulture).Replace('-', '_');
        }
    }
}