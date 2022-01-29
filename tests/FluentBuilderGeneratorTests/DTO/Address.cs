using System.Collections;
using System.Collections.Generic;

namespace FluentBuilderGeneratorTests.DTO
{
    // [FluentBuilder.AutoGenerateBuilder]
    public class Address
    {
        public int HouseNumber { get; set; }

        public string City { get; set; }

        //public string[] Array { get; set; }
        //public List<int>[] Matrix { get; set; }

        //public List<Address> ListAddress { get; set; }

        //public IEnumerable<string> IEnumerableString { get; set; }

        //public IEnumerable IEnumerable { get; set; }

        //public IList<string> IList { get; set; }

        public IList<Address> IListAddress { get; set; }
    }
}