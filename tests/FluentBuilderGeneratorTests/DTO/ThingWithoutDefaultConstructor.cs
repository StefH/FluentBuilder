namespace FluentBuilderGeneratorTests.DTO;

public class ThingWithoutDefaultConstructor
{
    public int X { get; }

    public ThingWithoutDefaultConstructor(int x)
    {
        X = x;
    }

    public ThingWithoutDefaultConstructor(int x, int y)
    {
        X = x * y;
    }
}