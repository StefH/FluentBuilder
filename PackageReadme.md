# Usage
### :one: Annotate a class
Annotate a class with `[AutoGenerateBuilder]` to indicate that a FluentBuilder should be generated for this class:
``` c#
[AutoGenerateBuilder]
public class User
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime? Date { get; set; }
}
```

### :two: Define a class which needs to act as a builder
This scenario is very usefull when you cannot modify the class to annotate it.

#### Create a public and partial builder class
And annotate this class with `[AutoGenerateBuilder(typeof(XXX))]` where `XXX` is the type for which you want to generate a FluentBuilder.
``` c#
[AutoGenerateBuilder(typeof(UserDto))]
public partial class MyUserDtoBuilder
{
}
```

### Use FluentBuilder
``` c#
using System;

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
---

For more information, see the [StefH/FluentBuilder](https://github.com/StefH/FluentBuilder).