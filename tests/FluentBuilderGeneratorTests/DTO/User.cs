using System;
using System.Collections.Generic;

namespace FluentBuilderGeneratorTests.DTO
{
    public class User
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? QuitDate { get; set; }

        public DummyClass TestDummyClass { get; set; }

        public List<Option> Options { get; set; }
    }
}