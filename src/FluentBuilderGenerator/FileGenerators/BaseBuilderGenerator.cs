// This source code is based on https://justsimplycode.com/2020/12/06/auto-generate-builders-using-source-generator-in-net-5
namespace FluentBuilderGenerator.FileGenerators
{
    internal class BaseBuilderGenerator : IFileGenerator
    {
        private const string Name = "FluentBuilder.BaseBuilder.g.cs";

        public FileData GenerateFile()
        {
            return new FileData
            {
                FileName = Name,
                Text = @"//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by https://github.com/StefH/FluentBuilder.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace FluentBuilder
{
    public abstract class Builder<T> where T : class
    {
        protected static string ErrorMessageConstructor = ""Only parameterless constructors are supported in FluentBuilder."";

        protected Lazy<T> Object;

        public abstract T Build(bool callDefaultConstructorIfPresent = false);

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