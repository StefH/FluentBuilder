namespace FluentBuilderGenerator.Models;

internal struct FluentData
{
    public string Namespace { get; init; }
    public string ShortBuilderClassName { get; init; }
    public string FullBuilderClassName { get; init; }
    public string FullRawTypeName { get; init; }
    public string ShortTypeName { get; init; }
    public string MetadataName { get; init; }
    public List<string> Usings { get; init; }
}