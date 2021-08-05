using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;

namespace FluentBuilderGenerator.Extensions
{
    internal static class SyntaxNodeExtensions
    {
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

        // https://stackoverflow.com/questions/20458457/getting-class-fullname-including-namespace-from-roslyn-classdeclarationsyntax
        public static bool TryGetChildSyntax<T>(this SyntaxNode? syntaxNode, [NotNullWhen(true)] out T? result) where T : SyntaxNode
        {
            result = null;

            if (syntaxNode is null)
            {
                return false;
            }

            if (syntaxNode.GetType() == typeof(T))
            {
                result = (T)syntaxNode;
                return true;
            }

            try
            {
                foreach (var child in syntaxNode.ChildNodes())
                {
                    if (TryGetChildSyntax(child, out result))
                    {
                        return true;
                    }
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}