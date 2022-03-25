var user = new UserBuilder()
    .WithFirstName("Test")
    .WithLastName("User")
    .WithDate(DateTime.MaxValue)
    .Build();

Console.WriteLine($"{user.FirstName} {user.LastName}");