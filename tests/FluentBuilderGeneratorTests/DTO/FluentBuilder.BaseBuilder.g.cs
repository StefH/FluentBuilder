//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by https://github.com/StefH/FluentBuilder version 0.8.0.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;

namespace FluentBuilderGeneratorTests.FluentBuilder
{
    public abstract class Builder<T>
    {
        protected Lazy<T> Instance;

        protected Type InstanceType = typeof(T);

        public abstract T Build();

        public abstract T Build(bool useObjectInitializer);
   
        protected virtual void PostBuild(T value) {}
    }
}