using FluentBuilder;

namespace BuilderConsumerNET6;

[AutoGenerateBuilder(false)]
public class UserNotHandleBaseClass : BaseClass
{
    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public DateTime? Date { get; set; }

    public int Int { get; set; }

    public char Char { get; set; }

    public int? NullableInt { get; set; }

    public char? NullableChar { get; set; }
}