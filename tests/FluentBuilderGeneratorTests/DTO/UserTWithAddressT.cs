namespace FluentBuilderGeneratorTests.DTO
{
    public class UserTWithAddressT<T> where T : struct
    {
        public T TValue { get; set; }

        public Address<short> Address { get; set; }
    }
}