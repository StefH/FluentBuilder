using System;

namespace FluentBuilderGeneratorTests.DTO
{
    // [FluentBuilder.AutoGenerateBuilder]
    public class User
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? QuitDate { get; set; }

        // public Address Address { get; set; }
    }
}