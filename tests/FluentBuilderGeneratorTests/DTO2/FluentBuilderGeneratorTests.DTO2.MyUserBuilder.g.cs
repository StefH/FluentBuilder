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

namespace FluentBuilderGeneratorTests.DTO2
{
    public partial class MyUserBuilder : Builder<FluentBuilderGeneratorTests.DTO.User>
    {
        private bool _firstNameIsSet;
        private Lazy<string> _firstName = new Lazy<string>(() => string.Empty);
        public MyUserBuilder WithFirstName(string value) => WithFirstName(() => value);
        public MyUserBuilder WithFirstName(Func<string> func)
        {
            _firstName = new Lazy<string>(func);
            _firstNameIsSet = true;
            return this;
        }
        private bool _lastNameIsSet;
        private Lazy<string> _lastName = new Lazy<string>(() => string.Empty);
        public MyUserBuilder WithLastName(string value) => WithLastName(() => value);
        public MyUserBuilder WithLastName(Func<string> func)
        {
            _lastName = new Lazy<string>(func);
            _lastNameIsSet = true;
            return this;
        }
        private bool _quitDateIsSet;
        private Lazy<System.DateTime?> _quitDate = new Lazy<System.DateTime?>(() => default(System.DateTime?));
        public MyUserBuilder WithQuitDate(System.DateTime? value) => WithQuitDate(() => value);
        public MyUserBuilder WithQuitDate(Func<System.DateTime?> func)
        {
            _quitDate = new Lazy<System.DateTime?>(func);
            _quitDateIsSet = true;
            return this;
        }
        private bool _testDummyClassIsSet;
        private Lazy<FluentBuilderGeneratorTests.DTO.DummyClass> _testDummyClass = new Lazy<FluentBuilderGeneratorTests.DTO.DummyClass>(() => new FluentBuilderGeneratorTests.DTO.DummyClass());
        public MyUserBuilder WithTestDummyClass(FluentBuilderGeneratorTests.DTO.DummyClass value) => WithTestDummyClass(() => value);
        public MyUserBuilder WithTestDummyClass(Func<FluentBuilderGeneratorTests.DTO.DummyClass> func)
        {
            _testDummyClass = new Lazy<FluentBuilderGeneratorTests.DTO.DummyClass>(func);
            _testDummyClassIsSet = true;
            return this;
        }
        private bool _optionsIsSet;
        private Lazy<System.Collections.Generic.List<FluentBuilderGeneratorTests.DTO.Option>> _options = new Lazy<System.Collections.Generic.List<FluentBuilderGeneratorTests.DTO.Option>>(() => new List<FluentBuilderGeneratorTests.DTO.Option>());
        public MyUserBuilder WithOptions(System.Collections.Generic.List<FluentBuilderGeneratorTests.DTO.Option> value) => WithOptions(() => value);
        public MyUserBuilder WithOptions(Func<System.Collections.Generic.List<FluentBuilderGeneratorTests.DTO.Option>> func)
        {
            _options = new Lazy<System.Collections.Generic.List<FluentBuilderGeneratorTests.DTO.Option>>(func);
            _optionsIsSet = true;
            return this;
        }
        public MyUserBuilder WithOptions(Action<FluentBuilderGeneratorTests.DTO.IListOptionBuilder> action, bool useObjectInitializer = true) => WithOptions(() =>
        {
            var builder = new FluentBuilderGeneratorTests.DTO.IListOptionBuilder();
            action(builder);
            return (System.Collections.Generic.List<FluentBuilderGeneratorTests.DTO.Option>) builder.Build(useObjectInitializer);
        });

        private bool _Constructor_1436654309_IsSet;
        private Lazy<FluentBuilderGeneratorTests.DTO.User> _Constructor_1436654309 = new Lazy<FluentBuilderGeneratorTests.DTO.User>(() => new FluentBuilderGeneratorTests.DTO.User());
        public MyUserBuilder UsingConstructor()
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


        public MyUserBuilder UsingInstance(User value) => UsingInstance(() => value);

        public MyUserBuilder UsingInstance(Func<User> func)
        {
            Instance = new Lazy<User>(func);
            return this;
        }

        public override User Build() => Build(true);

        public override User Build(bool useObjectInitializer)
        {
            if (Instance is null)
            {
                Instance = new Lazy<User>(() =>
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

                    return instance;
                });
            }

            if (_firstNameIsSet) { Instance.Value.FirstName = _firstName.Value; }
            if (_lastNameIsSet) { Instance.Value.LastName = _lastName.Value; }
            if (_quitDateIsSet) { Instance.Value.QuitDate = _quitDate.Value; }
            if (_testDummyClassIsSet) { Instance.Value.TestDummyClass = _testDummyClass.Value; }
            if (_optionsIsSet) { Instance.Value.Options = _options.Value; }

            PostBuild(Instance.Value);

            return Instance.Value;
        }

        public static User Default() => new User();

    }
}
#nullable disable