using System.Collections;
using System.Collections.Generic;

namespace FluentBuilderGeneratorTests.DTO
{
    // [FluentBuilder.AutoGenerateBuilder]
    public class Address
    {
        public int HouseNumber { get; set; }

        public string City { get; set; }

        public string[] Array { get; set; }

        public List<string> List { get; set; }

        public IList<Address> IListAddress { get; set; }

        public IDictionary Dictionary { get; set; }

        public IDictionary<string, int> Dictionary2 { get; set; }
    }
}