using FluentBuilder;

namespace BuilderConsumerNET8;

[AutoGenerateBuilder]
public class ClassWithPrimaryConstructor(string test, int num)
{
    public string Normal { get; set; } = string.Empty;

    public string Data { get; } = test + num;
}