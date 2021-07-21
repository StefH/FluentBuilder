using System.Collections.Generic;

namespace FluentBuilderGenerator.FileGenerators
{
    internal interface IFilesGenerator
    {
        IEnumerable<Data> GenerateFiles();
    }
}