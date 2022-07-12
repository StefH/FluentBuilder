using FluentBuilder;

namespace FluentBuilderGeneratorTests.DTO;

public class ClassWithFluentBuilderIgnore
{
    public int Id { get; set; }

    [FluentBuilderIgnore]
    public long IgnoreThis{ get; set; }
}