// This source code is based on https://justsimplycode.com/2020/12/06/auto-generate-builders-using-source-generator-in-net-5
namespace FluentBuilderGenerator.FileGenerators
{
    internal class AutoGenerateBuilderAttributeGenerator : IFileGenerator
    {
        private const string Name = "AutoGenerateBuilderAttribute.cs";

        public Data GenerateFile()
        {
            return new Data
            {
                FileName = Name,
                Text = @"using System;

namespace FluentBuilder
{
    [AttributeUsage(AttributeTargets.Class)]
    sealed class AutoGenerateBuilderAttribute : Attribute
    {
        public AutoGenerateBuilderAttribute() {}
    }
}"
            };
        }
    }
}