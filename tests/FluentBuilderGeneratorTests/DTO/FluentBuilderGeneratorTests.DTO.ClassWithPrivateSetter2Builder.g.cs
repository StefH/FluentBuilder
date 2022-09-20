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
    public partial class ClassWithPrivateSetter2Builder : Builder<FluentBuilderGeneratorTests.DTO.ClassWithPrivateSetter2>
    {
        private bool _value2IsSet;
        private Lazy<int> _value2 = new Lazy<int>(() => default(int));
        public ClassWithPrivateSetter2Builder WithValue2(int value) => WithValue2(() => value);
        public ClassWithPrivateSetter2Builder WithValue2(Func<int> func)
        {
            _value2 = new Lazy<int>(func);
            _value2IsSet = true;
            return this;
        }
        public ClassWithPrivateSetter2Builder WithoutValue2()
        {
            WithValue2(() => default(int));
            _value2IsSet = false;
            return this;
        }


        public override ClassWithPrivateSetter2 Build(bool useObjectInitializer = true)
        {
            if (Object?.IsValueCreated != true)
            {
                Object = new Lazy<ClassWithPrivateSetter2>(() =>
                {
                    ClassWithPrivateSetter2 instance;
                    if (useObjectInitializer)
                    {
                        instance = new ClassWithPrivateSetter2
                        {
                            Value2 = _value2.Value
                        };

                        return instance;
                    }

                    instance = new ClassWithPrivateSetter2();
                    if (_value2IsSet) { instance.Value2 = _value2.Value; }

                    return instance;
                });
            }

            PostBuild(Object.Value);

            return Object.Value;
        }

        public static ClassWithPrivateSetter2 Default() => new ClassWithPrivateSetter2();

    }
}
#nullable disable