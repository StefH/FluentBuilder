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

For more information, see the [StefH/FluentBuilder](https://github.com/StefH/FluentBuilder).


### Sponsors

[Entity Framework Extensions](https://entityframework-extensions.net/?utm_source=StefH) and [Dapper Plus](https://dapper-plus.net/?utm_source=StefH) are major sponsors and proud to contribute to the development of **FluentBuilder**.

[![Entity Framework Extensions](https://raw.githubusercontent.com/StefH/resources/main/sponsor/entity-framework-extensions-sponsor.png)](https://entityframework-extensions.net/bulk-insert?utm_source=StefH)

[![Dapper Plus](https://raw.githubusercontent.com/StefH/resources/main/sponsor/dapper-plus-sponsor.png)](https://dapper-plus.net/bulk-insert?utm_source=StefH)