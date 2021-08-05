using System;
using FluentBuilder;
using FluentBuilderGeneratorTests.DTO;

namespace FluentBuilder
{
    public partial class UserBuilder : Builder<User>
    {
        private Lazy<string> _firstName = new Lazy<string>(() => default(string));
        public UserBuilder WithFirstName(string value) => WithFirstName(() => value);
        public UserBuilder WithFirstName(Func<string> func)
        {
            _firstName = new Lazy<string>(func);
            return this;
        }
        public UserBuilder WithoutFirstName() => WithFirstName(() => default(string));

        private Lazy<string> _lastName = new Lazy<string>(() => default(string));
        public UserBuilder WithLastName(string value) => WithLastName(() => value);
        public UserBuilder WithLastName(Func<string> func)
        {
            _lastName = new Lazy<string>(func);
            return this;
        }
        public UserBuilder WithoutLastName() => WithLastName(() => default(string));

        private Lazy<DateTime?> _quitDate = new Lazy<DateTime?>(() => default(DateTime?));
        public UserBuilder WithQuitDate(DateTime? value) => WithQuitDate(() => value);
        public UserBuilder WithQuitDate(Func<DateTime?> func)
        {
            _quitDate = new Lazy<DateTime?>(func);
            return this;
        }
        public UserBuilder WithoutQuitDate() => WithQuitDate(() => default(DateTime?));


        public override User Build()
        {
            if (Object?.IsValueCreated != true)
            {
                Object = new Lazy<User>(() => new User
                {
                    FirstName = _firstName.Value,
                    LastName = _lastName.Value,
                    QuitDate = _quitDate.Value
                });
            }

            PostBuild(Object.Value);

            return Object.Value;
        }

        public static User Default() => new User();

    }
}