namespace FluentBuilderGenerator.Types;

[Flags]
internal enum FluentBuilderAccessibility
{
    All = 0,
    Private = 1,
    // ProtectedAndInternal = 2,
    // ProtectedAndFriend = ProtectedAndInternal,
    // Protected = 3,
    // Internal = 4,
    // Friend = Internal,
    // ProtectedOrInternal = 5,
    // ProtectedOrFriend = ProtectedOrInternal,
    Public = 6
}