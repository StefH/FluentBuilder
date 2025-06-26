// This source code is partly based on https://justsimplycode.com/2020/12/06/auto-generate-builders-using-source-generator-in-net-5

using FluentBuilderGenerator.Extensions;
using FluentBuilderGenerator.Models;
using FluentBuilderGenerator.Types;

namespace FluentBuilderGenerator.FileGenerators;

internal class ExtraFilesGenerator : IFileGenerator
{
    private const string Name = "FluentBuilder.Extra.g.cs";

    private readonly bool _supportsNullable;
    private readonly bool _supportsGenericAttributes;
    
    public ExtraFilesGenerator(bool supportsNullable, bool supportsGenericAttributes)
    {
        _supportsNullable = supportsNullable;
        _supportsGenericAttributes = supportsGenericAttributes;
    }

    public FileData GenerateFile()
    {
        var autoGenerateBuilderAttribute = $@"[AttributeUsage(AttributeTargets.Class)]
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
    }}";

        var autoGenerateBuilderAttributeGeneric = $@"[AttributeUsage(AttributeTargets.Class)]
    internal sealed class AutoGenerateBuilderAttribute<T> : Attribute where T : class
    {{
        public Type Type {{ get; }}
        public bool HandleBaseClasses {{ get; }}
        public FluentBuilderAccessibility Accessibility {{ get; }}
        public FluentBuilderMethods Methods {{ get; }}

        public AutoGenerateBuilderAttribute() : this(true, FluentBuilderAccessibility.Public, FluentBuilderMethods.WithOnly)
        {{
        }}

        public AutoGenerateBuilderAttribute(bool handleBaseClasses) : this(handleBaseClasses, FluentBuilderAccessibility.Public, FluentBuilderMethods.WithOnly)
        {{
        }}

        public AutoGenerateBuilderAttribute(FluentBuilderAccessibility accessibility) : this(true, accessibility, FluentBuilderMethods.WithOnly)
        {{
        }}

        public AutoGenerateBuilderAttribute(bool handleBaseClasses, FluentBuilderAccessibility accessibility) : this(handleBaseClasses, accessibility, FluentBuilderMethods.WithOnly)
        {{
        }}

        public AutoGenerateBuilderAttribute(bool handleBaseClasses, FluentBuilderMethods methods) : this(handleBaseClasses, FluentBuilderAccessibility.Public, methods)
        {{
        }}

        public AutoGenerateBuilderAttribute(bool handleBaseClasses, FluentBuilderAccessibility accessibility, FluentBuilderMethods methods)
        {{
            Type = typeof(T);
            HandleBaseClasses = handleBaseClasses;
            Accessibility = accessibility;
            Methods = methods;
        }}
    }}";

        return new FileData
        (
            FileDataType.Attribute,
            Name,
            $@"{Header.Text}

{_supportsNullable.IIf("#nullable enable")}
using System;

namespace FluentBuilder
{{
    {autoGenerateBuilderAttribute}

    {_supportsGenericAttributes.IIf(autoGenerateBuilderAttributeGeneric)}

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