using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace FluentBuilder
{
    public class FluentIEnumerableBuilder<T> : Builder<T[]>
    {
        private readonly Lazy<List<T>> _list = new(() => new List<T>());

        public FluentIEnumerableBuilder<T> With(T item)
        {
            _list.Value.Add(item);

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