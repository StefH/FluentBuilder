//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by https://github.com/StefH/FluentBuilder version 0.7.0.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable enable
using System;
using System.Collections.Generic;

namespace FluentBuilderGeneratorTests.FluentBuilder
{
    public partial class IListBuilder<T> : Builder<IList<T>>
    {
        private readonly Lazy<List<T>> _list = new Lazy<List<T>>(() => new List<T>());

        public IListBuilder<T> Add(T item) => Add(() => item);
        public IListBuilder<T> Add(Func<T> func)
        {
            _list.Value.Add(func());

            return this;
        }

        public override IList<T> Build(bool useObjectInitializer = true)
        {
            if (Object?.IsValueCreated != true)
            {
                Object = new Lazy<IList<T>>(() =>
                {
                    return _list.Value;
                });
            }

            PostBuild(Object.Value);

            return Object.Value;
        }
    }
}
#nullable disable