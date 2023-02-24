namespace FluentBuilderGeneratorTests.DTO;

public class ThingWithOnlyParameterizedConstructor
{
    public int X { get; set; }

    public string Y { get; set; }

    public string Z { get; set; }

    public ThingWithOnlyParameterizedConstructor(int x, string y, string z = "test")
    {
        X = x;
        Y = y;
        Z = z;
    }
}