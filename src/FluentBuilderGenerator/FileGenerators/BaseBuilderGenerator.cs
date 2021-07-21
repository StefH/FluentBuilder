// This source code is based on https://justsimplycode.com/2020/12/06/auto-generate-builders-using-source-generator-in-net-5
namespace FluentBuilderGenerator.FileGenerators
{
    internal class BaseBuilderGenerator : IFileGenerator
    {
        private const string Name = "BaseBuilder.cs";

        public Data GenerateFile()
        {
            return new Data
            {
                FileName = Name,
                Text = @"using System;
using System.Collections.Generic;
using System.Text;

namespace FluentBuilder
{
    public abstract class Builder<T> where T : class
    {
        protected Lazy<T> Object;

        public abstract T Build();

        public Builder<T> WithObject(T value) => WithObject(() => value);

        public Builder<T> WithObject(Func<T> func)
        {
            Object = new Lazy<T>(func);
            return this;
        }
    
        protected virtual void PostBuild(T value) {}
    }
}"
            };
        }
    }
}