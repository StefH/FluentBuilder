using FluentBuilderGenerator.Models;

namespace FluentBuilderGenerator.FileGenerators;

internal interface IFilesGenerator
{
    IReadOnlyList<FileData> GenerateFiles();
}