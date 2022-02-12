namespace FluentBuilderGeneratorTests.DTO
{
    public class AddressTT<T1, T2>
        where T1 : struct
        where T2 : class, new()
    {
        public T1 TestValue1 { get; set; }
        public T2? TestValue2 { get; set; }
    }
}