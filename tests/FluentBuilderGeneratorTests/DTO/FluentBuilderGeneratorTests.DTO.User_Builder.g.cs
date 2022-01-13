//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by https://github.com/StefH/FluentBuilder.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable enable
using System;
using FluentBuilder;
using FluentBuilderGeneratorTests.DTO;

namespace FluentBuilder
{
    public partial class UserBuilder : Builder<User>
    {
        private bool _firstNameIsSet;
        private Lazy<string> _firstName = new Lazy<string>(() => default(string));
        public UserBuilder WithFirstName(string value) => WithFirstName(() => value);
        public UserBuilder WithFirstName(Func<string> func)
        {
            _firstName = new Lazy<string>(func);
            _firstNameIsSet = true;
            return this;
        }
        public UserBuilder WithoutFirstName()
        {
            WithFirstName(() => default(string));
            _firstNameIsSet = false;
            return this;
        }

        private bool _lastNameIsSet;
        private Lazy<string> _lastName = new Lazy<string>(() => default(string));
        public UserBuilder WithLastName(string value) => WithLastName(() => value);
        public UserBuilder WithLastName(Func<string> func)
        {
            _lastName = new Lazy<string>(func);
            _lastNameIsSet = true;
            return this;
        }
        public UserBuilder WithoutLastName()
        {
            WithLastName(() => default(string));
            _lastNameIsSet = false;
            return this;
        }

        private bool _quitDateIsSet;
        private Lazy<DateTime?> _quitDate = new Lazy<DateTime?>(() => default(DateTime?));
        public UserBuilder WithQuitDate(DateTime? value) => WithQuitDate(() => value);
        public UserBuilder WithQuitDate(Func<DateTime?> func)
        {
            _quitDate = new Lazy<DateTime?>(func);
            _quitDateIsSet = true;
            return this;
        }
        public UserBuilder WithoutQuitDate()
        {
            WithQuitDate(() => default(DateTime?));
            _quitDateIsSet = false;
            return this;
        }


        public override User Build(bool useObjectInitializer = true)
        {
            if (Object?.IsValueCreated != true)
            {
                Object = new Lazy<User>(() =>
                {
                    if (typeof(User).GetConstructor(Type.EmptyTypes) is null)
                    {
                        throw new NotSupportedException(ErrorMessageConstructor);
                    }

                    if (useObjectInitializer)
                    {
                        return new User
                        {
                            FirstName = _firstName.Value,
                            LastName = _lastName.Value,
                            QuitDate = _quitDate.Value
                        };
                    }

                    var instance = new User();
                    if (_firstNameIsSet) { instance.FirstName = _firstName.Value; }
                    if (_lastNameIsSet) { instance.LastName = _lastName.Value; }
                    if (_quitDateIsSet) { instance.QuitDate = _quitDate.Value; }
                    return instance;
                }
            }

            PostBuild(Object.Value);

            return Object.Value;
        }

        public static User Default() => new User();

    }
}
#nullable disable