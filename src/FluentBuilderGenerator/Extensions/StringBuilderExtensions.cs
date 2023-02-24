// ReSharper disable once CheckNamespace
namespace System.Text;

internal static class StringBuilderExtensions
{
    public static void AppendLine(this StringBuilder sb, int spaces, string value)
    {
        sb.AppendLine($"{new string(' ', spaces)}{value}");
    }

    public static void AppendLines(this StringBuilder sb, int spaces, IEnumerable<string> values, string prefix = "")
    {
        sb.AppendLines(values.Select(v => $"{new string(' ', spaces)}{v}"), prefix);
    }

    public static void AppendLines(this StringBuilder sb, IEnumerable<string> values, string prefix = "")
    {
        sb.AppendLine(string.Join($"{prefix}\r\n", values));
    }
}