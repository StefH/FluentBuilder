﻿using System;
using System.Collections.Generic;

namespace BuilderConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var email = new FluentBuilder.EmailDtoBuilder()
                .WithValue("x@x.nl")
                .Build();

            var user = new FluentBuilder.UserDtoBuilder()
                .WithAge(99)
                .WithFirstName("Stef")
                .WithLastName("Heyenrath")
                .WithPrimaryEmail(email)
                .Build();

            Console.WriteLine($"{user.FirstName} {user.LastName} {user.PrimaryEmail.Value}");
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
    }

    [FluentBuilder.AutoGenerateBuilder]
    public class EmailDto
    {
        public string Value { get; set; }
    }
}