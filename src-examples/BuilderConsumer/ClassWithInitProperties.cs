using FluentBuilder;

namespace BuilderConsumer
{
    [AutoGenerateBuilder]
    public class ClassWithInitProperties
    {
        public string Normal { get; set; }

        public int SiteId { get; init; }

        public string ProductName { get; init; }

        public string PrivateProductName { get; private init; }

        public required string RequiredTest { get; set; }

        public required string RequiredTestInit { get; init; }

        public required ClassWithInitProperties2 X { get; init; }
    }

    public class ClassWithInitProperties2
    {
        public required string X { get; set; }
    }
}