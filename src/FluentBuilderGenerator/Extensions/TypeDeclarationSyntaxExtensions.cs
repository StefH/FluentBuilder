using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FluentBuilderGenerator.Extensions;

/// <summary>
/// https://stackoverflow.com/questions/20458457/getting-class-fullname-including-namespace-from-roslyn-classdeclarationsyntax
/// </summary>
internal static class TypeDeclarationSyntaxExtensions
{
    const char NESTED_CLASS_DELIMITER = '+';
    const char NAMESPACE_CLASS_DELIMITER = '.';
    const char TYPEPARAMETER_CLASS_DELIMITER = '`';

    public static string GetMetadataName(this TypeDeclarationSyntax source)
    {
        var namespaces = new LinkedList<NamespaceDeclarationSyntax>();
        var types = new LinkedList<TypeDeclarationSyntax>();
        for (var parent = source.Parent; parent is not null; parent = parent.Parent)
        {
            if (parent is NamespaceDeclarationSyntax @namespace)
            {
                namespaces.AddFirst(@namespace);
            }
            else if (parent is TypeDeclarationSyntax type)
            {
                types.AddFirst(type);
            }
        }

        var result = new StringBuilder();
        for (var item = namespaces.First; item is not null; item = item.Next)
        {
            result.Append(item.Value.Name).Append(NAMESPACE_CLASS_DELIMITER);
        }

        for (var item = types.First; item is not null; item = item.Next)
        {
            var type = item.Value;
            AppendName(result, type);
            result.Append(NESTED_CLASS_DELIMITER);
        }
        AppendName(result, source);

        return result.ToString();
    }

    private static void AppendName(StringBuilder builder, TypeDeclarationSyntax type)
    {
        builder.Append(type.Identifier.Text);
        var typeArguments = type.TypeParameterList?.ChildNodes().Count(node => node is TypeParameterSyntax) ?? 0;
        if (typeArguments != 0)
        {
            builder.Append(TYPEPARAMETER_CLASS_DELIMITER).Append(typeArguments);
        }
    }
}