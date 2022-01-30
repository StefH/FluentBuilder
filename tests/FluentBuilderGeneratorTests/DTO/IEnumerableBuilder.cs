using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace FluentBuilder
{
    // ReSharper disable once InconsistentNaming
    public partial class IEnumerableBuilder<T> : Builder<T[]>
    {
        protected readonly Lazy<List<T>> _list = new(() => new List<T>());

        public virtual IEnumerableBuilder<T> Add(T item) => Add(() => item);
        public virtual IEnumerableBuilder<T> Add(Func<T> func)
        {
            _list.Value.Add(func());

            return this;
        }

        public override T[] Build(bool useObjectInitializer = true)
        {
            if (Object?.IsValueCreated != true)
            {
                Object = new Lazy<T[]>(() =>
                {
                    return _list.Value.ToArray();
                });
            }

            PostBuild(Object.Value);

            return Object.Value;
        }
    }
}