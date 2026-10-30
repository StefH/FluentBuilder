using FluentBuilderGenerator.Types;

namespace FluentBuilderGenerator.Models;

internal struct FluentData : IEquatable<FluentData>
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

    public bool Equals(FluentData other)
    {
        return Namespace == other.Namespace &&
               ClassModifier == other.ClassModifier &&
               ShortBuilderClassName == other.ShortBuilderClassName &&
               FullBuilderClassName == other.FullBuilderClassName &&
               FullRawTypeName == other.FullRawTypeName &&
               ShortTypeName == other.ShortTypeName &&
               MetadataName == other.MetadataName &&
               HandleBaseClasses == other.HandleBaseClasses &&
               Accessibility == other.Accessibility &&
               BuilderType == other.BuilderType &&
               Methods == other.Methods &&
               (Usings == other.Usings || (Usings != null && other.Usings != null && Usings.SequenceEqual(other.Usings)));
    }

    public override bool Equals(object? obj) => obj is FluentData other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = 17;
            hash = hash * 31 + (Namespace?.GetHashCode() ?? 0);
            hash = hash * 31 + (ClassModifier?.GetHashCode() ?? 0);
            hash = hash * 31 + (ShortBuilderClassName?.GetHashCode() ?? 0);
            hash = hash * 31 + (FullBuilderClassName?.GetHashCode() ?? 0);
            hash = hash * 31 + (FullRawTypeName?.GetHashCode() ?? 0);
            hash = hash * 31 + (ShortTypeName?.GetHashCode() ?? 0);
            hash = hash * 31 + (MetadataName?.GetHashCode() ?? 0);
            hash = hash * 31 + HandleBaseClasses.GetHashCode();
            hash = hash * 31 + Accessibility.GetHashCode();
            hash = hash * 31 + BuilderType.GetHashCode();
            hash = hash * 31 + Methods.GetHashCode();
            if (Usings != null)
            {
                foreach (var u in Usings)
                {
                    hash = hash * 31 + (u?.GetHashCode() ?? 0);
                }
            }
            return hash;
        }
    }
}