using System;

namespace BuilderConsumerNET6;

internal class Program
{
    static void Main(string[] args)
    {
        var user = new UserBuilder()
            .WithId(42)
            .WithFirstName("Test")
            .WithLastName("User")
            .Build();

        Console.WriteLine($"{user.FirstName} {user.LastName} {user.Id}");
    }
}