using System;
using FluentBuilder;

namespace BuilderConsumerNET6
{
    [AutoGenerateBuilder]
    public class User
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? Date { get; set; }
    }
}