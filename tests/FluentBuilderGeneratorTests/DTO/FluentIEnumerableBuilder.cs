using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace FluentBuilder
{
    public class FluentIEnumerableBuilder<T> : Builder<T[]>
    {
        private readonly Lazy<List<T>> _list = new(() => new List<T>());

        public FluentIEnumerableBuilder<T> With(T item) => With(() => item);
        public FluentIEnumerableBuilder<T> With(Func<T> func)
        {
            _list.Value.Add(func());

            return this;
        }

        //public FluentIEnumerableBuilder<T> With(Action<FluentBuilder.Builder<T>> action, bool useObjectInitializer = true) => With(() =>
        //{
        //    var builder = new FluentBuilder.Builder<T>();
        //    action(builder);
        //    return builder.Build(useObjectInitializer);
        //});

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