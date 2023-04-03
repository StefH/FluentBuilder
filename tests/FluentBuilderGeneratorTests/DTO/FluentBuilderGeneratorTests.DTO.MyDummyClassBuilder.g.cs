//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by https://github.com/StefH/FluentBuilder version 0.8.0.0
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
    public partial class MyDummyClassBuilder : Builder<FluentBuilderGeneratorTests.DTO.DummyClass>
    {
        private bool _idIsSet;
        private Lazy<int> _id = new Lazy<int>(() => default(int));
        public MyDummyClassBuilder WithId(int value) => WithId(() => value);
        public MyDummyClassBuilder WithId(Func<int> func)
        {
            _id = new Lazy<int>(func);
            _idIsSet = true;
            return this;
        }

        private bool _Constructor921673711_IsSet;
        private Lazy<FluentBuilderGeneratorTests.DTO.DummyClass> _Constructor921673711 = new Lazy<FluentBuilderGeneratorTests.DTO.DummyClass>(() => new FluentBuilderGeneratorTests.DTO.DummyClass());
        public MyDummyClassBuilder UsingConstructor()
        {
            _Constructor921673711 = new Lazy<FluentBuilderGeneratorTests.DTO.DummyClass>(() =>
            {
                return new FluentBuilderGeneratorTests.DTO.DummyClass
                (

                );
            });
            _Constructor921673711_IsSet = true;

            return this;
        }


        public MyDummyClassBuilder UsingInstance(DummyClass value) => UsingInstance(() => value);

        public MyDummyClassBuilder UsingInstance(Func<DummyClass> func)
        {
            Instance = new Lazy<DummyClass>(func);
            return this;
        }

        public override DummyClass Build() => Build(true);

        public override DummyClass Build(bool useObjectInitializer)
        {
            if (Instance is null)
            {
                Instance = new Lazy<DummyClass>(() =>
                {
                    DummyClass instance;
                    if (useObjectInitializer)
                    {
                        instance = new DummyClass
                        {
                            Id = _id.Value
                        };

                        return instance;
                    }

                    if (_Constructor921673711_IsSet) { instance = _Constructor921673711.Value; }
                    else { instance = Default(); }

                    return instance;
                });
            }

            if (_idIsSet) { Instance.Value.Id = _id.Value; }

            PostBuild(Instance.Value);

            return Instance.Value;
        }

        public static DummyClass Default() => new DummyClass();

    }
}
#nullable disable