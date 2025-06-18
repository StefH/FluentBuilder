// This source code is based on https://justsimplycode.com/2020/12/06/auto-generate-builders-using-source-generator-in-net-5

using System.Diagnostics.CodeAnalysis;
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

    public static bool CheckSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is not ClassDeclarationSyntax classDeclarationSyntax)
        {
            return false;
        }

        var attributeList = classDeclarationSyntax.AttributeLists
            .FirstOrDefault(x => x.Attributes.Any(AttributeArgumentListParser.IsMatch));
        if (attributeList is null)
        {
            Console.WriteLine("ClassDeclarationSyntax should have the correct attribute.");
            return false;
        }

        if (!TryGetClassModifier(classDeclarationSyntax, out _))
        {
            Console.WriteLine("Class modifier should be 'public' or 'internal'.");
            return false;
        }

        return true;
    }

    public static FluentData HandleSyntaxNode(SyntaxNode syntaxNode, SemanticModel semanticModel)
    {
        if (TryGet((ClassDeclarationSyntax) syntaxNode, semanticModel, out var data))
        {
            return data;
        }

        throw new InvalidOperationException();
    }

    private static bool TryGet(ClassDeclarationSyntax classDeclarationSyntax, SemanticModel semanticModel, out FluentData data)
    {
        data = default;
        
        var attributeList = classDeclarationSyntax.AttributeLists
            .FirstOrDefault(x => x.Attributes.Any(AttributeArgumentListParser.IsMatch))!;
        
        if (!TryGetClassModifier(classDeclarationSyntax, out var classModifier))
        {
            return false;
        }

        var usings = new List<string>();

        var ns = classDeclarationSyntax.GetNamespace();
        if (!string.IsNullOrEmpty(ns))
        {
            usings.Add(ns);
        }

        if (classDeclarationSyntax.TryGetParentSyntax(out CompilationUnitSyntax? cc))
        {
            usings.AddRange(cc.Usings.Select(@using => @using.Name!.ToString()));
        }

        // https://github.com/StefH/FluentBuilder/issues/36
        usings.AddRange(classDeclarationSyntax.GetAncestorsUsings().Select(@using => @using.Name!.ToString()));

        usings = usings.Distinct().ToList();

        var fluentBuilderAttributeArguments = AttributeArgumentListParser.Parse(attributeList.Attributes.FirstOrDefault(), semanticModel);

        if (fluentBuilderAttributeArguments.RawTypeName != null) // The class which needs to be processed by the CustomBuilder is provided as type
        {
            if (!AreBuilderClassModifiersValid(classDeclarationSyntax))
            {
                Console.WriteLine("Custom builder class should be 'partial' and 'public' or 'internal'.");
                return false;
            }

            data = new FluentData
            {
                Namespace = ns,
                ClassModifier = classModifier,
                ShortBuilderClassName = $"{classDeclarationSyntax.Identifier}",
                FullBuilderClassName = CreateFullBuilderClassName(ns, classDeclarationSyntax),
                FullRawTypeName = fluentBuilderAttributeArguments.RawTypeName,
                ShortTypeName = ConvertTypeName(fluentBuilderAttributeArguments.RawTypeName).Split('.').Last(),
                MetadataName = ConvertTypeName(fluentBuilderAttributeArguments.RawTypeName),
                Usings = usings,
                HandleBaseClasses = fluentBuilderAttributeArguments.HandleBaseClasses,
                Accessibility = fluentBuilderAttributeArguments.Accessibility,
                BuilderType = BuilderType.Custom,
                Methods = fluentBuilderAttributeArguments.Methods
            };

            return true;
        }

        var fullType = GetFullType(ns, classDeclarationSyntax, false); // FluentBuilderGeneratorTests.DTO.UserT<T>
        var fullBuilderType = GetFullType(ns, classDeclarationSyntax, true);
        var metadataName = classDeclarationSyntax.GetMetadataName();

        data = new FluentData
        {
            Namespace = ns,
            ClassModifier = classModifier,
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

        return true;
    }

    private static bool AreBuilderClassModifiersValid(MemberDeclarationSyntax classDeclarationSyntax)
    {
        var modifiers = classDeclarationSyntax.Modifiers.Select(m => m.ToString()).Distinct().ToArray();
        return modifiers.Contains(ModifierPartial) && (modifiers.Contains(ModifierPublic) || modifiers.Contains(ModifierInternal));
    }

    private static bool TryGetClassModifier(MemberDeclarationSyntax classDeclarationSyntax, [NotNullWhen(true)] out string? modifier)
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

        modifier = default;
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