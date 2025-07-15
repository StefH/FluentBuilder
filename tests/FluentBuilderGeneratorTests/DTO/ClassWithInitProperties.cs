namespace FluentBuilderGeneratorTests.DTO
{
    public class ClassWithInitProperties
    {
        public string Normal { get; set; }

        public int SiteId { get; init; }

        public string ProductName { get; init; }

        public string PrivateProductName { get; private init; }

        public required string RequiredTest { get; set; }

        public required string RequiredTestInit { get; init; }
    }
}