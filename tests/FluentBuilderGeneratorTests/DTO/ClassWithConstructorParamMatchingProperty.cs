namespace FluentBuilderGeneratorTests.DTO;

public enum StageType
{
    Build,
    Test
}

public class ClassWithConstructorParamMatchingProperty
{
    public ClassWithConstructorParamMatchingProperty(StageType type)
    {
        Type = type;
    }

    public StageType Type { get; set; }

    public string Name { get; set; } = string.Empty;
}
