namespace namespace1
{
    using FluentBuilder;

    [AutoGenerateBuilder(typeof(MyModel))]
    public partial class MyModelBuilder
    {
    }

    public record MyModel
    {
        public string Name { get; set; } = "Unknown";
    }
}

namespace namespace2
{
    using FluentBuilder;
    using namespace1;

    [AutoGenerateBuilder(typeof(MyModel))]
    public partial class MyModelBuilder2
    {
    }
}