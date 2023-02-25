//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by https://github.com/StefH/FluentBuilder version 0.7.1.0
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
    public partial class UserBuilder : Builder<FluentBuilderGeneratorTests.DTO.User>
    {
        private bool _firstNameIsSet;
        private Lazy<string> _firstName = new Lazy<string>(() => string.Empty);
        public UserBuilder WithFirstName(string value) => WithFirstName(() => value);
        public UserBuilder WithFirstName(Func<string> func)
        {
            _firstName = new Lazy<string>(func);
            _firstNameIsSet = true;
            return this;
        }
        public UserBuilder WithoutFirstName()
        {
            WithFirstName(() => string.Empty);
            _firstNameIsSet = false;
            return this;
        }

        private bool _lastNameIsSet;
        private Lazy<string> _lastName = new Lazy<string>(() => string.Empty);
        public UserBuilder WithLastName(string value) => WithLastName(() => value);
        public UserBuilder WithLastName(Func<string> func)
        {
            _lastName = new Lazy<string>(func);
            _lastNameIsSet = true;
            return this;
        }
        public UserBuilder WithoutLastName()
        {
            WithLastName(() => string.Empty);
            _lastNameIsSet = false;
            return this;
        }

        private bool _quitDateIsSet;
        private Lazy<System.DateTime?> _quitDate = new Lazy<System.DateTime?>(() => default(System.DateTime?));
        public UserBuilder WithQuitDate(System.DateTime? value) => WithQuitDate(() => value);
        public UserBuilder WithQuitDate(Func<System.DateTime?> func)
        {
            _quitDate = new Lazy<System.DateTime?>(func);
            _quitDateIsSet = true;
            return this;
        }
        public UserBuilder WithoutQuitDate()
        {
            WithQuitDate(() => default(System.DateTime?));
            _quitDateIsSet = false;
            return this;
        }

        private bool _testDummyClassIsSet;
        private Lazy<FluentBuilderGeneratorTests.DTO.DummyClass> _testDummyClass = new Lazy<FluentBuilderGeneratorTests.DTO.DummyClass>(() => new FluentBuilderGeneratorTests.DTO.DummyClass());
        public UserBuilder WithTestDummyClass(FluentBuilderGeneratorTests.DTO.DummyClass value) => WithTestDummyClass(() => value);
        public UserBuilder WithTestDummyClass(Func<FluentBuilderGeneratorTests.DTO.DummyClass> func)
        {
            _testDummyClass = new Lazy<FluentBuilderGeneratorTests.DTO.DummyClass>(func);
            _testDummyClassIsSet = true;
            return this;
        }
        public UserBuilder WithTestDummyClass(Action<FluentBuilderGeneratorTests.DTO.MyDummyClassBuilder> action, bool useObjectInitializer = true) => WithTestDummyClass(() =>
        {
            var builder = new FluentBuilderGeneratorTests.DTO.MyDummyClassBuilder();
            action(builder);
            return builder.Build(useObjectInitializer);
        });
        public UserBuilder WithoutTestDummyClass()
        {
            WithTestDummyClass(() => new FluentBuilderGeneratorTests.DTO.DummyClass());
            _testDummyClassIsSet = false;
            return this;
        }

        private bool _optionsIsSet;
        private Lazy<System.Collections.Generic.List<FluentBuilderGeneratorTests.DTO.Option>> _options = new Lazy<System.Collections.Generic.List<FluentBuilderGeneratorTests.DTO.Option>>(() => new List<FluentBuilderGeneratorTests.DTO.Option>());
        public UserBuilder WithOptions(System.Collections.Generic.List<FluentBuilderGeneratorTests.DTO.Option> value) => WithOptions(() => value);
        public UserBuilder WithOptions(Func<System.Collections.Generic.List<FluentBuilderGeneratorTests.DTO.Option>> func)
        {
            _options = new Lazy<System.Collections.Generic.List<FluentBuilderGeneratorTests.DTO.Option>>(func);
            _optionsIsSet = true;
            return this;
        }
        public UserBuilder WithOptions(Action<FluentBuilderGeneratorTests.FluentBuilder.IListBuilder<FluentBuilderGeneratorTests.DTO.Option>> action, bool useObjectInitializer = true) => WithOptions(() =>
        {
            var builder = new FluentBuilderGeneratorTests.FluentBuilder.IListBuilder<FluentBuilderGeneratorTests.DTO.Option>();
            action(builder);
            return (System.Collections.Generic.List<FluentBuilderGeneratorTests.DTO.Option>) builder.Build(useObjectInitializer);
        });
        public UserBuilder WithoutOptions()
        {
            WithOptions(() => new List<FluentBuilderGeneratorTests.DTO.Option>());
            _optionsIsSet = false;
            return this;
        }


        private bool _Constructor_1436654309_IsSet;
        private Lazy<FluentBuilderGeneratorTests.DTO.User> _Constructor_1436654309 = new Lazy<FluentBuilderGeneratorTests.DTO.User>(() => new FluentBuilderGeneratorTests.DTO.User());
        public UserBuilder WithConstructor()
        {
            _Constructor_1436654309 = new Lazy<FluentBuilderGeneratorTests.DTO.User>(() =>
            {
                return new FluentBuilderGeneratorTests.DTO.User
                (

                );
            });
            _Constructor_1436654309_IsSet = true;

            return this;
        }


        public override User Build() => Build(true);

        public override User Build(bool useObjectInitializer)
        {
            if (Object?.IsValueCreated != true)
            {
                Object = new Lazy<User>(() =>
                {
                    User instance;
                    if (useObjectInitializer)
                    {
                        instance = new User
                        {
                            FirstName = _firstName.Value,
                            LastName = _lastName.Value,
                            QuitDate = _quitDate.Value,
                            TestDummyClass = _testDummyClass.Value,
                            Options = _options.Value
                        };

                        return instance;
                    }

                    if (_Constructor_1436654309_IsSet) { instance = _Constructor_1436654309.Value; }
                    else { instance = Default(); }

                    if (_firstNameIsSet) { instance.FirstName = _firstName.Value; }
                    if (_lastNameIsSet) { instance.LastName = _lastName.Value; }
                    if (_quitDateIsSet) { instance.QuitDate = _quitDate.Value; }
                    if (_testDummyClassIsSet) { instance.TestDummyClass = _testDummyClass.Value; }
                    if (_optionsIsSet) { instance.Options = _options.Value; }

                    return instance;
                });
            }

            PostBuild(Object.Value);

            return Object.Value;
        }

        public static User Default() => new User();

    }
}
#nullable disable