using System.Collections.Generic;

namespace ConsumerClassLibrary
{
    [FluentBuilder.AutoGenerateBuilder]
    public class TestClass
    {
        public int Id { get; set; }

        public IEnumerable<string> Values { get; set; }
    }
}