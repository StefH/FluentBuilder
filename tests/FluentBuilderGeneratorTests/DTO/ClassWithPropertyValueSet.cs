using System.Globalization;
using MyNamespace;
using MyNamespace2;

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

        public CultureInfo Locale4 { get; set; } = Y.Value;

        public CultureInfo Locale5 { get; set; } = Z.Abc;

        public string SuppressNullableWarningExpression { get; set; } = null!;

        public string StringNull { get; set; } = null;

        public string StringEmpty { get; set; } = string.Empty;
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

namespace MyNamespace2
{
    public static class Z
    {
        public static CultureInfo Abc = MyNamespace.Y.Value;
    }
}