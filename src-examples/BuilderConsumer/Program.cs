using System;
using System.Collections.Generic;
using System.Text.Json;

namespace BuilderConsumer
{
    class Program
    {
        private static JsonSerializerOptions JsonSerializerOptions = new()
        {
            WriteIndented = true
        };

        static void Main(string[] args)
        {
            var email = new FluentBuilder.EmailDtoBuilder()
                .WithAddress("x@x.nl")
                .Build();
            Console.WriteLine(JsonSerializer.Serialize(email, JsonSerializerOptions));

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

    public class TestDto
    {
        public string X { get; set; }
    }
}