namespace FluentBuilderGeneratorTests.DTO;

public class ThingWithConstructorWith2Parameters
{
    public int X { get; }

    public ThingWithConstructorWith2Parameters(int x, int y)
    {
        X = x * y;
    }
}