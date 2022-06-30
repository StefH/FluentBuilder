using System;
using System.Collections.Generic;
using System.Text;

namespace FluentBuilderGenerator.Models
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnorePropertyAttribute : Attribute
    {
    }
}
