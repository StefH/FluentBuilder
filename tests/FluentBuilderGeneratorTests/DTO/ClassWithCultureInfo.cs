using System.Globalization;

namespace FluentBuilderGeneratorTests.DTO
{
    public class ClassWithCultureInfo
    {
        public int NoValueSet { get; set; }

        public CultureInfo Locale { get; set; } = CultureInfo.CurrentCulture;
    }
}