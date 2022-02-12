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
    public partial class UserTWithAddressTBuilder<T> : Builder<UserTWithAddressT<T>> where T : struct
    {
        private bool _tValueIsSet;
        private Lazy<T> _tValue = new Lazy<T>(() => default(T));
        public UserTWithAddressTBuilder<T> WithTValue(T value) => WithTValue(() => value);
        public UserTWithAddressTBuilder<T> WithTValue(Func<T> func)
        {
            _tValue = new Lazy<T>(func);
            _tValueIsSet = true;
            return this;
        }
        public UserTWithAddressTBuilder<T> WithoutTValue()
        {
            WithTValue(() => default(T));
            _tValueIsSet = false;
            return this;
        }

        private bool _addressIsSet;
        private Lazy<FluentBuilderGeneratorTests.DTO.Address<short>> _address = new Lazy<FluentBuilderGeneratorTests.DTO.Address<short>>(() => default(FluentBuilderGeneratorTests.DTO.Address<short>));
        public UserTWithAddressTBuilder<T> WithAddress(FluentBuilderGeneratorTests.DTO.Address<short> value) => WithAddress(() => value);
        public UserTWithAddressTBuilder<T> WithAddress(Func<FluentBuilderGeneratorTests.DTO.Address<short>> func)
        {
            _address = new Lazy<FluentBuilderGeneratorTests.DTO.Address<short>>(func);
            _addressIsSet = true;
            return this;
        }
        public UserTWithAddressTBuilder<T> WithAddress(Action<AddressBuilder<T>> action, bool useObjectInitializer = true) => WithAddress(() =>
        {
            var builder = new AddressBuilder<T>();
            action(builder);
            return builder.Build(useObjectInitializer);
        });
        public UserTWithAddressTBuilder<T> WithoutAddress()
        {
            WithAddress(() => default(FluentBuilderGeneratorTests.DTO.Address<short>));
            _addressIsSet = false;
            return this;
        }


        public override UserTWithAddressT<T> Build(bool useObjectInitializer = true)
        {
            if (Object?.IsValueCreated != true)
            {
                Object = new Lazy<UserTWithAddressT<T>>(() =>
                {
                    if (useObjectInitializer)
                    {
                        return new UserTWithAddressT<T>
                        {
                            TValue = _tValue.Value,
                            Address = _address.Value
                        };
                    }

                    var instance = new UserTWithAddressT<T>();
                    if (_tValueIsSet) { instance.TValue = _tValue.Value; }
                    if (_addressIsSet) { instance.Address = _address.Value; }
                    return instance;
                });
            }

            PostBuild(Object.Value);

            return Object.Value;
        }

        public static UserTWithAddressT<T> Default() => new UserTWithAddressT<T>();

    }
}
#nullable disable