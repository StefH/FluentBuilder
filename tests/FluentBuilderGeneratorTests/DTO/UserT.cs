namespace FluentBuilderGeneratorTests.DTO
{
    public class UserT<T> where T : struct
    {
        public T TValue { get; set; }
    }
}