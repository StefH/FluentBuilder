using FluentBuilderGenerator.Models;

namespace FluentBuilderGeneratorTests.DTO;

public class SimpleClass
{
    public int Id { get; set; }
    [IgnoreProperty] //this should be ignored or it will fail.
    public System.Globalization.CultureInfo? CultureInfo { get; set; }
}