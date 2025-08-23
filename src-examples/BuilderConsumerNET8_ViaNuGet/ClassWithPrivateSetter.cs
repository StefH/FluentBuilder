using FluentBuilder;

namespace BuilderConsumerNET8
{
    [AutoGenerateBuilder(FluentBuilderAccessibility.Public)]
    public class ClassWithPrivateSetter
    {
        public int Value1 { get; private set; }

        public int Value2 { get; set; }
    }
}