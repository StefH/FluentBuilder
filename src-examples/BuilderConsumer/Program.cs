using System;
using System.Collections.Generic;
using System.Text.Json;

namespace BuilderConsumer
{
    class Program
    {
        private static readonly JsonSerializerOptions JsonSerializerOptions = new()
        {
            WriteIndented = true
        };

        static void Main(string[] args)
        {
            var email = new FluentBuilder.EmailDtoBuilder()
                .WithAddress("x@x.nl")
                .Build();
            Console.WriteLine("email = " + JsonSerializer.Serialize(email, JsonSerializerOptions));

            var email2a = new FluentBuilder.EmailDtoWithConstructorBuilder()
                .WithAddress("x@x.nl")
                .Build();
            Console.WriteLine("email2a = " + JsonSerializer.Serialize(email2a, JsonSerializerOptions));

            var email2b = new FluentBuilder.EmailDtoWithConstructorBuilder()
                .WithAddress("x@x.nl")
                .Build(false);
            Console.WriteLine("email2b = " + JsonSerializer.Serialize(email2b, JsonSerializerOptions));

            var email2c = new FluentBuilder.EmailDtoWithConstructorBuilder()
                .Build(false);
            Console.WriteLine("email2c = " + JsonSerializer.Serialize(email2c, JsonSerializerOptions));

            var user1 = new FluentBuilder.UserDtoBuilder()
                .WithAge(99)
                .WithFirstName("Stef")
                .WithLastName("Heyenrath")
                .WithPrimaryEmail(email)
                .Build();
            Console.WriteLine(JsonSerializer.Serialize(user1, JsonSerializerOptions));

            var user2 = new FluentBuilder.UserDtoBuilder()
                .WithAge(100)
                .WithFirstName("User")
                .WithLastName("Two")
                .WithPrimaryEmail(e => e.WithAddress("abc").WithPrimary(true))
                .WithUserDtoT(x => x.WithTValue(5))
                .Build();
            Console.WriteLine(JsonSerializer.Serialize(user2, JsonSerializerOptions));

            var userT1 = new FluentBuilder.UserDtoTBuilder<int>()
                .WithTValue(42)
                .Build();
            Console.WriteLine(JsonSerializer.Serialize(userT1, JsonSerializerOptions));

            var userWithAddress = new FluentBuilder.UserWithEmailDtoBuilder()
                .WithAge(99)
                .WithEmailWithConstructor(a => a.WithPrimary(false), false)
                .Build();
            Console.WriteLine(JsonSerializer.Serialize(userWithAddress, JsonSerializerOptions));
        }
    }

    [FluentBuilder.AutoGenerateBuilder]
    public class UserDto
    {
        public int Age { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public EmailDto PrimaryEmail { get; set; }

        public IEnumerable<EmailDto> SecondaryEmails { get; set; }

        public DateTime? QuitDate { get; set; }

        public TestDto? Test { get; set; }

        public UserDtoT<long> UserDtoT { get; set; }
    }

    [FluentBuilder.AutoGenerateBuilder]
    public class UserDtoT<T> where T : struct
    {
        public T TValue { get; set; }
    }

    [FluentBuilder.AutoGenerateBuilder]
    public class UserDtoTT<T1, T2>
        where T1 : struct
        where T2 : class, new()
    {
        public T1 T1Value { get; set; }

        public T2 T2Value { get; set; }
    }

    [FluentBuilder.AutoGenerateBuilder]
    public class EmailDto
    {
        public string Address { get; set; }

        public bool Primary { get; set; }
    }

    [FluentBuilder.AutoGenerateBuilder]
    public class EmailDtoWithConstructor
    {
        public string Address { get; set; }

        public bool Primary { get; set; }

        public EmailDtoWithConstructor()
        {
            Address = "initial value";
            Primary = true;
        }
    }

    [FluentBuilder.AutoGenerateBuilder]
    public class UserWithEmailDto
    {
        public int Age { get; set; }

        public EmailDto Email { get; set; }

        public EmailDtoWithConstructor EmailWithConstructor { get; set; }
    }

    public class TestDto
    {
        public string X { get; set; }
    }

    //[FluentBuilder.AutoGenerateBuilder]
    //public class Error
    //{
    //    public string X { get; set; }

    //    public Error(int x)
    //    {
    //    }
    //}
}