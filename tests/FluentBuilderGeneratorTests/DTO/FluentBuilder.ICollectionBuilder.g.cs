//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by https://github.com/StefH/FluentBuilder.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace FluentBuilder
{
    public partial class ICollectionBuilder<T> : Builder<ICollection<T>>
    {
        protected readonly Lazy<List<T>> _list = new Lazy<List<T>>(() => new List<T>());

        public virtual ICollectionBuilder<T> Add(T item) => Add(() => item);
        public virtual ICollectionBuilder<T> Add(Func<T> func)
        {
            _list.Value.Add(func());

            return this;
        }

        public override ICollection<T> Build(bool useObjectInitializer = true)
        {
            if (Object?.IsValueCreated != true)
            {
                Object = new Lazy<ICollection<T>>(() =>
                {
                    return _list.Value;
                });
            }

            PostBuild(Object.Value);

            return Object.Value;
        }
    }
}