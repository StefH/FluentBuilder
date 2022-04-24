using System.Collections.Generic;
using FluentBuilder;

namespace ConsumerClassLibrary
{
    [AutoGenerateBuilder]
    public class TestClass
    {
        public int Id { get; set; }

        public List<string> Values { get; set; } = null!;
    }
}