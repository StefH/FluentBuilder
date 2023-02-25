namespace FluentBuilderGeneratorTests.DTO;

public class ThingWithOnlyParameterizedConstructor
{
    public int X { get; set; }

    public int Y { get; set; }

    public string Z { get; set; }

    public ThingWithOnlyParameterizedConstructor(int x, int y, string z = "test")
    {
        X = x;
        Y = y;
        Z = z;
    }

    public ThingWithOnlyParameterizedConstructor(int x)
    {
        X = x;
        Y = -1;
        Z = "zzz";
    }
}