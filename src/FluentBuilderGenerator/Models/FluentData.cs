using FluentBuilderGenerator.Types;

namespace FluentBuilderGenerator.Models;

internal struct FluentData
{
    public string Namespace { get; init; }

    public string ClassModifier { get; init; }

    public string ShortBuilderClassName { get; init; }

    public string FullBuilderClassName { get; init; }

    public string FullRawTypeName { get; init; }

    public string ShortTypeName { get; init; }

    public string MetadataName { get; init; }

    public List<string> Usings { get; init; }

    public bool HandleBaseClasses { get; init; }

    public FluentBuilderAccessibility Accessibility { get; init; }

    public BuilderType BuilderType { get; init; }

    public FluentBuilderMethods Methods { get; init; } 
}