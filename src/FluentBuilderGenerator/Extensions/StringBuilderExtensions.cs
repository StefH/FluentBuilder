// ReSharper disable once CheckNamespace
namespace System.Text;

internal static class StringBuilderExtensions
{
    public static StringBuilder AppendLine(this StringBuilder sb, bool ifTrue, string value)
    {
        if (ifTrue)
        {
            sb.AppendLine(value);
        }

        return sb;
    }

    public static StringBuilder AppendLine(this StringBuilder sb, int spaces, string value)
    {
        return sb.AppendLine($"{new string(' ', spaces)}{value}");
    }

    public static StringBuilder AppendLines(this StringBuilder sb, int spaces, IEnumerable<string> values, string postFix = "")
    {
        return sb.AppendLines(values.Select(v => $"{new string(' ', spaces)}{v}"), postFix);
    }

    public static StringBuilder AppendLines(this StringBuilder sb, IEnumerable<string> values, string postFix = "")
    {
        return sb.AppendLine(string.Join($"{postFix}\r\n", values));
    }
}