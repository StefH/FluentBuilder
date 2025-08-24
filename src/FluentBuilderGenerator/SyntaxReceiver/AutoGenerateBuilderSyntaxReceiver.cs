// This source code is based on https://justsimplycode.com/2020/12/06/auto-generate-builders-using-source-generator-in-net-5

using System.Diagnostics.CodeAnalysis;
using FluentBuilderGenerator.Constants;
using FluentBuilderGenerator.Extensions;
using FluentBuilderGenerator.Models;
using FluentBuilderGenerator.Types;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FluentBuilderGenerator.SyntaxReceiver;

internal static class AutoGenerateBuilderSyntaxReceiver
{
    private const string ModifierPartial = "partial";
    private const string ModifierPublic = "public";
    private const string ModifierInternal = "internal";

    public static bool CheckSyntaxNode(SyntaxNode syntaxNode, out Diagnostic? diagnostic)
    {
        TypeDeclarationSyntax typeDeclarationSyntax;
        if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax)
        {
            typeDeclarationSyntax = classDeclarationSyntax;
        }
        else if (syntaxNode is RecordDeclarationSyntax recordDeclarationSyntax)
        {
            typeDeclarationSyntax = recordDeclarationSyntax;
        }
        else
        {
            diagnostic = null;
            return false;
        }

        var attributeList = typeDeclarationSyntax.AttributeLists
            .FirstOrDefault(x => x.Attributes.Any(AttributeArgumentListParser.IsMatch));
        if (attributeList is null)
        {
            diagnostic = null;
            return false;
        }

        if (!TryGetModifier(typeDeclarationSyntax, out _))
        {
            diagnostic = Diagnostic.Create(DiagnosticDescriptors.ClassOrRecordModifierShouldBeInternalOrPublic, typeDeclarationSyntax.GetLocation());
            return false;
        }

        diagnostic = null;
        return true;
    }

    public static FluentData HandleSyntaxNode(SyntaxNode syntaxNode, SemanticModel semanticModel, out Diagnostic? diagnostic)
    {
        return syntaxNode switch
        {
            ClassDeclarationSyntax classDeclarationSyntax when TryGet(classDeclarationSyntax, semanticModel, out var data, out diagnostic) => data,
            RecordDeclarationSyntax recordDeclarationSyntax when TryGet(recordDeclarationSyntax, semanticModel, out var data, out diagnostic) => data,
            _ => throw new InvalidOperationException("Only classes or records are supported."),
        };
    }

    private static bool TryGet(TypeDeclarationSyntax typeDeclarationSyntax, SemanticModel semanticModel, out FluentData data, out Diagnostic? diagnostic)
    {
        data = default;

        var attributeList = typeDeclarationSyntax.AttributeLists
            .FirstOrDefault(x => x.Attributes.Any(AttributeArgumentListParser.IsMatch))!;

        if (!TryGetModifier(typeDeclarationSyntax, out var modifier))
        {
            diagnostic = Diagnostic.Create(DiagnosticDescriptors.ClassOrRecordModifierShouldBeInternalOrPublic, typeDeclarationSyntax.GetLocation());
            return false;
        }

        var usings = new List<string>();

        var ns = typeDeclarationSyntax.GetNamespace();
        if (!string.IsNullOrEmpty(ns))
        {
            usings.Add(ns);
        }

        if (typeDeclarationSyntax.TryGetParentSyntax(out CompilationUnitSyntax? cc))
        {
            usings.AddRange(cc.Usings.Select(@using => @using.Name!.ToString()));
        }

        // https://github.com/StefH/FluentBuilder/issues/36
        usings.AddRange(typeDeclarationSyntax.GetAncestorsUsings().Select(@using => @using.Name!.ToString()));

        usings = usings.Distinct().ToList();

        var fluentBuilderAttributeArguments = AttributeArgumentListParser.Parse(attributeList.Attributes.FirstOrDefault(), semanticModel);

        if (fluentBuilderAttributeArguments.RawTypeName != null) // The class which needs to be processed by the CustomBuilder is provided as type
        {
            if (!AreBuilderClassModifiersValid(typeDeclarationSyntax))
            {
                diagnostic = Diagnostic.Create(DiagnosticDescriptors.CustomBuilderClassModifierShouldBePartialAndInternalOrPublic, typeDeclarationSyntax.GetLocation());
                return false;
            }

            data = new FluentData
            {
                Namespace = ns,
                ClassModifier = modifier,
                ShortBuilderClassName = $"{typeDeclarationSyntax.Identifier}",
                FullBuilderClassName = CreateFullBuilderClassName(ns, typeDeclarationSyntax),
                FullRawTypeName = fluentBuilderAttributeArguments.RawTypeName,
                ShortTypeName = ConvertTypeName(fluentBuilderAttributeArguments.RawTypeName).Split('.').Last(),
                MetadataName = ConvertTypeName(fluentBuilderAttributeArguments.RawTypeName),
                Usings = usings,
                HandleBaseClasses = fluentBuilderAttributeArguments.HandleBaseClasses,
                Accessibility = fluentBuilderAttributeArguments.Accessibility,
                BuilderType = BuilderType.Custom,
                Methods = fluentBuilderAttributeArguments.Methods
            };

            diagnostic = null;
            return true;
        }

        var fullType = GetFullType(ns, typeDeclarationSyntax, false); // FluentBuilderGeneratorTests.DTO.UserT<T>
        var fullBuilderType = GetFullType(ns, typeDeclarationSyntax, true);
        var metadataName = typeDeclarationSyntax.GetMetadataName();

        data = new FluentData
        {
            Namespace = ns,
            ClassModifier = modifier,
            ShortBuilderClassName = $"{fullBuilderType.Split('.').Last()}",
            FullBuilderClassName = fullBuilderType,
            FullRawTypeName = fullType,
            ShortTypeName = ConvertTypeName(fullType).Split('.').Last(),
            MetadataName = metadataName,
            Usings = usings,
            HandleBaseClasses = fluentBuilderAttributeArguments.HandleBaseClasses,
            Accessibility = fluentBuilderAttributeArguments.Accessibility,
            BuilderType = BuilderType.Generated,
            Methods = fluentBuilderAttributeArguments.Methods
        };

        diagnostic = null;
        return true;
    }

    private static bool AreBuilderClassModifiersValid(MemberDeclarationSyntax classDeclarationSyntax)
    {
        var modifiers = classDeclarationSyntax.Modifiers.Select(m => m.ToString()).Distinct().ToArray();
        return modifiers.Contains(ModifierPartial) && (modifiers.Contains(ModifierPublic) || modifiers.Contains(ModifierInternal));
    }

    private static bool TryGetModifier(MemberDeclarationSyntax classDeclarationSyntax, [NotNullWhen(true)] out string? modifier)
    {
        var modifiers = classDeclarationSyntax.Modifiers.Select(m => m.ToString()).Distinct().ToArray();
        if (modifiers.Contains(ModifierPublic))
        {
            modifier = ModifierPublic;
            return true;
        }

        if (modifiers.Contains(ModifierInternal))
        {
            modifier = ModifierInternal;
            return true;
        }

        modifier = null;
        return false;
    }

    private static string GetFullType(string ns, TypeDeclarationSyntax classDeclarationSyntax, bool addBuilderPostfix)
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