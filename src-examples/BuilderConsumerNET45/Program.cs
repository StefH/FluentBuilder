using System;
using System.Collections.Generic;
using FluentBuilder;
using Newtonsoft.Json;

namespace BuilderConsumerNET45
{
    class Program
    {
        static void Main(string[] args)
        {
            var t0 = new ThingWithOnlyParameterizedConstructorsBuilder()
                .Build();
            Console.WriteLine("t0 = " + JsonConvert.SerializeObject(t0));

            var t1 = new ThingWithOnlyParameterizedConstructorsBuilder()
                .WithConstructor(1,2,"xxx")
                .Build();
            Console.WriteLine("t1 = " + JsonConvert.SerializeObject(t1));

            var email = new EmailDtoBuilder()
                .WithValue("x@x.nl")
                .Build();

            var user = new UserDtoBuilder()
                        .WithFirstName("Stef")
                        .WithLastName("Heyenrath")
                        .WithPrimaryEmail(email)
                        .Build();

            Console.WriteLine($"{user.FirstName} {user.LastName} {user.PrimaryEmail.Value}");
        }
    }

    [AutoGenerateBuilder]
    public class UserDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public EmailDto PrimaryEmail { get; set; }

        public IEnumerable<EmailDto> SecondaryEmails { get; set; }

        public DateTime? QuitDate { get; set; }
    }

    [AutoGenerateBuilder]
    public class EmailDto
    {
        public string Value { get; set; }
    }
}