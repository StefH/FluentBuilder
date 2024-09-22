//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by https://github.com/StefH/FluentBuilder version 0.9.2.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using FluentBuilderGeneratorTests.FluentBuilder;
using FluentBuilderGeneratorTests.DTO;

namespace FluentBuilderGeneratorTests.DTO
{
    public static partial class AddressTTBuilder_T1_T2_Extensions
    {
        public static AddressTTBuilder<T1,T2> AsBuilder<T1, T2>(this FluentBuilderGeneratorTests.DTO.AddressTT<T1, T2> instance) where T1 : struct where T2 : class, new()
        {
            return new AddressTTBuilder<T1,T2>().UsingInstance(instance);
        }
    }

    public partial class AddressTTBuilder<T1,T2> : Builder<FluentBuilderGeneratorTests.DTO.AddressTT<T1, T2>> where T1 : struct where T2 : class, new()
    {
        private bool _testValue1IsSet;
        private Lazy<T1> _testValue1 = new Lazy<T1>(() => default(T1));
        public AddressTTBuilder<T1,T2> WithTestValue1(T1 value) => WithTestValue1(() => value);
        public AddressTTBuilder<T1,T2> WithTestValue1(Func<T1> func)
        {
            _testValue1 = new Lazy<T1>(func);
            _testValue1IsSet = true;
            return this;
        }
        private bool _testValue2IsSet;
        private Lazy<T2?> _testValue2 = new Lazy<T2?>(() => default(T2?));
        public AddressTTBuilder<T1,T2> WithTestValue2(T2? value) => WithTestValue2(() => value);
        public AddressTTBuilder<T1,T2> WithTestValue2(Func<T2?> func)
        {
            _testValue2 = new Lazy<T2?>(func);
            _testValue2IsSet = true;
            return this;
        }

        private bool _Constructor_758958168_IsSet;
        private Lazy<FluentBuilderGeneratorTests.DTO.AddressTT<T1, T2>> _Constructor_758958168 = new Lazy<FluentBuilderGeneratorTests.DTO.AddressTT<T1, T2>>(() => new FluentBuilderGeneratorTests.DTO.AddressTT<T1, T2>());
        public AddressTTBuilder<T1,T2> UsingConstructor()
        {
            _Constructor_758958168 = new Lazy<FluentBuilderGeneratorTests.DTO.AddressTT<T1, T2>>(() =>
            {
                return new FluentBuilderGeneratorTests.DTO.AddressTT<T1, T2>
                (

                );
            });
            _Constructor_758958168_IsSet = true;

            return this;
        }


        public AddressTTBuilder<T1,T2> UsingInstance(AddressTT<T1, T2> value) => UsingInstance(() => value);

        public AddressTTBuilder<T1,T2> UsingInstance(Func<AddressTT<T1, T2>> func)
        {
            Instance = new Lazy<AddressTT<T1, T2>>(func);
            return this;
        }

        public override AddressTT<T1, T2> Build() => Build(true);

        public override AddressTT<T1, T2> Build(bool useObjectInitializer)
        {
            if (Instance is null)
            {
                Instance = new Lazy<AddressTT<T1, T2>>(() =>
                {
                    AddressTT<T1, T2> instance;
                    if (useObjectInitializer)
                    {
                        instance = new AddressTT<T1, T2>
                        {
                            TestValue1 = _testValue1.Value,
                            TestValue2 = _testValue2.Value
                        };

                        return instance;
                    }

                    if (_Constructor_758958168_IsSet) { instance = _Constructor_758958168.Value; }
                    else { instance = Default(); }

                    return instance;
                });
            }

            if (_testValue1IsSet) { Instance.Value.TestValue1 = _testValue1.Value; }
            if (_testValue2IsSet) { Instance.Value.TestValue2 = _testValue2.Value; }

            PostBuild(Instance.Value);

            return Instance.Value;
        }

        public static AddressTT<T1, T2> Default() => new AddressTT<T1, T2>();

    }
}
#nullable disable