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
    public partial class ClassWithInitBuilder : Builder<FluentBuilderGeneratorTests.DTO.ClassWithInit>
    {
        private bool _normalIsSet;
        private Lazy<string> _normal = new Lazy<string>(() => string.Empty);
        public ClassWithInitBuilder WithNormal(string value) => WithNormal(() => value);
        public ClassWithInitBuilder WithNormal(Func<string> func)
        {
            _normal = new Lazy<string>(func);
            _normalIsSet = true;
            return this;
        }
        public ClassWithInitBuilder WithoutNormal()
        {
            WithNormal(() => string.Empty);
            _normalIsSet = false;
            return this;
        }



        public override ClassWithInit Build(bool useObjectInitializer = true)
        {
            if (Object?.IsValueCreated != true)
            {
                Object = new Lazy<ClassWithInit>(() =>
                {
                    ClassWithInit instance;
                    if (useObjectInitializer)
                    {
                        instance = new ClassWithInit
                        {
                            Normal = _normal.Value
                        };

                        return instance;
                    }

                    instance = new ClassWithInit();
                    if (_normalIsSet) { instance.Normal = _normal.Value; }

                    return instance;
                });
            }

            PostBuild(Object.Value);

            return Object.Value;
        }

        public static ClassWithInit Default() => new ClassWithInit();

    }
}
#nullable disable