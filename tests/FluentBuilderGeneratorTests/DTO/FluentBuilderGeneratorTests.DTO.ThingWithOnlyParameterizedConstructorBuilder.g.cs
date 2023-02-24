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
    public partial class ThingWithOnlyParameterizedConstructorBuilder : Builder<FluentBuilderGeneratorTests.DTO.ThingWithOnlyParameterizedConstructor>
    {
        private bool _xIsSet;
        private Lazy<int> _x = new Lazy<int>(() => default(int));
        public ThingWithOnlyParameterizedConstructorBuilder WithX(int value) => WithX(() => value);
        public ThingWithOnlyParameterizedConstructorBuilder WithX(Func<int> func)
        {
            _x = new Lazy<int>(func);
            _xIsSet = true;
            return this;
        }
        public ThingWithOnlyParameterizedConstructorBuilder WithoutX()
        {
            WithX(() => default(int));
            _xIsSet = false;
            return this;
        }

        private bool _yIsSet;
        private Lazy<string> _y = new Lazy<string>(() => string.Empty);
        public ThingWithOnlyParameterizedConstructorBuilder WithY(string value) => WithY(() => value);
        public ThingWithOnlyParameterizedConstructorBuilder WithY(Func<string> func)
        {
            _y = new Lazy<string>(func);
            _yIsSet = true;
            return this;
        }
        public ThingWithOnlyParameterizedConstructorBuilder WithoutY()
        {
            WithY(() => string.Empty);
            _yIsSet = false;
            return this;
        }

        private bool _zIsSet;
        private Lazy<string> _z = new Lazy<string>(() => string.Empty);
        public ThingWithOnlyParameterizedConstructorBuilder WithZ(string value) => WithZ(() => value);
        public ThingWithOnlyParameterizedConstructorBuilder WithZ(Func<string> func)
        {
            _z = new Lazy<string>(func);
            _zIsSet = true;
            return this;
        }
        public ThingWithOnlyParameterizedConstructorBuilder WithoutZ()
        {
            WithZ(() => string.Empty);
            _zIsSet = false;
            return this;
        }


        private bool _591074099_xIsSet;
        private Lazy<int> _591074099_x = new Lazy<int>(() => default(int));
        private bool _591074099_yIsSet;
        private Lazy<string> _591074099_y = new Lazy<string>(() => string.Empty);
        private bool _591074099_zIsSet;
        private Lazy<string> _591074099_z = new Lazy<string>(() => string.Empty);
        public ThingWithOnlyParameterizedConstructorBuilder WithConstructor(int x, string y, string z = "test")
        {
            _591074099_x = new Lazy<int>(() => x);
            _591074099_xIsSet = true;

            _591074099_y = new Lazy<string>(() => y);
            _591074099_yIsSet = true;

            _591074099_z = new Lazy<string>(() => z);
            _591074099_zIsSet = true;


            return this;
        }

        public override ThingWithOnlyParameterizedConstructor Build(bool useObjectInitializer = true)
        {
            if (Object?.IsValueCreated != true)
            {
               
            }

            PostBuild(Object.Value);

            return Object.Value;
        }


    }
}
#nullable disable