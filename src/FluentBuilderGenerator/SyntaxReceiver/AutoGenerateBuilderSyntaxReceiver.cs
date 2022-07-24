// This source code is based on https://justsimplycode.com/2020/12/06/auto-generate-builders-using-source-generator-in-net-5

using FluentBuilderGenerator.Extensions;
using FluentBuilderGenerator.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FluentBuilderGenerator.SyntaxReceiver;

internal class AutoGenerateBuilderSyntaxReceiver : IAutoGenerateBuilderSyntaxReceiver
{
    private static readonly string[] AutoGenerateBuilderAttributes = { "FluentBuilder.AutoGenerateBuilder", "AutoGenerateBuilder" };

    public IList<FluentData> CandidateFluentDataItems { get; } = new List<FluentData>();

    /// <summary>
    /// Called for every syntax node in the compilation, we can inspect the nodes and save any information useful for generation
    /// </summary>
    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax && TryGet(classDeclarationSyntax, out var data))
        {
            CandidateFluentDataItems.Add(data);
        }
    }

    private static bool TryGet(ClassDeclarationSyntax classDeclarationSyntax, out FluentData data)
    {
        data = default;

        var attributeList = classDeclarationSyntax.AttributeLists.FirstOrDefault(x => x.Attributes.Any(a => AutoGenerateBuilderAttributes.Contains(a.Name.ToString())));
        if (attributeList is null)
        {
            // ClassDeclarationSyntax should have the correct attribute
            return false;
        }

        var usings = new List<string>();

        string ns = classDeclarationSyntax.GetNamespace();
        if (!string.IsNullOrEmpty(ns))
        {
            usings.Add(ns);
        }

        if (classDeclarationSyntax.TryGetParentSyntax(out CompilationUnitSyntax? cc))
        {
            usings.AddRange(cc.Usings.Select(@using => @using.Name.ToString()));
        }

        // https://github.com/StefH/FluentBuilder/issues/36
        usings.AddRange(classDeclarationSyntax.GetAncestorsUsings().Select(@using => @using.Name.ToString()));

        usings = usings.Distinct().ToList();

        var (rawTypeName, handleBaseClasses) = ArgumentListParser.ParseAttributeArgumentList(attributeList.Attributes.FirstOrDefault()?.ArgumentList);

        if (rawTypeName != null) // The class which needs to be processed by the Builder is provided as type
        {
            var modifiers = classDeclarationSyntax.Modifiers.Select(m => m.ToString()).ToArray();
            if (!(modifiers.Contains("public") && modifiers.Contains("partial")))
            {
                // ClassDeclarationSyntax should be "public" + "partial"
                return false;
            }

            data = new FluentData
            {
                Namespace = ns,
                ShortBuilderClassName = $"{classDeclarationSyntax.Identifier}",
                FullBuilderClassName = CreateFullBuilderClassName(ns, classDeclarationSyntax),
                FullRawTypeName = rawTypeName,
                ShortTypeName = ConvertTypeName(rawTypeName).Split('.').Last(),
                MetadataName = ConvertTypeName(rawTypeName),
                Usings = usings,
                HandleBaseClasses = handleBaseClasses
            };

            return true;
        }

        var fullType = GetFullType(ns, classDeclarationSyntax, false); // FluentBuilderGeneratorTests.DTO.UserT<T>
        var fullBuilderType = GetFullType(ns, classDeclarationSyntax, true);
        var metadataName = classDeclarationSyntax.GetMetadataName();

        data = new FluentData
        {
            Namespace = ns,
            ShortBuilderClassName = $"{fullBuilderType.Split('.').Last()}",
            FullBuilderClassName = fullBuilderType,
            FullRawTypeName = fullType,
            ShortTypeName = ConvertTypeName(fullType).Split('.').Last(),
            MetadataName = metadataName,
            Usings = usings,
            HandleBaseClasses = handleBaseClasses
        };

        return true;
    }

    private static string GetFullType(string ns, ClassDeclarationSyntax classDeclarationSyntax, bool addBuilderPostfix)
    {
        var fullBuilderClassName = CreateFullBuilderClassName(ns, classDeclarationSyntax);
        var type = $"{fullBuilderClassName}{(addBuilderPostfix ? "Builder" : string.Empty)}";

        if (classDeclarationSyntax.TypeParameterList != null)
        {
            var list = classDeclarationSyntax.TypeParameterList.Parameters.Select(p => p.Identifier.ToString());
            return $"{type}<{string.Join(",", list)}>";
        }

        return type;
    }

    private static string CreateFullBuilderClassName(string ns, BaseTypeDeclarationSyntax classDeclarationSyntax)
    {
        return !string.IsNullOrEmpty(ns) ? $"{ns}.{classDeclarationSyntax.Identifier}" : classDeclarationSyntax.Identifier.ToString();
    }

    private static string ConvertTypeName(string typeName)
    {
        return !(typeName.Contains('<') && typeName.Contains('>')) ?
            typeName :
            $"{typeName.Replace("<", string.Empty).Replace(">", string.Empty).Replace(",", string.Empty).Trim()}`{typeName.Count(c => c == ',') + 1}";
    }
}