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
using System.Globalization;
using MyNamespace;
using MyNamespace2;

namespace FluentBuilderGeneratorTests.DTO
{
    public partial class ClassWithPropertyValueSetBuilder : Builder<FluentBuilderGeneratorTests.DTO.ClassWithPropertyValueSet>
    {
        private bool _noIntValueSetIsSet;
        private Lazy<int> _noIntValueSet = new Lazy<int>(() => default(int));
        public ClassWithPropertyValueSetBuilder WithNoIntValueSet(int value) => WithNoIntValueSet(() => value);
        public ClassWithPropertyValueSetBuilder WithNoIntValueSet(Func<int> func)
        {
            _noIntValueSet = new Lazy<int>(func);
            _noIntValueSetIsSet = true;
            return this;
        }
        public ClassWithPropertyValueSetBuilder WithoutNoIntValueSet()
        {
            WithNoIntValueSet(() => default(int));
            _noIntValueSetIsSet = false;
            return this;
        }

        private bool _intValueSet1IsSet;
        private Lazy<int> _intValueSet1 = new Lazy<int>(() => 1 + 1);
        public ClassWithPropertyValueSetBuilder WithIntValueSet1(int value) => WithIntValueSet1(() => value);
        public ClassWithPropertyValueSetBuilder WithIntValueSet1(Func<int> func)
        {
            _intValueSet1 = new Lazy<int>(func);
            _intValueSet1IsSet = true;
            return this;
        }
        public ClassWithPropertyValueSetBuilder WithoutIntValueSet1()
        {
            WithIntValueSet1(() => 1 + 1);
            _intValueSet1IsSet = false;
            return this;
        }

        private bool _intValueSet2IsSet;
        private Lazy<int> _intValueSet2 = new Lazy<int>(() => 2);
        public ClassWithPropertyValueSetBuilder WithIntValueSet2(int value) => WithIntValueSet2(() => value);
        public ClassWithPropertyValueSetBuilder WithIntValueSet2(Func<int> func)
        {
            _intValueSet2 = new Lazy<int>(func);
            _intValueSet2IsSet = true;
            return this;
        }
        public ClassWithPropertyValueSetBuilder WithoutIntValueSet2()
        {
            WithIntValueSet2(() => 2);
            _intValueSet2IsSet = false;
            return this;
        }

        private bool _localeIsSet;
        private Lazy<System.Globalization.CultureInfo> _locale = new Lazy<System.Globalization.CultureInfo>(() => CultureInfo.CurrentCulture);
        public ClassWithPropertyValueSetBuilder WithLocale(System.Globalization.CultureInfo value) => WithLocale(() => value);
        public ClassWithPropertyValueSetBuilder WithLocale(Func<System.Globalization.CultureInfo> func)
        {
            _locale = new Lazy<System.Globalization.CultureInfo>(func);
            _localeIsSet = true;
            return this;
        }
        public ClassWithPropertyValueSetBuilder WithoutLocale()
        {
            WithLocale(() => CultureInfo.CurrentCulture);
            _localeIsSet = false;
            return this;
        }

        private bool _locale2IsSet;
        private Lazy<System.Globalization.CultureInfo> _locale2 = new Lazy<System.Globalization.CultureInfo>(() => System.Globalization.CultureInfo.CurrentCulture);
        public ClassWithPropertyValueSetBuilder WithLocale2(System.Globalization.CultureInfo value) => WithLocale2(() => value);
        public ClassWithPropertyValueSetBuilder WithLocale2(Func<System.Globalization.CultureInfo> func)
        {
            _locale2 = new Lazy<System.Globalization.CultureInfo>(func);
            _locale2IsSet = true;
            return this;
        }
        public ClassWithPropertyValueSetBuilder WithoutLocale2()
        {
            WithLocale2(() => System.Globalization.CultureInfo.CurrentCulture);
            _locale2IsSet = false;
            return this;
        }

        private bool _locale3IsSet;
        private Lazy<System.Globalization.CultureInfo> _locale3 = new Lazy<System.Globalization.CultureInfo>(() => X.Value);
        public ClassWithPropertyValueSetBuilder WithLocale3(System.Globalization.CultureInfo value) => WithLocale3(() => value);
        public ClassWithPropertyValueSetBuilder WithLocale3(Func<System.Globalization.CultureInfo> func)
        {
            _locale3 = new Lazy<System.Globalization.CultureInfo>(func);
            _locale3IsSet = true;
            return this;
        }
        public ClassWithPropertyValueSetBuilder WithoutLocale3()
        {
            WithLocale3(() => X.Value);
            _locale3IsSet = false;
            return this;
        }

        private bool _locale4IsSet;
        private Lazy<System.Globalization.CultureInfo> _locale4 = new Lazy<System.Globalization.CultureInfo>(() => Y.Value);
        public ClassWithPropertyValueSetBuilder WithLocale4(System.Globalization.CultureInfo value) => WithLocale4(() => value);
        public ClassWithPropertyValueSetBuilder WithLocale4(Func<System.Globalization.CultureInfo> func)
        {
            _locale4 = new Lazy<System.Globalization.CultureInfo>(func);
            _locale4IsSet = true;
            return this;
        }
        public ClassWithPropertyValueSetBuilder WithoutLocale4()
        {
            WithLocale4(() => Y.Value);
            _locale4IsSet = false;
            return this;
        }

        private bool _locale5IsSet;
        private Lazy<System.Globalization.CultureInfo> _locale5 = new Lazy<System.Globalization.CultureInfo>(() => Z.Abc);
        public ClassWithPropertyValueSetBuilder WithLocale5(System.Globalization.CultureInfo value) => WithLocale5(() => value);
        public ClassWithPropertyValueSetBuilder WithLocale5(Func<System.Globalization.CultureInfo> func)
        {
            _locale5 = new Lazy<System.Globalization.CultureInfo>(func);
            _locale5IsSet = true;
            return this;
        }
        public ClassWithPropertyValueSetBuilder WithoutLocale5()
        {
            WithLocale5(() => Z.Abc);
            _locale5IsSet = false;
            return this;
        }

        private bool _suppressNullableWarningExpressionIsSet;
        private Lazy<string> _suppressNullableWarningExpression = new Lazy<string>(() => string.Empty);
        public ClassWithPropertyValueSetBuilder WithSuppressNullableWarningExpression(string value) => WithSuppressNullableWarningExpression(() => value);
        public ClassWithPropertyValueSetBuilder WithSuppressNullableWarningExpression(Func<string> func)
        {
            _suppressNullableWarningExpression = new Lazy<string>(func);
            _suppressNullableWarningExpressionIsSet = true;
            return this;
        }
        public ClassWithPropertyValueSetBuilder WithoutSuppressNullableWarningExpression()
        {
            WithSuppressNullableWarningExpression(() => string.Empty);
            _suppressNullableWarningExpressionIsSet = false;
            return this;
        }

        private bool _stringNullIsSet;
        private Lazy<string> _stringNull = new Lazy<string>(() => null);
        public ClassWithPropertyValueSetBuilder WithStringNull(string value) => WithStringNull(() => value);
        public ClassWithPropertyValueSetBuilder WithStringNull(Func<string> func)
        {
            _stringNull = new Lazy<string>(func);
            _stringNullIsSet = true;
            return this;
        }
        public ClassWithPropertyValueSetBuilder WithoutStringNull()
        {
            WithStringNull(() => null);
            _stringNullIsSet = false;
            return this;
        }

        private bool _stringEmptyIsSet;
        private Lazy<string> _stringEmpty = new Lazy<string>(() => string.Empty);
        public ClassWithPropertyValueSetBuilder WithStringEmpty(string value) => WithStringEmpty(() => value);
        public ClassWithPropertyValueSetBuilder WithStringEmpty(Func<string> func)
        {
            _stringEmpty = new Lazy<string>(func);
            _stringEmptyIsSet = true;
            return this;
        }
        public ClassWithPropertyValueSetBuilder WithoutStringEmpty()
        {
            WithStringEmpty(() => string.Empty);
            _stringEmptyIsSet = false;
            return this;
        }



        public override ClassWithPropertyValueSet Build(bool useObjectInitializer = true)
        {
            if (Object?.IsValueCreated != true)
            {
                Object = new Lazy<ClassWithPropertyValueSet>(() =>
                {
                    ClassWithPropertyValueSet instance;
                    if (useObjectInitializer)
                    {
                        instance = new ClassWithPropertyValueSet
                        {
                            NoIntValueSet = _noIntValueSet.Value,
                            IntValueSet1 = _intValueSet1.Value,
                            IntValueSet2 = _intValueSet2.Value,
                            Locale = _locale.Value,
                            Locale2 = _locale2.Value,
                            Locale3 = _locale3.Value,
                            Locale4 = _locale4.Value,
                            Locale5 = _locale5.Value,
                            SuppressNullableWarningExpression = _suppressNullableWarningExpression.Value,
                            StringNull = _stringNull.Value,
                            StringEmpty = _stringEmpty.Value
                        };

                        return instance;
                    }

                    instance = new ClassWithPropertyValueSet();
                    if (_noIntValueSetIsSet) { instance.NoIntValueSet = _noIntValueSet.Value; }
                    if (_intValueSet1IsSet) { instance.IntValueSet1 = _intValueSet1.Value; }
                    if (_intValueSet2IsSet) { instance.IntValueSet2 = _intValueSet2.Value; }
                    if (_localeIsSet) { instance.Locale = _locale.Value; }
                    if (_locale2IsSet) { instance.Locale2 = _locale2.Value; }
                    if (_locale3IsSet) { instance.Locale3 = _locale3.Value; }
                    if (_locale4IsSet) { instance.Locale4 = _locale4.Value; }
                    if (_locale5IsSet) { instance.Locale5 = _locale5.Value; }
                    if (_suppressNullableWarningExpressionIsSet) { instance.SuppressNullableWarningExpression = _suppressNullableWarningExpression.Value; }
                    if (_stringNullIsSet) { instance.StringNull = _stringNull.Value; }
                    if (_stringEmptyIsSet) { instance.StringEmpty = _stringEmpty.Value; }

                    return instance;
                });
            }

            PostBuild(Object.Value);

            return Object.Value;
        }

        public static ClassWithPropertyValueSet Default() => new ClassWithPropertyValueSet();

    }
}
#nullable disable