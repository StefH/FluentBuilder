using System;
using FluentBuilder;

namespace BuilderConsumerNET6;

[AutoGenerateBuilder]
public class User : BaseClass
{
    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public DateTime? Date { get; set; }
}