namespace FluentBuilderGenerator.Types;

internal record FluentBuilderAttributeParameters
{
    public string? RawTypeName { get; set; }

    public bool HandleBaseClasses { get; set; } = true;

    public FluentBuilderAccessibility Accessibility { get; set; } = FluentBuilderAccessibility.All;
}