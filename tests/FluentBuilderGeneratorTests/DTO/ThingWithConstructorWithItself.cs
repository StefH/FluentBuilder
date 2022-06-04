namespace FluentBuilderGeneratorTests.DTO;

public class ThingWithConstructorWithItself
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public ThingWithConstructorWithItself(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public ThingWithConstructorWithItself(ThingWithConstructorWithItself person)
    {
        FirstName = person.FirstName;
        LastName = person.LastName;
    }
}