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
