namespace FluentBuilderGenerator.Types;

internal record FluentBuilderAttributeArguments
{
    public string? RawTypeName { get; set; }

    public bool HandleBaseClasses { get; set; } = true;

    public FluentBuilderAccessibility Accessibility { get; set; } = FluentBuilderAccessibility.Public;
}