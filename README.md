# Projects

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

## Usage
### Annotate
Annotate a class with `[FluentBuilder.AutoGenerateBuilder]` to indicate that a FluentBuilder should be generated for this class:
``` c#
[FluentBuilder.AutoGenerateBuilder]
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
            var user = new FluentBuilder.UserBuilder()
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
[FluentBuilder.AutoGenerateBuilder]
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

### Using FluentBuilder when a class has an `Array` or `IEnumerable<T>` properties
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