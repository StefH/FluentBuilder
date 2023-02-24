// ReSharper disable once CheckNamespace
namespace System.Text;

internal static class StringBuilderExtensions
{
    public static void AppendLine(this StringBuilder sb, int spaces, string value)
    {
        sb.AppendLine($"{new string(' ', spaces)}{value}");
    }

    public static void AppendLines(this StringBuilder sb, int spaces, IEnumerable<string> values, string postFix = "")
    {
        sb.AppendLines(values.Select(v => $"{new string(' ', spaces)}{v}"), postFix);
    }

    public static void AppendLines(this StringBuilder sb, IEnumerable<string> values, string postFix = "")
    {
        sb.AppendLine(string.Join($"{postFix}\r\n", values));
    }
}