using System;

namespace BuilderConsumerNET6
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