// This source code is partly based on https://justsimplycode.com/2020/12/06/auto-generate-builders-using-source-generator-in-net-5

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
        return new FileData
        (
            FileDataType.Attribute,
            Name,
            $@"{Header.Text}

{(_supportsNullable ? "#nullable enable" : string.Empty)}
using System;

namespace FluentBuilder
{{
    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class AutoGenerateBuilderAttribute : Attribute
    {{
        public Type{(_supportsNullable ? "?" : string.Empty)} Type {{ get; }}
        public bool HandleBaseClasses {{ get; }}
        public FluentBuilderAccessibility Accessibility {{ get; }}

        public AutoGenerateBuilderAttribute() : this(null, true, FluentBuilderAccessibility.Public)
        {{
        }}

        public AutoGenerateBuilderAttribute(bool handleBaseClasses) : this(null, handleBaseClasses, FluentBuilderAccessibility.Public)
        {{
        }}

        public AutoGenerateBuilderAttribute(FluentBuilderAccessibility accessibility) : this(null, true, accessibility)
        {{
        }}

        public AutoGenerateBuilderAttribute(bool handleBaseClasses, FluentBuilderAccessibility accessibility) : this(null, handleBaseClasses, accessibility)
        {{
        }}

        public AutoGenerateBuilderAttribute(Type{(_supportsNullable ? "?" : string.Empty)} type) : this(type, true, FluentBuilderAccessibility.Public)
        {{
        }}

        public AutoGenerateBuilderAttribute(Type{(_supportsNullable ? "?" : string.Empty)} type, FluentBuilderAccessibility accessibility) : this(type, true, accessibility)
        {{
        }}

        public AutoGenerateBuilderAttribute(Type{(_supportsNullable ? "?" : string.Empty)} type, bool handleBaseClasses, FluentBuilderAccessibility accessibility = FluentBuilderAccessibility.Public)
        {{
            Type = type;
            HandleBaseClasses = handleBaseClasses;
            Accessibility = accessibility;
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
}}
{(_supportsNullable ? "#nullable disable" : string.Empty)}"
        );
    }
}