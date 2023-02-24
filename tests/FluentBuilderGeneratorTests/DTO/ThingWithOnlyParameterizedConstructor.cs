namespace FluentBuilderGeneratorTests.DTO;

public class ThingWithOnlyParameterizedConstructor
{
    public int X { get; set; }

    public string Y { get; set; }

    public ThingWithOnlyParameterizedConstructor(int x, string y)
    {
        X = x;
        Y = y;
    }
}