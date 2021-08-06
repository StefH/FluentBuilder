namespace FluentBuilderGeneratorTests.DTO
{
    public class Address<T> where T : struct
    {
        public T Street { get; set; }
    }
}