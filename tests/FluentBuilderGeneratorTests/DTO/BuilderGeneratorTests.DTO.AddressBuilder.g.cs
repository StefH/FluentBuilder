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
    public static partial class AddressBuilderExtensions
    {
        public static AddressBuilder AsBuilder(this FluentBuilderGeneratorTests.DTO.Address instance)
        {
            return new AddressBuilder().UsingInstance(instance);
        }
    }

    public partial class AddressBuilder : Builder<FluentBuilderGeneratorTests.DTO.Address>
    {
        private bool _houseNumberIsSet;
        private Lazy<int> _houseNumber = new Lazy<int>(() => default(int));
        public AddressBuilder WithHouseNumber(int value) => WithHouseNumber(() => value);
        public AddressBuilder WithHouseNumber(Func<int> func)
        {
            _houseNumber = new Lazy<int>(func);
            _houseNumberIsSet = true;
            return this;
        }
        private bool _cityIsSet;
        private Lazy<string?> _city = new Lazy<string?>(() => default(string?));
        public AddressBuilder WithCity(string? value) => WithCity(() => value);
        public AddressBuilder WithCity(Func<string?> func)
        {
            _city = new Lazy<string?>(func);
            _cityIsSet = true;
            return this;
        }
        private bool _arrayIsSet;
        private Lazy<string[]> _array = new Lazy<string[]>(() => new string[0]);
        public AddressBuilder WithArray(params string[] value) => WithArray(() => value);
        public AddressBuilder WithArray(Func<string[]> func)
        {
            _array = new Lazy<string[]>(func);
            _arrayIsSet = true;
            return this;
        }
        public AddressBuilder WithArray(Action<FluentBuilderGeneratorTests.FluentBuilder.ArrayBuilder<string>> action, bool useObjectInitializer = true) => WithArray(() =>
        {
            var builder = new FluentBuilderGeneratorTests.FluentBuilder.ArrayBuilder<string>();
            action(builder);
            return builder.Build(useObjectInitializer);
        });
        private bool _array2IsSet;
        private Lazy<FluentBuilderGeneratorTests.DTO.Address[]> _array2 = new Lazy<FluentBuilderGeneratorTests.DTO.Address[]>(() => new FluentBuilderGeneratorTests.DTO.Address[0]);
        public AddressBuilder WithArray2(params FluentBuilderGeneratorTests.DTO.Address[] value) => WithArray2(() => value);
        public AddressBuilder WithArray2(Func<FluentBuilderGeneratorTests.DTO.Address[]> func)
        {
            _array2 = new Lazy<FluentBuilderGeneratorTests.DTO.Address[]>(func);
            _array2IsSet = true;
            return this;
        }
        public AddressBuilder WithArray2(Action<FluentBuilderGeneratorTests.DTO.ArrayAddressBuilder> action, bool useObjectInitializer = true) => WithArray2(() =>
        {
            var builder = new FluentBuilderGeneratorTests.DTO.ArrayAddressBuilder();
            action(builder);
            return builder.Build(useObjectInitializer);
        });
        private bool _thingUsingConstructorWithItselfIsSet;
        private Lazy<FluentBuilderGeneratorTests.DTO.ThingUsingConstructorWithItself> _thingUsingConstructorWithItself = new Lazy<FluentBuilderGeneratorTests.DTO.ThingUsingConstructorWithItself>(() => new FluentBuilderGeneratorTests.DTO.ThingUsingConstructorWithItself(string.Empty, string.Empty));
        public AddressBuilder WithThingUsingConstructorWithItself(FluentBuilderGeneratorTests.DTO.ThingUsingConstructorWithItself value) => WithThingUsingConstructorWithItself(() => value);
        public AddressBuilder WithThingUsingConstructorWithItself(Func<FluentBuilderGeneratorTests.DTO.ThingUsingConstructorWithItself> func)
        {
            _thingUsingConstructorWithItself = new Lazy<FluentBuilderGeneratorTests.DTO.ThingUsingConstructorWithItself>(func);
            _thingUsingConstructorWithItselfIsSet = true;
            return this;
        }
        private bool _thingUsingConstructorWith2ParametersIsSet;
        private Lazy<FluentBuilderGeneratorTests.DTO.ThingUsingConstructorWith2Parameters> _thingUsingConstructorWith2Parameters = new Lazy<FluentBuilderGeneratorTests.DTO.ThingUsingConstructorWith2Parameters>(() => new FluentBuilderGeneratorTests.DTO.ThingUsingConstructorWith2Parameters(default(int), default(int)));
        public AddressBuilder WithThingUsingConstructorWith2Parameters(FluentBuilderGeneratorTests.DTO.ThingUsingConstructorWith2Parameters value) => WithThingUsingConstructorWith2Parameters(() => value);
        public AddressBuilder WithThingUsingConstructorWith2Parameters(Func<FluentBuilderGeneratorTests.DTO.ThingUsingConstructorWith2Parameters> func)
        {
            _thingUsingConstructorWith2Parameters = new Lazy<FluentBuilderGeneratorTests.DTO.ThingUsingConstructorWith2Parameters>(func);
            _thingUsingConstructorWith2ParametersIsSet = true;
            return this;
        }
        private bool _thingWithoutDefaultConstructorIsSet;
        private Lazy<FluentBuilderGeneratorTests.DTO.ThingWithoutDefaultConstructor> _thingWithoutDefaultConstructor = new Lazy<FluentBuilderGeneratorTests.DTO.ThingWithoutDefaultConstructor>(() => new FluentBuilderGeneratorTests.DTO.ThingWithoutDefaultConstructor(default(int)));
        public AddressBuilder WithThingWithoutDefaultConstructor(FluentBuilderGeneratorTests.DTO.ThingWithoutDefaultConstructor value) => WithThingWithoutDefaultConstructor(() => value);
        public AddressBuilder WithThingWithoutDefaultConstructor(Func<FluentBuilderGeneratorTests.DTO.ThingWithoutDefaultConstructor> func)
        {
            _thingWithoutDefaultConstructor = new Lazy<FluentBuilderGeneratorTests.DTO.ThingWithoutDefaultConstructor>(func);
            _thingWithoutDefaultConstructorIsSet = true;
            return this;
        }
        private bool _thingWithPrivateConstructorIsSet;
        private Lazy<FluentBuilderGeneratorTests.DTO.ThingWithPrivateConstructor> _thingWithPrivateConstructor = new Lazy<FluentBuilderGeneratorTests.DTO.ThingWithPrivateConstructor>(() => default(FluentBuilderGeneratorTests.DTO.ThingWithPrivateConstructor)!);
        public AddressBuilder WithThingWithPrivateConstructor(FluentBuilderGeneratorTests.DTO.ThingWithPrivateConstructor value) => WithThingWithPrivateConstructor(() => value);
        public AddressBuilder WithThingWithPrivateConstructor(Func<FluentBuilderGeneratorTests.DTO.ThingWithPrivateConstructor> func)
        {
            _thingWithPrivateConstructor = new Lazy<FluentBuilderGeneratorTests.DTO.ThingWithPrivateConstructor>(func);
            _thingWithPrivateConstructorIsSet = true;
            return this;
        }
        private bool _thingIsSet;
        private Lazy<FluentBuilderGeneratorTests.DTO.Thing> _thing = new Lazy<FluentBuilderGeneratorTests.DTO.Thing>(() => new FluentBuilderGeneratorTests.DTO.Thing());
        public AddressBuilder WithThing(FluentBuilderGeneratorTests.DTO.Thing value) => WithThing(() => value);
        public AddressBuilder WithThing(Func<FluentBuilderGeneratorTests.DTO.Thing> func)
        {
            _thing = new Lazy<FluentBuilderGeneratorTests.DTO.Thing>(func);
            _thingIsSet = true;
            return this;
        }
        private bool _thingIsStructIsSet;
        private Lazy<FluentBuilderGeneratorTests.DTO.ThingIsStruct> _thingIsStruct = new Lazy<FluentBuilderGeneratorTests.DTO.ThingIsStruct>(() => default(FluentBuilderGeneratorTests.DTO.ThingIsStruct));
        public AddressBuilder WithThingIsStruct(FluentBuilderGeneratorTests.DTO.ThingIsStruct value) => WithThingIsStruct(() => value);
        public AddressBuilder WithThingIsStruct(Func<FluentBuilderGeneratorTests.DTO.ThingIsStruct> func)
        {
            _thingIsStruct = new Lazy<FluentBuilderGeneratorTests.DTO.ThingIsStruct>(func);
            _thingIsStructIsSet = true;
            return this;
        }
        private bool _iReadOnlyCollectionIsSet;
        private Lazy<System.Collections.Generic.IReadOnlyCollection<string>> _iReadOnlyCollection = new Lazy<System.Collections.Generic.IReadOnlyCollection<string>>(() => new List<string>());
        public AddressBuilder WithIReadOnlyCollection(System.Collections.Generic.IReadOnlyCollection<string> value) => WithIReadOnlyCollection(() => value);
        public AddressBuilder WithIReadOnlyCollection(Func<System.Collections.Generic.IReadOnlyCollection<string>> func)
        {
            _iReadOnlyCollection = new Lazy<System.Collections.Generic.IReadOnlyCollection<string>>(func);
            _iReadOnlyCollectionIsSet = true;
            return this;
        }
        private bool _readOnlyCollectionIsSet;
        private Lazy<System.Collections.ObjectModel.ReadOnlyCollection<long>> _readOnlyCollection = new Lazy<System.Collections.ObjectModel.ReadOnlyCollection<long>>(() => new System.Collections.ObjectModel.ReadOnlyCollection<long>(new List<long>()));
        public AddressBuilder WithReadOnlyCollection(System.Collections.ObjectModel.ReadOnlyCollection<long> value) => WithReadOnlyCollection(() => value);
        public AddressBuilder WithReadOnlyCollection(Func<System.Collections.ObjectModel.ReadOnlyCollection<long>> func)
        {
            _readOnlyCollection = new Lazy<System.Collections.ObjectModel.ReadOnlyCollection<long>>(func);
            _readOnlyCollectionIsSet = true;
            return this;
        }
        private bool _enumerableIsSet;
        private Lazy<System.Collections.Generic.IEnumerable<byte>> _enumerable = new Lazy<System.Collections.Generic.IEnumerable<byte>>(() => new byte[0]);
        public AddressBuilder WithEnumerable(System.Collections.Generic.IEnumerable<byte> value) => WithEnumerable(() => value);
        public AddressBuilder WithEnumerable(Func<System.Collections.Generic.IEnumerable<byte>> func)
        {
            _enumerable = new Lazy<System.Collections.Generic.IEnumerable<byte>>(func);
            _enumerableIsSet = true;
            return this;
        }
        public AddressBuilder WithEnumerable(Action<FluentBuilderGeneratorTests.FluentBuilder.IEnumerableBuilder<byte>> action, bool useObjectInitializer = true) => WithEnumerable(() =>
        {
            var builder = new FluentBuilderGeneratorTests.FluentBuilder.IEnumerableBuilder<byte>();
            action(builder);
            return builder.Build(useObjectInitializer);
        });
        private bool _enumerable2IsSet;
        private Lazy<System.Collections.Generic.IEnumerable<FluentBuilderGeneratorTests.DTO.Address>> _enumerable2 = new Lazy<System.Collections.Generic.IEnumerable<FluentBuilderGeneratorTests.DTO.Address>>(() => new FluentBuilderGeneratorTests.DTO.Address[0]);
        public AddressBuilder WithEnumerable2(System.Collections.Generic.IEnumerable<FluentBuilderGeneratorTests.DTO.Address> value) => WithEnumerable2(() => value);
        public AddressBuilder WithEnumerable2(Func<System.Collections.Generic.IEnumerable<FluentBuilderGeneratorTests.DTO.Address>> func)
        {
            _enumerable2 = new Lazy<System.Collections.Generic.IEnumerable<FluentBuilderGeneratorTests.DTO.Address>>(func);
            _enumerable2IsSet = true;
            return this;
        }
        public AddressBuilder WithEnumerable2(Action<FluentBuilderGeneratorTests.DTO.IEnumerableAddressBuilder> action, bool useObjectInitializer = true) => WithEnumerable2(() =>
        {
            var builder = new FluentBuilderGeneratorTests.DTO.IEnumerableAddressBuilder();
            action(builder);
            return builder.Build(useObjectInitializer);
        });
        private bool _listIsSet;
        private Lazy<System.Collections.Generic.List<string>> _list = new Lazy<System.Collections.Generic.List<string>>(() => new List<string>());
        public AddressBuilder WithList(System.Collections.Generic.List<string> value) => WithList(() => value);
        public AddressBuilder WithList(Func<System.Collections.Generic.List<string>> func)
        {
            _list = new Lazy<System.Collections.Generic.List<string>>(func);
            _listIsSet = true;
            return this;
        }
        public AddressBuilder WithList(Action<FluentBuilderGeneratorTests.FluentBuilder.IListBuilder<string>> action, bool useObjectInitializer = true) => WithList(() =>
        {
            var builder = new FluentBuilderGeneratorTests.FluentBuilder.IListBuilder<string>();
            action(builder);
            return (System.Collections.Generic.List<string>) builder.Build(useObjectInitializer);
        });
        private bool _list2IsSet;
        private Lazy<System.Collections.Generic.List<FluentBuilderGeneratorTests.DTO.Address>> _list2 = new Lazy<System.Collections.Generic.List<FluentBuilderGeneratorTests.DTO.Address>>(() => new List<FluentBuilderGeneratorTests.DTO.Address>());
        public AddressBuilder WithList2(System.Collections.Generic.List<FluentBuilderGeneratorTests.DTO.Address> value) => WithList2(() => value);
        public AddressBuilder WithList2(Func<System.Collections.Generic.List<FluentBuilderGeneratorTests.DTO.Address>> func)
        {
            _list2 = new Lazy<System.Collections.Generic.List<FluentBuilderGeneratorTests.DTO.Address>>(func);
            _list2IsSet = true;
            return this;
        }
        public AddressBuilder WithList2(Action<FluentBuilderGeneratorTests.DTO.IListAddressBuilder> action, bool useObjectInitializer = true) => WithList2(() =>
        {
            var builder = new FluentBuilderGeneratorTests.DTO.IListAddressBuilder();
            action(builder);
            return (System.Collections.Generic.List<FluentBuilderGeneratorTests.DTO.Address>) builder.Build(useObjectInitializer);
        });
        private bool _collectionIsSet;
        private Lazy<System.Collections.Generic.ICollection<long>> _collection = new Lazy<System.Collections.Generic.ICollection<long>>(() => new List<long>());
        public AddressBuilder WithCollection(System.Collections.Generic.ICollection<long> value) => WithCollection(() => value);
        public AddressBuilder WithCollection(Func<System.Collections.Generic.ICollection<long>> func)
        {
            _collection = new Lazy<System.Collections.Generic.ICollection<long>>(func);
            _collectionIsSet = true;
            return this;
        }
        public AddressBuilder WithCollection(Action<FluentBuilderGeneratorTests.FluentBuilder.ICollectionBuilder<long>> action, bool useObjectInitializer = true) => WithCollection(() =>
        {
            var builder = new FluentBuilderGeneratorTests.FluentBuilder.ICollectionBuilder<long>();
            action(builder);
            return builder.Build(useObjectInitializer);
        });
        private bool _collection2IsSet;
        private Lazy<System.Collections.Generic.ICollection<FluentBuilderGeneratorTests.DTO.Address>> _collection2 = new Lazy<System.Collections.Generic.ICollection<FluentBuilderGeneratorTests.DTO.Address>>(() => new List<FluentBuilderGeneratorTests.DTO.Address>());
        public AddressBuilder WithCollection2(System.Collections.Generic.ICollection<FluentBuilderGeneratorTests.DTO.Address> value) => WithCollection2(() => value);
        public AddressBuilder WithCollection2(Func<System.Collections.Generic.ICollection<FluentBuilderGeneratorTests.DTO.Address>> func)
        {
            _collection2 = new Lazy<System.Collections.Generic.ICollection<FluentBuilderGeneratorTests.DTO.Address>>(func);
            _collection2IsSet = true;
            return this;
        }
        public AddressBuilder WithCollection2(Action<FluentBuilderGeneratorTests.DTO.ICollectionAddressBuilder> action, bool useObjectInitializer = true) => WithCollection2(() =>
        {
            var builder = new FluentBuilderGeneratorTests.DTO.ICollectionAddressBuilder();
            action(builder);
            return builder.Build(useObjectInitializer);
        });
        private bool _iDictionaryIsSet;
        private Lazy<System.Collections.IDictionary> _iDictionary = new Lazy<System.Collections.IDictionary>(() => new Dictionary<object, object>());
        public AddressBuilder WithIDictionary(System.Collections.IDictionary value) => WithIDictionary(() => value);
        public AddressBuilder WithIDictionary(Func<System.Collections.IDictionary> func)
        {
            _iDictionary = new Lazy<System.Collections.IDictionary>(func);
            _iDictionaryIsSet = true;
            return this;
        }
        private bool _iDictionary2IsSet;
        private Lazy<System.Collections.Generic.IDictionary<string, int>> _iDictionary2 = new Lazy<System.Collections.Generic.IDictionary<string, int>>(() => new Dictionary<string, int>());
        public AddressBuilder WithIDictionary2(System.Collections.Generic.IDictionary<string, int> value) => WithIDictionary2(() => value);
        public AddressBuilder WithIDictionary2(Func<System.Collections.Generic.IDictionary<string, int>> func)
        {
            _iDictionary2 = new Lazy<System.Collections.Generic.IDictionary<string, int>>(func);
            _iDictionary2IsSet = true;
            return this;
        }
        public AddressBuilder WithIDictionary2(Action<FluentBuilderGeneratorTests.FluentBuilder.IDictionaryBuilder<string, int>> action, bool useObjectInitializer = true) => WithIDictionary2(() =>
        {
            var builder = new FluentBuilderGeneratorTests.FluentBuilder.IDictionaryBuilder<string, int>();
            action(builder);
            return builder.Build(useObjectInitializer);
        });
        private bool _dictionary2IsSet;
        private Lazy<System.Collections.Generic.Dictionary<string, int>> _dictionary2 = new Lazy<System.Collections.Generic.Dictionary<string, int>>(() => new Dictionary<string, int>());
        public AddressBuilder WithDictionary2(System.Collections.Generic.Dictionary<string, int> value) => WithDictionary2(() => value);
        public AddressBuilder WithDictionary2(Func<System.Collections.Generic.Dictionary<string, int>> func)
        {
            _dictionary2 = new Lazy<System.Collections.Generic.Dictionary<string, int>>(func);
            _dictionary2IsSet = true;
            return this;
        }
        public AddressBuilder WithDictionary2(Action<FluentBuilderGeneratorTests.FluentBuilder.IDictionaryBuilder<string, int>> action, bool useObjectInitializer = true) => WithDictionary2(() =>
        {
            var builder = new FluentBuilderGeneratorTests.FluentBuilder.IDictionaryBuilder<string, int>();
            action(builder);
            return (System.Collections.Generic.Dictionary<string, int>) builder.Build(useObjectInitializer);
        });

        private bool _Constructor_1362952513_IsSet;
        private Lazy<FluentBuilderGeneratorTests.DTO.Address> _Constructor_1362952513 = new Lazy<FluentBuilderGeneratorTests.DTO.Address>(() => new FluentBuilderGeneratorTests.DTO.Address());
        public AddressBuilder UsingConstructor()
        {
            _Constructor_1362952513 = new Lazy<FluentBuilderGeneratorTests.DTO.Address>(() =>
            {
                return new FluentBuilderGeneratorTests.DTO.Address
                (

                );
            });
            _Constructor_1362952513_IsSet = true;

            return this;
        }


        public AddressBuilder UsingInstance(Address value) => UsingInstance(() => value);

        public AddressBuilder UsingInstance(Func<Address> func)
        {
            Instance = new Lazy<Address>(func);
            return this;
        }

        public override Address Build() => Build(true);

        public override Address Build(bool useObjectInitializer)
        {
            if (Instance is null)
            {
                Instance = new Lazy<Address>(() =>
                {
                    Address instance;
                    if (useObjectInitializer)
                    {
                        instance = new Address
                        {
                            HouseNumber = _houseNumber.Value,
                            City = _city.Value,
                            Array = _array.Value,
                            Array2 = _array2.Value,
                            ThingUsingConstructorWithItself = _thingUsingConstructorWithItself.Value,
                            ThingUsingConstructorWith2Parameters = _thingUsingConstructorWith2Parameters.Value,
                            ThingWithoutDefaultConstructor = _thingWithoutDefaultConstructor.Value,
                            ThingWithPrivateConstructor = _thingWithPrivateConstructor.Value,
                            Thing = _thing.Value,
                            ThingIsStruct = _thingIsStruct.Value,
                            IReadOnlyCollection = _iReadOnlyCollection.Value,
                            ReadOnlyCollection = _readOnlyCollection.Value,
                            Enumerable = _enumerable.Value,
                            Enumerable2 = _enumerable2.Value,
                            List = _list.Value,
                            List2 = _list2.Value,
                            Collection = _collection.Value,
                            Collection2 = _collection2.Value,
                            IDictionary = _iDictionary.Value,
                            IDictionary2 = _iDictionary2.Value,
                            Dictionary2 = _dictionary2.Value
                        };

                        return instance;
                    }

                    if (_Constructor_1362952513_IsSet) { instance = _Constructor_1362952513.Value; }
                    else { instance = Default(); }

                    return instance;
                });
            }

            if (_houseNumberIsSet) { Instance.Value.HouseNumber = _houseNumber.Value; }
            if (_cityIsSet) { Instance.Value.City = _city.Value; }
            if (_arrayIsSet) { Instance.Value.Array = _array.Value; }
            if (_array2IsSet) { Instance.Value.Array2 = _array2.Value; }
            if (_thingUsingConstructorWithItselfIsSet) { Instance.Value.ThingUsingConstructorWithItself = _thingUsingConstructorWithItself.Value; }
            if (_thingUsingConstructorWith2ParametersIsSet) { Instance.Value.ThingUsingConstructorWith2Parameters = _thingUsingConstructorWith2Parameters.Value; }
            if (_thingWithoutDefaultConstructorIsSet) { Instance.Value.ThingWithoutDefaultConstructor = _thingWithoutDefaultConstructor.Value; }
            if (_thingWithPrivateConstructorIsSet) { Instance.Value.ThingWithPrivateConstructor = _thingWithPrivateConstructor.Value; }
            if (_thingIsSet) { Instance.Value.Thing = _thing.Value; }
            if (_thingIsStructIsSet) { Instance.Value.ThingIsStruct = _thingIsStruct.Value; }
            if (_iReadOnlyCollectionIsSet) { Instance.Value.IReadOnlyCollection = _iReadOnlyCollection.Value; }
            if (_readOnlyCollectionIsSet) { Instance.Value.ReadOnlyCollection = _readOnlyCollection.Value; }
            if (_enumerableIsSet) { Instance.Value.Enumerable = _enumerable.Value; }
            if (_enumerable2IsSet) { Instance.Value.Enumerable2 = _enumerable2.Value; }
            if (_listIsSet) { Instance.Value.List = _list.Value; }
            if (_list2IsSet) { Instance.Value.List2 = _list2.Value; }
            if (_collectionIsSet) { Instance.Value.Collection = _collection.Value; }
            if (_collection2IsSet) { Instance.Value.Collection2 = _collection2.Value; }
            if (_iDictionaryIsSet) { Instance.Value.IDictionary = _iDictionary.Value; }
            if (_iDictionary2IsSet) { Instance.Value.IDictionary2 = _iDictionary2.Value; }
            if (_dictionary2IsSet) { Instance.Value.Dictionary2 = _dictionary2.Value; }

            PostBuild(Instance.Value);

            return Instance.Value;
        }

        public static Address Default() => new Address();

    }
}
#nullable disable