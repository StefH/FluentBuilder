namespace FluentBuilderGenerator.Models;

internal record FluentData
(
    string Namespace,
    string ShortBuilderClassName,
    string FullBuilderClassName,
    string FullRawTypeName,
    string ShortTypeName,
    string FullTypeName,
    List<string> Usings
);