var user = new UserBuilder()
    .WithFirstName("Test")
    .WithLastName("User")
    .Build();

Console.WriteLine($"{user.FirstName} {user.LastName}");