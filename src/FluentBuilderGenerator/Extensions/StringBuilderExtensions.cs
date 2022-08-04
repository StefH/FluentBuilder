// ReSharper disable once CheckNamespace
namespace System.Text;

internal static class StringBuilderExtensions
{
    public static void AppendLines(this StringBuilder sb, IEnumerable<string> values, string prefix = "")
    {
        sb.AppendLine(string.Join($"{prefix}\r\n", values));
    }
}