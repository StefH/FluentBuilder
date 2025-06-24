using FluentBuilder;

namespace BuilderConsumerNET6
{
    [AutoGenerateBuilder(FluentBuilderAccessibility.Public)]
    public class ClassWithPrivateSetter
    {
        public int Value1 { get; private set; }

        public int Value2 { get; set; }

        [AutoGenerateBuilder(FluentBuilderAccessibility.PublicAndPrivate)]
        private class PrivateClass
        {
            public int Test { get; set; }
        }
    }
}