//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by https://github.com/StefH/FluentBuilder version 0.7.1.0
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
    public partial class IEnumerableBuilder<T> : Builder<IEnumerable<T>>
    {
        private readonly Lazy<List<T>> _list = new Lazy<List<T>>(() => new List<T>());

        public IEnumerableBuilder<T> Add(T item) => Add(() => item);
        public IEnumerableBuilder<T> Add(Func<T> func)
        {
            _list.Value.Add(func());

            return this;
        }

        public override IEnumerable<T> Build() => Build(true);

        public override IEnumerable<T> Build(bool useObjectInitializer)
        {
            if (Object?.IsValueCreated != true)
            {
                Object = new Lazy<IEnumerable<T>>(() =>
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