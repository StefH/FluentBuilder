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
    public partial class TestBuilder : Builder<FluentBuilderGeneratorTests.DTO.Test>
    {
        private bool _classOnOtherNamespaceListIsSet;
        private Lazy<System.Collections.Generic.List<AbcTest.OtherNamespace.ClassOnOtherNamespace>> _classOnOtherNamespaceList = new Lazy<System.Collections.Generic.List<AbcTest.OtherNamespace.ClassOnOtherNamespace>>(() => new List<AbcTest.OtherNamespace.ClassOnOtherNamespace>());
        public TestBuilder WithClassOnOtherNamespaceList(System.Collections.Generic.List<AbcTest.OtherNamespace.ClassOnOtherNamespace> value) => WithClassOnOtherNamespaceList(() => value);
        public TestBuilder WithClassOnOtherNamespaceList(Func<System.Collections.Generic.List<AbcTest.OtherNamespace.ClassOnOtherNamespace>> func)
        {
            _classOnOtherNamespaceList = new Lazy<System.Collections.Generic.List<AbcTest.OtherNamespace.ClassOnOtherNamespace>>(func);
            _classOnOtherNamespaceListIsSet = true;
            return this;
        }
        public TestBuilder WithClassOnOtherNamespaceList(Action<AbcTest.OtherNamespace.IListClassOnOtherNamespaceBuilder> action, bool useObjectInitializer = true) => WithClassOnOtherNamespaceList(() =>
        {
            var builder = new AbcTest.OtherNamespace.IListClassOnOtherNamespaceBuilder();
            action(builder);
            return (System.Collections.Generic.List<AbcTest.OtherNamespace.ClassOnOtherNamespace>) builder.Build(useObjectInitializer);
        });

        private bool _Constructor1204588943_IsSet;
        private Lazy<FluentBuilderGeneratorTests.DTO.Test> _Constructor1204588943 = new Lazy<FluentBuilderGeneratorTests.DTO.Test>(() => new FluentBuilderGeneratorTests.DTO.Test());
        public TestBuilder UsingConstructor()
        {
            _Constructor1204588943 = new Lazy<FluentBuilderGeneratorTests.DTO.Test>(() =>
            {
                return new FluentBuilderGeneratorTests.DTO.Test
                (

                );
            });
            _Constructor1204588943_IsSet = true;

            return this;
        }


        public TestBuilder UsingInstance(Test value) => UsingInstance(() => value);

        public TestBuilder UsingInstance(Func<Test> func)
        {
            Instance = new Lazy<Test>(func);
            return this;
        }

        public override Test Build() => Build(true);

        public override Test Build(bool useObjectInitializer)
        {
            if (Instance is null)
            {
                Instance = new Lazy<Test>(() =>
                {
                    Test instance;
                    if (useObjectInitializer)
                    {
                        instance = new Test
                        {
                            ClassOnOtherNamespaceList = _classOnOtherNamespaceList.Value
                        };

                        return instance;
                    }

                    if (_Constructor1204588943_IsSet) { instance = _Constructor1204588943.Value; }
                    else { instance = Default(); }

                    return instance;
                });
            }

            if (_classOnOtherNamespaceListIsSet) { Instance.Value.ClassOnOtherNamespaceList = _classOnOtherNamespaceList.Value; }

            PostBuild(Instance.Value);

            return Instance.Value;
        }

        public static Test Default() => new Test();

    }
}
#nullable disable