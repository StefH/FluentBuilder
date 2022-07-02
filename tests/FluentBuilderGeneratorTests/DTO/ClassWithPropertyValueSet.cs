using System.Globalization;
using MyNamespace;

namespace FluentBuilderGeneratorTests.DTO
{
    public class ClassWithPropertyValueSet
    {
        public int NoIntValueSet { get; set; }

        public int IntValueSet1 { get; set; } = 1 + 1;

        public int IntValueSet2 { get; set; } = 2;

        public CultureInfo Locale { get; set; } = CultureInfo.CurrentCulture;

        public CultureInfo Locale2 { get; set; } = System.Globalization.CultureInfo.CurrentCulture;

        public CultureInfo Locale3 { get; set; } = X.Value;

        // public CultureInfo Locale4 { get; set; } = Y.Value;
    }

    public static class X
    {
        public static CultureInfo Value = CultureInfo.CurrentCulture;
    }
}

namespace MyNamespace
{
    public static class Y
    {
        public static CultureInfo Value = CultureInfo.CurrentCulture;
    }
}