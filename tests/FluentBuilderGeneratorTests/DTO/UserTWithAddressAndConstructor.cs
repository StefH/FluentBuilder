namespace FluentBuilderGeneratorTests.DTO
{
    public class UserTWithAddressAndConstructor<T> where T : struct
    {
        public T TValue { get; set; }

        public Address<short> Address { get; set; }

        public UserTWithAddressAndConstructor()
        {
            Address = new Address<short>();
        }
    }
}