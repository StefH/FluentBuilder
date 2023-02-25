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
    public partial class ThingWithOnlyParameterizedConstructorsBuilder : Builder<FluentBuilderGeneratorTests.DTO.ThingWithOnlyParameterizedConstructors>
    {
        private bool _xIsSet;
        private Lazy<int> _x = new Lazy<int>(() => default(int));
        public ThingWithOnlyParameterizedConstructorsBuilder WithX(int value) => WithX(() => value);
        public ThingWithOnlyParameterizedConstructorsBuilder WithX(Func<int> func)
        {
            _x = new Lazy<int>(func);
            _xIsSet = true;
            return this;
        }
        public ThingWithOnlyParameterizedConstructorsBuilder WithoutX()
        {
            WithX(() => default(int));
            _xIsSet = false;
            return this;
        }

        private bool _yIsSet;
        private Lazy<int> _y = new Lazy<int>(() => default(int));
        public ThingWithOnlyParameterizedConstructorsBuilder WithY(int value) => WithY(() => value);
        public ThingWithOnlyParameterizedConstructorsBuilder WithY(Func<int> func)
        {
            _y = new Lazy<int>(func);
            _yIsSet = true;
            return this;
        }
        public ThingWithOnlyParameterizedConstructorsBuilder WithoutY()
        {
            WithY(() => default(int));
            _yIsSet = false;
            return this;
        }

        private bool _zIsSet;
        private Lazy<string> _z = new Lazy<string>(() => string.Empty);
        public ThingWithOnlyParameterizedConstructorsBuilder WithZ(string value) => WithZ(() => value);
        public ThingWithOnlyParameterizedConstructorsBuilder WithZ(Func<string> func)
        {
            _z = new Lazy<string>(func);
            _zIsSet = true;
            return this;
        }
        public ThingWithOnlyParameterizedConstructorsBuilder WithoutZ()
        {
            WithZ(() => string.Empty);
            _zIsSet = false;
            return this;
        }


        private bool _Constructor696540298_IsSet;
        private Lazy<FluentBuilderGeneratorTests.DTO.ThingWithOnlyParameterizedConstructors> _Constructor696540298 = new Lazy<FluentBuilderGeneratorTests.DTO.ThingWithOnlyParameterizedConstructors>(() => new FluentBuilderGeneratorTests.DTO.ThingWithOnlyParameterizedConstructors(default(int)));
        public ThingWithOnlyParameterizedConstructorsBuilder WithConstructor(int x)
        {
            _Constructor696540298 = new Lazy<FluentBuilderGeneratorTests.DTO.ThingWithOnlyParameterizedConstructors>(() =>
            {
                return new FluentBuilderGeneratorTests.DTO.ThingWithOnlyParameterizedConstructors
                (
                    x
                );
            });
            _Constructor696540298_IsSet = true;

            return this;
        }

        private bool _Constructor290739056_IsSet;
        private Lazy<FluentBuilderGeneratorTests.DTO.ThingWithOnlyParameterizedConstructors> _Constructor290739056 = new Lazy<FluentBuilderGeneratorTests.DTO.ThingWithOnlyParameterizedConstructors>(() => new FluentBuilderGeneratorTests.DTO.ThingWithOnlyParameterizedConstructors(default(int),default(int),string.Empty));
        public ThingWithOnlyParameterizedConstructorsBuilder WithConstructor(int x, int y, string z = "test")
        {
            _Constructor290739056 = new Lazy<FluentBuilderGeneratorTests.DTO.ThingWithOnlyParameterizedConstructors>(() =>
            {
                return new FluentBuilderGeneratorTests.DTO.ThingWithOnlyParameterizedConstructors
                (
                    x, 
                    y, 
                    z
                );
            });
            _Constructor290739056_IsSet = true;

            return this;
        }


        public override ThingWithOnlyParameterizedConstructors Build(bool c)
        {
            if (Object?.IsValueCreated != true)
            {
                Object = new Lazy<ThingWithOnlyParameterizedConstructors>(() =>
                {
                    if (_Constructor290739056_IsSet) { return _Constructor290739056.Value; }
                    if (_Constructor696540298_IsSet) { return _Constructor696540298.Value; }

                    return Default();
                });
            }

            PostBuild(Object.Value);

            return Object.Value;
        }

        public static ThingWithOnlyParameterizedConstructors Default() => new ThingWithOnlyParameterizedConstructors(default(int), default(int), string.Empty);

        
    }
}
#nullable disable