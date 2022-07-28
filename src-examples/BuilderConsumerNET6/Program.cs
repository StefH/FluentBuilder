using System;

namespace BuilderConsumerNET6;

internal class Program
{
    static void Main(string[] args)
    {
        var userA = new UserBuilder()
            .WithId(42)
            .WithFirstName("Test")
            .WithLastName("User")
            .Build();

        Console.WriteLine($"{userA.FirstName} {userA.LastName} {userA.Id}");

        var userB = new UserBuilder()
            .WithId(42)
            .Build();

        Console.WriteLine($"{userB.FirstName.Length} {userB.LastName == null} {userB.Id}");

        var userC = new UserNotHandleBaseClassBuilder()
            // .WithId(42)
            .WithFirstName("C-F")
            .WithLastName("C-L")
            .Build();

        Console.WriteLine($"{userC.FirstName.Length} {userC.LastName == null}");
    }
}