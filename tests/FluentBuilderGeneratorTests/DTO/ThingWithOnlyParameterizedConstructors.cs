namespace FluentBuilderGeneratorTests.DTO;

public class ThingWithOnlyParameterizedConstructors
{
    public int X { get; }

    public int Y { get; }

    public string Z { get;  }

    public long L { get; set; }

    public ThingWithOnlyParameterizedConstructors(int x, int y, string z = "test")
    {
        X = x;
        Y = y;
        Z = z;
    }

    public ThingWithOnlyParameterizedConstructors(int x)
    {
        X = x;
        Y = -1;
        Z = "zzz";
    }
}