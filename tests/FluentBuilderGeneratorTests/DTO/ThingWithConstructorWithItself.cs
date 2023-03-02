namespace FluentBuilderGeneratorTests.DTO;

public class ThingUsingConstructorWithItself
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public ThingUsingConstructorWithItself(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public ThingUsingConstructorWithItself(ThingUsingConstructorWithItself person)
    {
        FirstName = person.FirstName;
        LastName = person.LastName;
    }
}