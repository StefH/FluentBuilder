//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by https://github.com/StefH/FluentBuilder.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using FluentBuilder;
using FluentBuilderGeneratorTests.DTO;

namespace FluentBuilderGeneratorTests.DTO
{
    public partial class UserTBuilder<T> : Builder<UserT<T>> where T : struct
    {
        private bool _tValueIsSet;
        private Lazy<T> _tValue = new Lazy<T>(() => default(T));
        public UserTBuilder<T> WithTValue(T value) => WithTValue(() => value);
        public UserTBuilder<T> WithTValue(Func<T> func)
        {
            _tValue = new Lazy<T>(func);
            _tValueIsSet = true;
            return this;
        }
        public UserTBuilder<T> WithoutTValue()
        {
            WithTValue(() => default(T));
            _tValueIsSet = false;
            return this;
        }


        public override UserT<T> Build(bool useObjectInitializer = true)
        {
            if (Object?.IsValueCreated != true)
            {
                Object = new Lazy<UserT<T>>(() =>
                {
                    if (useObjectInitializer)
                    {
                        return new UserT<T>
                        {
                            TValue = _tValue.Value
                        };
                    }

                    var instance = new UserT<T>();
                    if (_tValueIsSet) { instance.TValue = _tValue.Value; }
                    return instance;
                });
            }

            PostBuild(Object.Value);

            return Object.Value;
        }

        public static UserT<T> Default() => new UserT<T>();

    }
}
#nullable disable