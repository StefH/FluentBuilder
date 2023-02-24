using System;
using System.Collections.Generic;
using System.Text.Json;
using BuilderConsumer.Builders;
using ConsumerClassLibrary;
using FluentBuilder;

namespace BuilderConsumer;

class Program
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        WriteIndented = true
    };

    static void Main(string[] args)
    {
        var t1 = new ThingWithOnlyParameterizedConstructorBuilder();

        var s = new MyVersionBuilder().Build();
        Console.WriteLine("s = " + JsonSerializer.Serialize(s, JsonSerializerOptions));

        var ut = new MyCustomTestDtoBuilder()
            .WithName("MyCustomTestDtoBuilder")
            .Build();
        Console.WriteLine("ut = " + JsonSerializer.Serialize(ut, JsonSerializerOptions));

        var test = new TestClassBuilder()
            .WithId(100)
            .WithMyArray(100, 200, 300)
            .WithValues(eb => eb
                .Add("abc")
                .Build())
            .Build();
        Console.WriteLine("test = " + JsonSerializer.Serialize(test, JsonSerializerOptions));

        var user = new UserDtoBuilder()
            .WithDictionary1(db => db      // 👈 Use a Dictionary<TKey, TValue> Builder
                .Add("test", 123)          // Add a key with value

                .Add(() => new KeyValuePair<string, int>("x", 42)) // Add a KeyValuePair with a Func<>
                .Build()
            )

            .WithIntArray(ib => ib         // 👈 Use a Integer Array Builder
                .Add(1)                    // Add a normal integer

                .Add(() => 2)              // Add an integer with a Func<>
                .Build()
            )

            .WithSecondaryEmails(sb => sb  // 👈 Use a EmailDto IEnumerable Builder
                .Add(new EmailDto())       // Add a normal EmailDto using new() constructor

                .Add(() => new EmailDto()) // Add an EmailDto using Func<>

                .Add(eb => eb              // 👈 Use a EmailDto IEnumerable Builder to add an EmailDto
                    .WithPrimary(true)
                    .Build()
                )
                .Build()
            )
            .Build();
        Console.WriteLine("userWithArray = " + JsonSerializer.Serialize(user, JsonSerializerOptions));

        var email = new EmailDtoBuilder()
            .WithAddress("x@x.nl")
            .Build();
        Console.WriteLine("email = " + JsonSerializer.Serialize(email, JsonSerializerOptions));

        var email2a = new EmailDtoWithConstructorBuilder()
            .WithAddress("x@x.nl")
            .Build();
        Console.WriteLine("email2a = " + JsonSerializer.Serialize(email2a, JsonSerializerOptions));

        var email2b = new EmailDtoWithConstructorBuilder()
            .WithAddress("x@x.nl")
            .Build(false);
        Console.WriteLine("email2b = " + JsonSerializer.Serialize(email2b, JsonSerializerOptions));

        var email2c = new EmailDtoWithConstructorBuilder()
            .Build(false);
        Console.WriteLine("email2c = " + JsonSerializer.Serialize(email2c, JsonSerializerOptions));

        var user1 = new UserDtoBuilder()
            .WithAge(99)
            .WithFirstName("Stef")
            .WithLastName("Heyenrath")
            .WithPrimaryEmail(email)
            .WithIntArray(() => new[] { 7 })
            .WithEmailDtoArray(() => new[] { new EmailDto() })
            .Build();
        Console.WriteLine(JsonSerializer.Serialize(user1, JsonSerializerOptions));

        var user2 = new UserDtoBuilder()
            .WithAge(100)
            .WithFirstName("User")
            .WithLastName("Two")
            .WithPrimaryEmail(e => e.WithAddress("abc").WithPrimary(true))
            .WithUserDtoT(x => x.WithTValue(5))
            .Build();
        Console.WriteLine(JsonSerializer.Serialize(user2, JsonSerializerOptions));

        var userT1 = new UserDtoTBuilder<int>()
            .WithTValue(42)
            .Build();
        Console.WriteLine(JsonSerializer.Serialize(userT1, JsonSerializerOptions));

        var userWithAddress = new UserWithEmailDtoBuilder()
            .WithAge(99)
            .WithEmailWithConstructor(a => a.WithPrimary(false), false)
            .Build();
        Console.WriteLine(JsonSerializer.Serialize(userWithAddress, JsonSerializerOptions));
    }
}

[AutoGenerateBuilder]
public class UserDto
{
    public int Age { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public EmailDto PrimaryEmail { get; set; } = null!;

    public IEnumerable<EmailDto>? SecondaryEmails { get; set; }

    public DateTime? QuitDate { get; set; }

    public TestDto? Test { get; set; }

    public UserDtoT<long> UserDtoT { get; set; } = null!;

    public int[] IntArray { get; set; } = null!;

    public EmailDto[] EmailDtoArray { get; set; } = null!;

    public IDictionary<string, int> Dictionary1 { get; set; } = null!;
}

[AutoGenerateBuilder]
public class UserDtoT<T> where T : struct
{
    public T TValue { get; set; }
}

[AutoGenerateBuilder]
public class UserDtoTT<T1, T2>
    where T1 : struct
    where T2 : class, new()
{
    public T1 T1Value { get; set; }

    public T2? T2Value { get; set; }
}

[AutoGenerateBuilder]
public class EmailDto
{
    public string Address { get; set; } = null!;

    public bool Primary { get; set; }
}

[AutoGenerateBuilder]
public class EmailDtoWithConstructor
{
    public string Address { get; set; }

    public bool Primary { get; set; }

    public EmailDtoWithConstructor()
    {
        Address = "initial value";
        Primary = true;
    }
}

[AutoGenerateBuilder]
public class UserWithEmailDto
{
    public int Age { get; set; }

    public EmailDto Email { get; set; } = null!;

    public EmailDtoWithConstructor EmailWithConstructor { get; set; } = null!;
}

public class TestDto
{
    public string? X { get; set; }
}

public class MyTestDto
{
    public int Id { get; set; }

    public string? Name { get; set; }
}

[AutoGenerateBuilder(typeof(MyTestDto))]
public partial class MyCustomTestDtoBuilder
{

}

public class MyDummyVersion
{
    public long Minor { get; set; }
}

[AutoGenerateBuilder(typeof(MyDummyVersion))]
public partial class MyVersionBuilder
{

}

//[AutoGenerateBuilder(typeof(AppDomain))]
//public partial class MyAppDomainBuilder
//{

//}