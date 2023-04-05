//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by https://github.com/StefH/FluentBuilder version 0.9.0.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable enable
using System;

namespace FluentBuilder
{
    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class AutoGenerateBuilderAttribute : Attribute
    {
        public Type? Type { get; }
        public bool HandleBaseClasses { get; }
        public FluentBuilderAccessibility Accessibility { get; }
        public FluentBuilderMethods Methods { get; }

        public AutoGenerateBuilderAttribute() : this(null, true, FluentBuilderAccessibility.Public, FluentBuilderMethods.WithOnly)
        {
        }

        public AutoGenerateBuilderAttribute(bool handleBaseClasses) : this(null, handleBaseClasses, FluentBuilderAccessibility.Public, FluentBuilderMethods.WithOnly)
        {
        }

        public AutoGenerateBuilderAttribute(FluentBuilderAccessibility accessibility) : this(null, true, accessibility, FluentBuilderMethods.WithOnly)
        {
        }

        public AutoGenerateBuilderAttribute(FluentBuilderMethods methods) : this(null, true, FluentBuilderAccessibility.Public, methods)
        {
        }

        public AutoGenerateBuilderAttribute(bool handleBaseClasses, FluentBuilderAccessibility accessibility) : this(null, handleBaseClasses, accessibility, FluentBuilderMethods.WithOnly)
        {
        }

        public AutoGenerateBuilderAttribute(bool handleBaseClasses, FluentBuilderMethods methods) : this(null, handleBaseClasses, FluentBuilderAccessibility.Public, methods)
        {
        }

        public AutoGenerateBuilderAttribute(Type? type) : this(type, true, FluentBuilderAccessibility.Public, FluentBuilderMethods.WithOnly)
        {
        }

        public AutoGenerateBuilderAttribute(Type? type, bool handleBaseClasses) : this(type, handleBaseClasses, FluentBuilderAccessibility.Public, FluentBuilderMethods.WithOnly)
        {
        }

        public AutoGenerateBuilderAttribute(Type? type, FluentBuilderAccessibility accessibility) : this(type, true, accessibility, FluentBuilderMethods.WithOnly)
        {
        }

        public AutoGenerateBuilderAttribute(Type? type, bool handleBaseClasses, FluentBuilderAccessibility accessibility) : this(type, handleBaseClasses, accessibility, FluentBuilderMethods.WithOnly)
        {
        }

        public AutoGenerateBuilderAttribute(Type? type, bool handleBaseClasses, FluentBuilderMethods methods) : this(type, handleBaseClasses, FluentBuilderAccessibility.Public, methods)
        {
        }

        public AutoGenerateBuilderAttribute(Type? type, bool handleBaseClasses, FluentBuilderAccessibility accessibility, FluentBuilderMethods methods)
        {
            Type = type;
            HandleBaseClasses = handleBaseClasses;
            Accessibility = accessibility;
            Methods = methods;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    internal sealed class FluentBuilderIgnoreAttribute : Attribute
    {
    }

    [Flags]
    internal enum FluentBuilderAccessibility
    {
        Public = 0,
        PublicAndPrivate = 1
    }

    [Flags]
    internal enum FluentBuilderMethods
    {
        WithOnly = 0,
        WithAndWithout = 1
    }
}
#nullable disable