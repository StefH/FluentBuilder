// This source code is partly based on https://justsimplycode.com/2020/12/06/auto-generate-builders-using-source-generator-in-net-5

using System.Text;
using FluentBuilderGenerator.Extensions;
using FluentBuilderGenerator.Models;
using FluentBuilderGenerator.Types;

namespace FluentBuilderGenerator.FileGenerators;

internal class ExtraFilesGenerator : IFileGenerator
{
    private const string Name = "FluentBuilder.Extra.g.cs";

    private readonly string _assemblyName;
    private readonly bool _supportsNullable;

    public ExtraFilesGenerator(string assemblyName, bool supportsNullable)
    {
        _assemblyName = assemblyName;
        _supportsNullable = supportsNullable;
    }

    public FileData GenerateFile()
    {
        //var arguments = new[] { "bool handleBaseClasses", "FluentBuilderAccessibility accessibility", "FluentBuilderMethods methods" };
        //var defaultValues = new int[] { "true", "FluentBuilderAccessibility.Public", "FluentBuilderMethods.WithOnly" };

        //var constructors = new StringBuilder();
        //foreach (var argument in arguments)
        //{
        //    constructors.AppendLine(8, "public AutoGenerateBuilderAttribute(");
        //}

        return new FileData
        (
            FileDataType.Attribute,
            Name,
            $@"{Header.Text}

{_supportsNullable.IIf("#nullable enable")}
using System;

namespace FluentBuilder
{{
    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class AutoGenerateBuilderAttribute : Attribute
    {{
        public Type{_supportsNullable.IIf("?")} Type {{ get; }}
        public bool HandleBaseClasses {{ get; }}
        public FluentBuilderAccessibility Accessibility {{ get; }}
        public FluentBuilderMethods Methods {{ get; }}

        public AutoGenerateBuilderAttribute() : this(null, true, FluentBuilderAccessibility.Public, FluentBuilderMethods.WithOnly)
        {{
        }}

        public AutoGenerateBuilderAttribute(bool handleBaseClasses) : this(null, handleBaseClasses, FluentBuilderAccessibility.Public, FluentBuilderMethods.WithOnly)
        {{
        }}

        public AutoGenerateBuilderAttribute(FluentBuilderAccessibility accessibility) : this(null, true, accessibility, FluentBuilderMethods.WithOnly)
        {{
        }}

        public AutoGenerateBuilderAttribute(FluentBuilderMethods methods) : this(null, true, FluentBuilderAccessibility.Public, methods)
        {{
        }}

        public AutoGenerateBuilderAttribute(bool handleBaseClasses, FluentBuilderAccessibility accessibility) : this(null, handleBaseClasses, accessibility, FluentBuilderMethods.WithOnly)
        {{
        }}

        public AutoGenerateBuilderAttribute(bool handleBaseClasses, FluentBuilderMethods methods) : this(null, handleBaseClasses, FluentBuilderAccessibility.Public, methods)
        {{
        }}

        public AutoGenerateBuilderAttribute(Type{_supportsNullable.IIf("?")} type) : this(type, true, FluentBuilderAccessibility.Public, FluentBuilderMethods.WithOnly)
        {{
        }}

        public AutoGenerateBuilderAttribute(Type{_supportsNullable.IIf("?")} type, bool handleBaseClasses) : this(type, handleBaseClasses, FluentBuilderAccessibility.Public, FluentBuilderMethods.WithOnly)
        {{
        }}

        public AutoGenerateBuilderAttribute(Type{_supportsNullable.IIf("?")} type, FluentBuilderAccessibility accessibility) : this(type, true, accessibility, FluentBuilderMethods.WithOnly)
        {{
        }}

        public AutoGenerateBuilderAttribute(Type{_supportsNullable.IIf("?")} type, bool handleBaseClasses, FluentBuilderAccessibility accessibility) : this(type, handleBaseClasses, accessibility, FluentBuilderMethods.WithOnly)
        {{
        }}

        public AutoGenerateBuilderAttribute(Type{_supportsNullable.IIf("?")} type, bool handleBaseClasses, FluentBuilderMethods methods) : this(type, handleBaseClasses, FluentBuilderAccessibility.Public, methods)
        {{
        }}

        public AutoGenerateBuilderAttribute(Type{_supportsNullable.IIf("?")} type, bool handleBaseClasses, FluentBuilderAccessibility accessibility, FluentBuilderMethods methods)
        {{
            Type = type;
            HandleBaseClasses = handleBaseClasses;
            Accessibility = accessibility;
            Methods = methods;
        }}
    }}

    [AttributeUsage(AttributeTargets.Property)]
    internal sealed class FluentBuilderIgnoreAttribute : Attribute
    {{
    }}

    [Flags]
    internal enum FluentBuilderAccessibility
    {{
        Public = 0,
        PublicAndPrivate = 1
    }}

    [Flags]
    internal enum FluentBuilderMethods
    {{
        WithOnly = 0,
        WithAndWithout = 1
    }}
}}
{_supportsNullable.IIf("#nullable disable")}"
        );
    }
}