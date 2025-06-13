# ![Icon](./resources/icon_32x32.png) Projects

See also these blogs:
- [mstack.nl - source-generators part 1](https://mstack.nl/blog/20210801-source-generators)
- [mstack.nl - source-generators part 2](https://mstack.nl/blog/20220801-source-generators-part2/)
- [mstack.nl - source-generators part 3](https://mstack.nl/blogs/source-generators-part3/)

## ⭐ CSharp.SourceGenerators.Extensions
See this [page](https://github.com/StefH/FluentBuilder/tree/main/src-extensions).

## ⭐ FluentBuilder
A project which uses Source Generation to create a FluentBuilder for a specified model or DTO.

This project is based on [Tom Phan : "auto-generate-builders-using-source-generator-in-net-5"](https://justsimplycode.com/2020/12/06/auto-generate-builders-using-source-generator-in-net-5/).

## 📦 NuGet
[![NuGet Badge](https://shields.io/nuget/v/FluentBuilder)](https://www.nuget.org/packages/FluentBuilder)

:memo: Note that FluentBuilder version 0.10.0 requires at least Visual Studio 17.11.5

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

This attribute has 4 arguments:
- **type** (Type): The type for which to create the builder, see 'Define a class which needs to act as a builder'
- **handleBaseClasses** (bool): Handle also base-classes. Default value is `true`.
- **accessibility** (enum FluentBuilderAccessibility): Generate builder methods for `Public` or `PublicAndPrivate`. Default value when not provided is `Public`.
- **methods** (enum  FluentBuilderMethods): Generate `With***` methods or also `Without***` methods. Default value when not provided is `WithOnly`. See also [Notes](#Notes)

### Use FluentBuilder
``` c#
using System;
using FluentBuilder;

namespace Test;

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
```

### Use FluentBuilder when the class has a default (parameter-less) constructor
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

namespace Test;

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
```

### Use FluentBuilder when the class has a constructor with parameters
``` c#
using FluentBuilder;

[AutoGenerateBuilder]
public class User
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime? Date { get; set; }

    public User(string first)
    {
        FirstName = first;
    }
}
```

``` c#
using System;
using FluentBuilder;

namespace Test;

class Program
{
    static void Main(string[] args)
    {
        var user = new UserBuilder()
            .UsingConstructor("First")  // ⭐ Use `UsingConstructor` here.
            .WithLastName("User")
            .Build();

        Console.WriteLine($"{user.FirstName} {user.LastName}");
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

#### ℹ️ Generic Attribute
If you are using **C# 11.0** (`.NET 7` and up), you can also use the generic `AutoGenerateBuilder<T>`-attribute.
Example:
``` c#
using FluentBuilder;

[AutoGenerateBuilder<UserDto>()]
public partial class MyUserDtoBuilder
{
}
```


### Use FluentBuilder
``` c#
using System;
using FluentBuilder;

namespace Test;

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
```

## Extension Method

By default, the `AsBuilder` extension method is also generated which allows you to change an existing instance using the `With`-methods from the builder.

Example:
``` c#
var user = await dbContext.Users.FirstAsync();

user = user.AsBuilder() // Lifts the user into a builder instance
    .WithLastName("Different LastName") // Updates "LastName" while keeping existing properties
    .Build(); // Changes are applied

await dbContext.SaveChangesAsync(); // User's LastName property is updated.
```

## Notes

Since version 0.8.0, this FluentBuilder will only generate the `With***` methods. If you want the builder to also generate the `Without***` methods, add the enum `FluentBuilderMethods.WithAndWithout` to the attribute.

``` c#
using FluentBuilder;

[AutoGenerateBuilder(typeof(UserDto), FluentBuilderMethods.WithAndWithout)]
public partial class MyUserDtoBuilder
{
}
```

## Links
- https://protogen.marcgravell.com/
- https://protobuf-decoder.netlify.app/
- https://www.protobufpal.com/


## Sponsors

[Entity Framework Extensions](https://entityframework-extensions.net/?utm_source=StefH) and [Dapper Plus](https://dapper-plus.net/?utm_source=StefH) are major sponsors and proud to contribute to the development of **FluentBuilder** and **CSharp.SourceGenerators.Extensions**.

[![Entity Framework Extensions](https://raw.githubusercontent.com/StefH/resources/main/sponsor/entity-framework-extensions-sponsor.png)](https://entityframework-extensions.net/bulk-insert?utm_source=StefH)

[![Dapper Plus](https://raw.githubusercontent.com/StefH/resources/main/sponsor/dapper-plus-sponsor.png)](https://dapper-plus.net/bulk-insert?utm_source=StefH)