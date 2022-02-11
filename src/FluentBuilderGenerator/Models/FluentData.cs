namespace FluentBuilderGenerator.Models;

internal record FluentData
(
    string Namespace,
    string ShortInterfaceName,
    string FullInterfaceName,
    string FullRawTypeName,
    string ShortTypeName,
    string FullTypeName,
    List<string> Usings
);