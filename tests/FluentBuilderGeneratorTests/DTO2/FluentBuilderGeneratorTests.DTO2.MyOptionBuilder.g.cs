//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by https://github.com/StefH/FluentBuilder version 0.10.0.0
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

namespace FluentBuilderGeneratorTests.DTO2
{
    public static partial class MyOptionBuilderExtensions
    {
        public static MyOptionBuilder AsBuilder(this FluentBuilderGeneratorTests.DTO.Option instance)
        {
            return new MyOptionBuilder().UsingInstance(instance);
        }
    }

    public partial class MyOptionBuilder : Builder<FluentBuilderGeneratorTests.DTO.Option>
    {
        private bool _nameIsSet;
        private Lazy<string> _name = new Lazy<string>(() => string.Empty);
        public MyOptionBuilder WithName(string value) => WithName(() => value);
        public MyOptionBuilder WithName(Func<string> func)
        {
            _name = new Lazy<string>(func);
            _nameIsSet = true;
            return this;
        }

        private bool _Constructor155259363_IsSet;
        private Lazy<FluentBuilderGeneratorTests.DTO.Option> _Constructor155259363 = new Lazy<FluentBuilderGeneratorTests.DTO.Option>(() => new FluentBuilderGeneratorTests.DTO.Option());
        public MyOptionBuilder UsingConstructor()
        {
            _Constructor155259363 = new Lazy<FluentBuilderGeneratorTests.DTO.Option>(() =>
            {
                return new FluentBuilderGeneratorTests.DTO.Option
                (

                );
            });
            _Constructor155259363_IsSet = true;

            return this;
        }


        public MyOptionBuilder UsingInstance(Option value) => UsingInstance(() => value);

        public MyOptionBuilder UsingInstance(Func<Option> func)
        {
            Instance = new Lazy<Option>(func);
            return this;
        }

        public override Option Build() => Build(true);

        public override Option Build(bool useObjectInitializer)
        {
            if (Instance is null)
            {
                Instance = new Lazy<Option>(() =>
                {
                    Option instance;
                    if (useObjectInitializer)
                    {
                        instance = new Option
                        {
                            Name = _name.Value
                        };

                        return instance;
                    }

                    if (_Constructor155259363_IsSet) { instance = _Constructor155259363.Value; }
                    else { instance = Default(); }

                    return instance;
                });
            }

            if (_nameIsSet) { Instance.Value.Name = _name.Value; }

            PostBuild(Instance.Value);

            return Instance.Value;
        }

        public static Option Default() => new Option();

    }
}
#nullable disable