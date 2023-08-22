using FluentBuilderGenerator.Extensions;

namespace FluentBuilderGenerator.Helpers;

internal static class Aliashelper
{
    public static string GetAlias(string @using)
    {
        var alias = @using.Replace(".", string.Empty);
        return $"{alias}_{alias.GetDeterministicHashCodeAsString()}";
    }

    public static string GetFullAlias(string @using)
    {
        var alias = GetAlias(@using);
        return $"{alias} = {@using}";
    }
}