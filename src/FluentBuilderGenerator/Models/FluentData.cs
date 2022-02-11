namespace FluentBuilderGenerator.Models;

internal record FluentData
(
    string Namespace,
    string ShortClassName,
    string FullClassName,
    string FullRawTypeName,
    string ShortTypeName,
    string FullTypeName,
    List<string> Usings
);