using System;

namespace FluentBuilderGeneratorTests.DTO;

public class ClassWithFuncAndAction
{
    public Func<int, string> Func1 { get; set; }

    public Func<int, bool, string> Func2 { get; set; }

    public Action<int> Action { get; set; }
}