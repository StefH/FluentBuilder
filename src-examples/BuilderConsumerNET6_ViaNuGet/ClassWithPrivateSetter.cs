using FluentBuilder;

namespace BuilderConsumerNET6
{
    [AutoGenerateBuilder(FluentBuilderAccessibility.Public)]
    public class ClassWithPrivateSetter
    {
        public int Value1 { get; private set; }

        public int Value2 { get; set; }
    }
}