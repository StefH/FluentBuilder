using FluentBuilder;

namespace FluentBuilderGeneratorTests.DTO;

public class ClassWithPrivateBuilderClass
{
    [AutoGenerateBuilder(FluentBuilderAccessibility.PublicAndPrivate)]
    private class PrivateClass
    {
        public int Test { get; set; }
    }
}