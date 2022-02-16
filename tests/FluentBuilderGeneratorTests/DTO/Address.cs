using System.Collections;
using System.Collections.Generic;
using WireMock.Types;

namespace FluentBuilderGeneratorTests.DTO
{
    // [FluentBuilder.AutoGenerateBuilder]
    public class Address
    {
        public int HouseNumber { get; set; }

        public string City { get; set; }

        public string[] Array { get; set; }

        public Address[] Array2 { get; set; }

        public IEnumerable<byte> Enumerable { get; set; }

        public IEnumerable<Address> Enumerable2 { get; set; }

        public List<string> List { get; set; }

        public List<Address> List2 { get; set; }

        public ICollection<long> Collection { get; set; }

        public ICollection<Address> Collection2 { get; set; }

        public IDictionary IDictionary { get; set; }

        public IDictionary<string, int> IDictionary2 { get; set; }

        public Dictionary<string, int> Dictionary2 { get; set; }

        public Dictionary<long, WireMockList<string>> DictionaryWireMockListString { get; set; }
    }
}