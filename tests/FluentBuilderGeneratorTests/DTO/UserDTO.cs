using System;
using System.Collections.Generic;

namespace FluentBuilderGeneratorTests.DTO
{
    // [FluentBuilder.AutoGenerateBuilder]
    public class UserDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? QuitDate { get; set; }
    }
}
