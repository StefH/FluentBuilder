using System;

namespace FluentBuilderGeneratorTests.DTO;

public class ThingWithParameterizedConstructor
{
    public int X { get; set; }

    public ThingWithParameterizedConstructor()
    {
    }

    public ThingWithParameterizedConstructor(int x)
    {
        X = x;
    }
}
