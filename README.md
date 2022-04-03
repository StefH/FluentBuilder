# Projects

See also this blog: [mstack.nl - source-generators](https://mstack.nl/blog/20210801-source-generators).

## CSharp.SourceGenerators.Extensions
See this [page](https://github.com/StefH/FluentBuilder/tree/main/src-extensions).

## FluentBuilder
A project which uses Source Generation to create a FluentBuilder for a specified model or DTO.

This project is based on [Tom Phan : "auto-generate-builders-using-source-generator-in-net-5"](https://justsimplycode.com/2020/12/06/auto-generate-builders-using-source-generator-in-net-5/).

## Install
[![NuGet Badge](https://buildstats.info/nuget/FluentBuilder)](https://www.nuget.org/packages/FluentBuilder)

You can install from NuGet using the following command in the package manager window:

`Install-Package FluentBuilder`

Or via the Visual Studio NuGet package manager or if you use the `dotnet` command:

`dotnet add package FluentBuilder`

## :one: Usage on a existing class
### Annotate a class
Annotate an existing class with `[FluentBuilder.AutoGenerateBuilder]` to indicate that a FluentBuilder should be generated for this class:
``` c#
[AutoGenerateBuilder]
public class User
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime? Date { get; set; }
}
```

### Use FluentBuilder
``` c#
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var user = new UserBuilder()
                .WithFirstName("Test")
                .WithLastName("User")
                .Build();

            Console.WriteLine($"{user.FirstName} {user.LastName}");
        }
    }
}
```

### Use FluentBuilder when the class has a default constructor
``` c#
[AutoGenerateBuilder]
public class User
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime? Date { get; set; }

    public User()
    {
        FirstName = "test";
    }
}
```

``` c#
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var user = new FluentBuilder.UserBuilder()
                .WithLastName("User")
                .Build(false); // ⭐ Provide `false` for `useObjectInitializer` here.

            Console.WriteLine($"{user.FirstName} {user.LastName}");
        }
    }
}
```

### Using FluentBuilder when a class has an `Array` or `IEnumerable<T>` property
``` c#
[FluentBuilder.AutoGenerateBuilder]
public class UserDto
{
    public IEnumerable<EmailDto> SecondaryEmails { get; set; }

    public int[] IntArray { get; set; }
}
```

``` c#
var user = new FluentBuilder.UserDtoBuilder()
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
```

### Using FluentBuilder when a class has an `IDictionary<TKey, TValue>` property
``` c#
[FluentBuilder.AutoGenerateBuilder]
public class UserDto
{
    public IDictionary<string, int> Tags { get; set; }
}
```

``` c#
var user = new FluentBuilder.UserDtoBuilder()
    .WithTags(db => db      // 👈 Use a Dictionary<TKey, TValue> Builder
        .Add("test", 123)   // Add a key with value

        .Add(() => new KeyValuePair<string, int>("x", 42)) // Add a KeyValuePair with a Func<>
        .Build()
    )
    .Build();
```

## :two: Define a class which needs to act as a builder
This scenario is very usefull when you cannot modify the class to annotate it.

### Create a public and partial builder class
And annotate this class with `[FluentBuilder.AutoGenerateBuilder(typeof(XXX))]` where `XXX` is the type for which you want to generate a FluentBuilder.
``` c#
[FluentBuilder.AutoGenerateBuilder(typeof(UserDto))]
public partial class MyUserDtoBuilder
{
}
```

### Use FluentBuilder
``` c#
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var user = new MyUserDtoBuilder() // 👈 Just use your own Builder
                .WithFirstName("Test")
                .WithLastName("User")
                .Build();

            Console.WriteLine($"{user.FirstName} {user.LastName}");
        }
    }
}
```