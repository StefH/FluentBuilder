using System;
using System.Collections.Generic;

namespace BuilderConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var email = new FluentBuilder.EmailDtoBuilder()
                .WithAddress("x@x.nl")
                .Build();

            var user1 = new FluentBuilder.UserDtoBuilder()
                .WithAge(99)
                .WithFirstName("Stef")
                .WithLastName("Heyenrath")
                .WithPrimaryEmail(email)
                .Build();

            Console.WriteLine($"user1 : {user1.FirstName} {user1.LastName} {user1.PrimaryEmail.Address} {user1.PrimaryEmail.Primary}");

            var user2 = new FluentBuilder.UserDtoBuilder()
                .WithAge(100)
                .WithFirstName("User")
                .WithLastName("Two")
                .WithPrimaryEmail((e) => e.WithAddress("abc").WithPrimary(true))
                .Build();

            Console.WriteLine($"user2 : {user2.FirstName} {user2.LastName} {user2.PrimaryEmail.Address} {user2.PrimaryEmail.Primary}");
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