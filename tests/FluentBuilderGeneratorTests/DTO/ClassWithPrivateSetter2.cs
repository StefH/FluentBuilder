using System.Collections.Generic;

namespace FluentBuilderGeneratorTests.DTO
{
    public class ClassWithPrivateSetter2
    {
        public int Value1 { get; private set; }

        public int Value2 { get; set; }

        public List<int> NormalList { get; set; } = new();

        public List<long> GetOnlyList { get; } = new();
    }
}