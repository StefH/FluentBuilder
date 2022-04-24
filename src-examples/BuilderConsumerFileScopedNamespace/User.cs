using FluentBuilder;

namespace BuilderConsumerFileScopedNamespace;

[AutoGenerateBuilder]
public class User
{
    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public System.DateTime? Date { get; set; }
}