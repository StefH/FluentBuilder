using System;

namespace BuilderConsumerNET6;

internal class Program
{
    static void Main(string[] args)
    {
        var user1 = new UserBuilder()
            .WithId(42)
            .WithFirstName("Test")
            .WithLastName("User")
            .Build();

        Console.WriteLine($"{user1.FirstName} {user1.LastName} {user1.Id}");

        var user2 = new UserBuilder()
            .WithId(42)
            .Build();

        Console.WriteLine($"{user2.FirstName.Length} {user2.LastName == null} {user2.Id}");
    }
}