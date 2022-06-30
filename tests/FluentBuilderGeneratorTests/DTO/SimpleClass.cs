using FluentBuilder;
using System;

namespace FluentBuilderGeneratorTests.DTO
{
    public class SimpleClass
    {
        public int Id { get; set; }
        [IgnoreProperty] //this should be ignored or it will fail.
        public System.Globalization.CultureInfo? CultureInfo { get; set; }
    }
}
namespace FluentBuilder
{
    [System.AttributeUsage(AttributeTargets.Property)]
    public class IgnorePropertyAttribute : Attribute
    {
    }
}




