using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

// ReSharper disable once CheckNamespace
namespace Microsoft.CodeAnalysis;

internal static class SyntaxNodeExtensions
{
    /// <summary>
    /// determine the namespace the class/enum/struct is declared in, if any
    /// https://andrewlock.net/creating-a-source-generator-part-5-finding-a-type-declarations-namespace-and-type-hierarchy/
    /// </summary>
    /// <param name="syntaxNode"></param>
    /// <returns>NameSpace</returns>
    public static string GetNamespace(this SyntaxNode syntaxNode)
    {
        // If we don't have a namespace at all we'll return an empty string
        // This accounts for the "default namespace" case
        var nameSpace = string.Empty;

        // Get the containing syntax node for the type declaration
        // (could be a nested type, for example)
        var potentialNamespaceParent = syntaxNode.Parent;

        // Keep moving "out" of nested classes until we get to a namespace or until we run out of parents
        while (potentialNamespaceParent != null &&
               potentialNamespaceParent is not NamespaceDeclarationSyntax
               && potentialNamespaceParent is not FileScopedNamespaceDeclarationSyntax)
        {
            potentialNamespaceParent = potentialNamespaceParent.Parent;
        }

        // Build up the final namespace by looping until we no longer have a namespace declaration
        if (potentialNamespaceParent is BaseNamespaceDeclarationSyntax namespaceParent)
        {
            // We have a namespace. Use that as the type
            nameSpace = namespaceParent.Name.ToString();

            // Keep moving "out" of the namespace declarations until we 
            // run out of nested namespace declarations
            while (true)
            {
                if (namespaceParent.Parent is not NamespaceDeclarationSyntax parent)
                {
                    break;
                }

                // Add the outer namespace as a prefix to the final namespace
                nameSpace = $"{namespaceParent.Name}.{nameSpace}";
                namespaceParent = parent;
            }
        }

        // return the final namespace
        return nameSpace;
    }


    // https://stackoverflow.com/questions/20458457/getting-class-fullname-including-namespace-from-roslyn-classdeclarationsyntax
    public static bool TryGetParentSyntax<T>(this SyntaxNode? syntaxNode, [NotNullWhen(true)] out T? result) where T : SyntaxNode
    {
        result = null;

        if (syntaxNode is null)
        {
            return false;
        }

        try
        {
            syntaxNode = syntaxNode.Parent;

            if (syntaxNode is null)
            {
                return false;
            }

            if (syntaxNode.GetType() == typeof(T))
            {
                result = (T)syntaxNode;
                return true;
            }

            return TryGetParentSyntax(syntaxNode, out result);
        }
        catch
        {
            return false;
        }
    }

    // https://stackoverflow.com/questions/49970813/collect-usings-from-all-enclosing-namespaces-having-an-itypesymbol
    public static IReadOnlyList<UsingDirectiveSyntax> GetAncestorsUsings(this SyntaxNode syntaxNode)
    {
        var allUsings = SyntaxFactory.List<UsingDirectiveSyntax>();

        foreach (var parent in syntaxNode.Ancestors(false))
        {
            allUsings = parent switch
            {
                NamespaceDeclarationSyntax namespaceDeclarationSyntax => allUsings.AddRange(namespaceDeclarationSyntax.Usings),
                CompilationUnitSyntax compilationUnitSyntax => allUsings.AddRange(compilationUnitSyntax.Usings),
                _ => allUsings
            };
        }

        return allUsings;
    }

    public static IReadOnlyList<T> FindDescendantNodes<T>(this SyntaxNode syntaxNode, Func<T, bool>? predicate = null) where T : SyntaxNode
    {
        return syntaxNode.DescendantNodes().OfType<T>().Where(x => predicate == null || predicate(x)).ToList();
    }

    public static T? FindDescendantNode<T>(this SyntaxNode syntaxNode, Func<T, bool>? predicate = null) where T : SyntaxNode
    {
        return syntaxNode.DescendantNodes().OfType<T>().FirstOrDefault(x => predicate == null || predicate(x));
    }
}