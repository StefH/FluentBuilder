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
using FluentBuilder;
using FluentBuilderGeneratorTests.DTO;

namespace FluentBuilder
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
        public UserTWithAddressTBuilder<T> WithAddress(Action<FluentBuilder.AddressBuilder<Int16>> action) => WithAddress(() =>
        {
            var builder = new FluentBuilder.AddressBuilder<Int16>();
            action(builder);
            return builder.Build();
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
                    if (typeof(UserTWithAddressT<T>).GetConstructor(Type.EmptyTypes) is null)
                    {
                        throw new NotSupportedException(ErrorMessageConstructor);
                    }

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