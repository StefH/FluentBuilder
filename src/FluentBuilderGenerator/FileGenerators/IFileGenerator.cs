using FluentBuilderGenerator.Models;

namespace FluentBuilderGenerator.FileGenerators;

internal interface IFileGenerator
{
    FileData GenerateFile();
}