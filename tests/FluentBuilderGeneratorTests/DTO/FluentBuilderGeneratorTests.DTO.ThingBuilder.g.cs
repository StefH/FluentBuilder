//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by https://github.com/StefH/FluentBuilder version 0.7.0.0
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
    public partial class ThingBuilder : Builder<FluentBuilderGeneratorTests.DTO.Thing>
    {
        private bool _tIsSet;
        private Lazy<FluentBuilderGeneratorTests.DTO.Thing> _t = new Lazy<FluentBuilderGeneratorTests.DTO.Thing>(() => new FluentBuilderGeneratorTests.DTO.Thing());
        public ThingBuilder WithT(FluentBuilderGeneratorTests.DTO.Thing value) => WithT(() => value);
        public ThingBuilder WithT(Func<FluentBuilderGeneratorTests.DTO.Thing> func)
        {
            _t = new Lazy<FluentBuilderGeneratorTests.DTO.Thing>(func);
            _tIsSet = true;
            return this;
        }
        public ThingBuilder WithT(Action<FluentBuilderGeneratorTests.DTO.ThingBuilder> action, bool useObjectInitializer = true) => WithT(() =>
        {
            var builder = new FluentBuilderGeneratorTests.DTO.ThingBuilder();
            action(builder);
            return builder.Build(useObjectInitializer);
        });
        public ThingBuilder WithoutT()
        {
            WithT(() => new FluentBuilderGeneratorTests.DTO.Thing());
            _tIsSet = false;
            return this;
        }



        public override Thing Build() => Build(true);

        public override Thing Build(bool useObjectInitializer)
        {
            if (Object?.IsValueCreated != true)
            {
                Object = new Lazy<Thing>(() =>
                {
                    Thing instance;
                    if (useObjectInitializer)
                    {
                        instance = new Thing
                        {
                            T = _t.Value
                        };

                        return instance;
                    }

                    instance = new Thing();
                    if (_tIsSet) { instance.T = _t.Value; }

                    return instance;
                });
            }

            PostBuild(Object.Value);

            return Object.Value;
        }

        public static Thing Default() => new Thing();

    }
}
#nullable disable