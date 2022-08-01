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
        // public FluentBuilderAccessibility Accessibility {{ get; }}

        public AutoGenerateBuilderAttribute() : this(null, true)
        {{
        }}

        public AutoGenerateBuilderAttribute(bool handleBaseClasses) : this(null, handleBaseClasses)
        {{
        }}

        public AutoGenerateBuilderAttribute(Type{(_supportsNullable ? "?" : string.Empty)} type) : this(type, true)
        {{
        }}

        public AutoGenerateBuilderAttribute(Type{(_supportsNullable ? "?" : string.Empty)} type, bool handleBaseClasses)
        {{
            Type = type;
            HandleBaseClasses = handleBaseClasses;
            // Accessibility = accessibility;
        }}
    }}

    [AttributeUsage(AttributeTargets.Property)]
    internal sealed class FluentBuilderIgnoreAttribute : Attribute
    {{
    }}

    // Based on Microsoft.CodeAnalysis.Accessibility
    /*[Flags]
    internal enum FluentBuilderAccessibility
    {{
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
    }}*/
}}
{(_supportsNullable ? "#nullable disable" : string.Empty)}"
        );
    }
}