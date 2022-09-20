# Projects

See also these blogs:
- [mstack.nl - source-generators part 1](https://mstack.nl/blog/20210801-source-generators)
- [mstack.nl - source-generators part 2](https://mstack.nl/blog/20220801-source-generators-part2/)

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
using FluentBuilder;

[AutoGenerateBuilder]
public class User
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime? Date { get; set; }

    [FluentBuilderIgnore] // Add this attribute to ignore this property when generating a FluentBuilder.
    public int Age { get; set; }

    public int Answer { get; set; } = 42; // When a default value is set, this value is also set as default in the FluentBuilder.
}
```

#### AutoGenerateBuilder - attribute

This attribute has 3 arguments:
- `type` (Type) = The type for which to create the builder, see 'Define a class which needs to act as a builder'
- `handleBaseClasses` (bool) = Handle also base-classes. Default value is `true`.
- `accessibility` (FluentBuilderAccessibility) = Generate builder methods for `Public` or `PublicAndPrivate`. Default value when not provided is `Public`.

### Use FluentBuilder
``` c#
using System;
using FluentBuilder;

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
using FluentBuilder;

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
using FluentBuilder;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var user = new UserBuilder()
                .WithLastName("User")
                .Build(false); // ⭐ Provide `false` for `useObjectInitializer` here.

            Console.WriteLine($"{user.FirstName} {user.LastName}");
        }
    }
}
```

### Using FluentBuilder when a class has an `Array` or `IEnumerable<T>` property
``` c#
using FluentBuilder;

[AutoGenerateBuilder]
public class UserDto
{
    public IEnumerable<EmailDto> SecondaryEmails { get; set; }

    public int[] IntArray { get; set; }
}
```

``` c#
var user = new UserDtoBuilder()
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
using FluentBuilder;

[AutoGenerateBuilder]
public class UserDto
{
    public IDictionary<string, int> Tags { get; set; }
}
```

``` c#
var user = new UserDtoBuilder()
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
And annotate this class with `[AutoGenerateBuilder(typeof(XXX))]` where `XXX` is the type for which you want to generate a FluentBuilder.
``` c#
using FluentBuilder;

[AutoGenerateBuilder(typeof(UserDto))]
public partial class MyUserDtoBuilder
{
}
```

### Use FluentBuilder
``` c#
using System;
using FluentBuilder;

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