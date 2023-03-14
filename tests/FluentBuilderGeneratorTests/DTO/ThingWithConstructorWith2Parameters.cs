namespace FluentBuilderGeneratorTests.DTO;

public class ThingUsingConstructorWith2Parameters
{
    public int X { get; }

    public ThingUsingConstructorWith2Parameters(int x, int y)
    {
        X = x * y;
    }
}