//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by https://github.com/StefH/FluentBuilder version 0.5.1.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using FluentBuilderGeneratorTests.FluentBuilder;
using FluentBuilderGeneratorTests.DTO;

namespace FluentBuilderGeneratorTests.DTO
{
    public partial class UserTWithAddressTBuilder<T> : Builder<FluentBuilderGeneratorTests.DTO.UserTWithAddressT<T>> where T : struct
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
        private Lazy<FluentBuilderGeneratorTests.DTO.Address<short>> _address = new Lazy<FluentBuilderGeneratorTests.DTO.Address<short>>(() => new FluentBuilderGeneratorTests.DTO.Address<short>());
        public UserTWithAddressTBuilder<T> WithAddress(FluentBuilderGeneratorTests.DTO.Address<short> value) => WithAddress(() => value);
        public UserTWithAddressTBuilder<T> WithAddress(Func<FluentBuilderGeneratorTests.DTO.Address<short>> func)
        {
            _address = new Lazy<FluentBuilderGeneratorTests.DTO.Address<short>>(func);
            _addressIsSet = true;
            return this;
        }
        public UserTWithAddressTBuilder<T> WithAddress(Action<FluentBuilderGeneratorTests.DTO.AddressBuilder<short>> action, bool useObjectInitializer = true) => WithAddress(() =>
        {
            var builder = new FluentBuilderGeneratorTests.DTO.AddressBuilder<short>();
            action(builder);
            return builder.Build(useObjectInitializer);
        });
        public UserTWithAddressTBuilder<T> WithoutAddress()
        {
            WithAddress(() => new FluentBuilderGeneratorTests.DTO.Address<short>());
            _addressIsSet = false;
            return this;
        }


        public override UserTWithAddressT<T> Build(bool useObjectInitializer = true)
        {
            if (Object?.IsValueCreated != true)
            {
                Object = new Lazy<UserTWithAddressT<T>>(() =>
                {
                    UserTWithAddressT<T> instance;
                    if (useObjectInitializer)
                    {
                        instance = new UserTWithAddressT<T>
                        {
                            TValue = _tValue.Value,
                            Address = _address.Value
                        };
                        return instance;
                    }

                    instance = new UserTWithAddressT<T>();
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