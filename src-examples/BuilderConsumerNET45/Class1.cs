//using System.Collections.Generic;
//using System;

//namespace BuilderConsumer
//{
//    public class EmailDto
//    {
//        public string Value { get; set; }
//    }
//}

//namespace BuilderConsumer
//{
//    public class UserDto
//    {
//        public int Age { get; set; }

//        public string FirstName { get; set; }

//        public string LastName { get; set; }

//        public EmailDto PrimaryEmail { get; set; }

//        public IEnumerable<EmailDto> SecondaryEmails { get; set; }

//        public DateTime? QuitDate { get; set; }
//    }
//}

//namespace FluentBuilder
//{
//    public abstract class Builder<T> where T : class
//    {
//        protected Lazy<T> Object;

//        public abstract T Build();

//        public Builder<T> WithObject(T value) => WithObject(() => value);

//        public Builder<T> WithObject(Func<T> func)
//        {
//            Object = new Lazy<T>(func);
//            return this;
//        }

//        protected virtual void PostBuild(T value) { }
//    }
//}

//namespace FluentBuilder
//{
//    public partial class EmailDtoBuilder : Builder<BuilderConsumer.EmailDto>
//    {
//        private Lazy<string> _value = new Lazy<string>(() => default(string));
//        public EmailDtoBuilder WithValue(string value) => WithValue(() => value);
//        public EmailDtoBuilder WithValue(Func<string> func)
//        {
//            _value = new Lazy<string>(func);
//            return this;
//        }
//        public EmailDtoBuilder WithoutValue() => WithValue(() => default(string));


//        public override BuilderConsumer.EmailDto Build()
//        {
//            if (Object?.IsValueCreated != true)
//            {
//                Object = new Lazy<BuilderConsumer.EmailDto>(() => new BuilderConsumer.EmailDto
//                {
//                    Value = _value.Value
//                });
//            }

//            PostBuild(Object.Value);

//            return Object.Value;
//        }

//        public static BuilderConsumer.EmailDto Default() => new BuilderConsumer.EmailDto();

//    }
//}


//namespace FluentBuilder
//{
//    public partial class UserDtoBuilder : Builder<BuilderConsumer.UserDto>
//    {
//        private Lazy<int> _age = new Lazy<int>(() => default(int));
//        public UserDtoBuilder WithAge(int value) => WithAge(() => value);
//        public UserDtoBuilder WithAge(Func<int> func)
//        {
//            _age = new Lazy<int>(func);
//            return this;
//        }
//        public UserDtoBuilder WithoutAge() => WithAge(() => default(int));

//        private Lazy<string> _firstName = new Lazy<string>(() => default(string));
//        public UserDtoBuilder WithFirstName(string value) => WithFirstName(() => value);
//        public UserDtoBuilder WithFirstName(Func<string> func)
//        {
//            _firstName = new Lazy<string>(func);
//            return this;
//        }
//        public UserDtoBuilder WithoutFirstName() => WithFirstName(() => default(string));

//        private Lazy<string> _lastName = new Lazy<string>(() => default(string));
//        public UserDtoBuilder WithLastName(string value) => WithLastName(() => value);
//        public UserDtoBuilder WithLastName(Func<string> func)
//        {
//            _lastName = new Lazy<string>(func);
//            return this;
//        }
//        public UserDtoBuilder WithoutLastName() => WithLastName(() => default(string));

//        private Lazy<BuilderConsumer.EmailDto> _primaryEmail = new Lazy<BuilderConsumer.EmailDto>(() => default(BuilderConsumer.EmailDto));
//        public UserDtoBuilder WithPrimaryEmail(BuilderConsumer.EmailDto value) => WithPrimaryEmail(() => value);
//        public UserDtoBuilder WithPrimaryEmail(Func<BuilderConsumer.EmailDto> func)
//        {
//            _primaryEmail = new Lazy<BuilderConsumer.EmailDto>(func);
//            return this;
//        }
//        public UserDtoBuilder WithPrimaryEmail(Action<FluentBuilder.EmailDtoBuilder> action) => WithPrimaryEmail(() =>
//        {
//            var builder = new FluentBuilder.EmailDtoBuilder();
//            action(builder);
//            return builder.Build();
//        });
//        public UserDtoBuilder WithoutPrimaryEmail() => WithPrimaryEmail(() => default(BuilderConsumer.EmailDto));

//        private Lazy<System.Collections.Generic.IEnumerable<BuilderConsumer.EmailDto>> _secondaryEmails = new Lazy<System.Collections.Generic.IEnumerable<BuilderConsumer.EmailDto>>(() => default(System.Collections.Generic.IEnumerable<BuilderConsumer.EmailDto>));
//        public UserDtoBuilder WithSecondaryEmails(System.Collections.Generic.IEnumerable<BuilderConsumer.EmailDto> value) => WithSecondaryEmails(() => value);
//        public UserDtoBuilder WithSecondaryEmails(Func<System.Collections.Generic.IEnumerable<BuilderConsumer.EmailDto>> func)
//        {
//            _secondaryEmails = new Lazy<System.Collections.Generic.IEnumerable<BuilderConsumer.EmailDto>>(func);
//            return this;
//        }
//        public UserDtoBuilder WithoutSecondaryEmails() => WithSecondaryEmails(() => default(System.Collections.Generic.IEnumerable<BuilderConsumer.EmailDto>));

//        private Lazy<System.DateTime?> _quitDate = new Lazy<System.DateTime?>(() => default(System.DateTime?));
//        public UserDtoBuilder WithQuitDate(System.DateTime? value) => WithQuitDate(() => value);
//        public UserDtoBuilder WithQuitDate(Func<System.DateTime?> func)
//        {
//            _quitDate = new Lazy<System.DateTime?>(func);
//            return this;
//        }
//        public UserDtoBuilder WithoutQuitDate() => WithQuitDate(() => default(System.DateTime?));


//        public override BuilderConsumer.UserDto Build()
//        {
//            if (Object?.IsValueCreated != true)
//            {
//                Object = new Lazy<BuilderConsumer.UserDto>(() => new BuilderConsumer.UserDto
//                {
//                    Age = _age.Value,
//                    FirstName = _firstName.Value,
//                    LastName = _lastName.Value,
//                    PrimaryEmail = _primaryEmail.Value,
//                    SecondaryEmails = _secondaryEmails.Value,
//                    QuitDate = _quitDate.Value
//                });
//            }

//            PostBuild(Object.Value);

//            return Object.Value;
//        }

//        public static BuilderConsumer.UserDto Default() => new BuilderConsumer.UserDto();

//    }
//}