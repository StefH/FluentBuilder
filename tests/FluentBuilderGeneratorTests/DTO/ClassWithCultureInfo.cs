using System.Globalization;
using MyNamespace;

namespace FluentBuilderGeneratorTests.DTO
{
    public class ClassWithCultureInfo
    {
        public int NoValueSet { get; set; }

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