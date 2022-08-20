//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by https://github.com/StefH/FluentBuilder version 0.6.0.0
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
    public partial class MyAddressBuilder : Builder<FluentBuilderGeneratorTests.DTO.Address>
    {
        private bool _houseNumberIsSet;
        private Lazy<int> _houseNumber = new Lazy<int>(() => default(int));
        public MyAddressBuilder WithHouseNumber(int value) => WithHouseNumber(() => value);
        public MyAddressBuilder WithHouseNumber(Func<int> func)
        {
            _houseNumber = new Lazy<int>(func);
            _houseNumberIsSet = true;
            return this;
        }
        public MyAddressBuilder WithoutHouseNumber()
        {
            WithHouseNumber(() => default(int));
            _houseNumberIsSet = false;
            return this;
        }

        private bool _cityIsSet;
        private Lazy<string?> _city = new Lazy<string?>(() => default(string?));
        public MyAddressBuilder WithCity(string? value) => WithCity(() => value);
        public MyAddressBuilder WithCity(Func<string?> func)
        {
            _city = new Lazy<string?>(func);
            _cityIsSet = true;
            return this;
        }
        public MyAddressBuilder WithoutCity()
        {
            WithCity(() => default(string?));
            _cityIsSet = false;
            return this;
        }

        private bool _arrayIsSet;
        private Lazy<string[]> _array = new Lazy<string[]>(() => new string[0]);
        public MyAddressBuilder WithArray(params string[] value) => WithArray(() => value);
        public MyAddressBuilder WithArray(Func<string[]> func)
        {
            _array = new Lazy<string[]>(func);
            _arrayIsSet = true;
            return this;
        }
        public MyAddressBuilder WithArray(Action<FluentBuilderGeneratorTests.FluentBuilder.ArrayBuilder<string>> action, bool useObjectInitializer = true) => WithArray(() =>
        {
            var builder = new FluentBuilderGeneratorTests.FluentBuilder.ArrayBuilder<string>();
            action(builder);
            return builder.Build(useObjectInitializer);
        });
        public MyAddressBuilder WithoutArray()
        {
            WithArray(() => new string[0]);
            _arrayIsSet = false;
            return this;
        }

        private bool _array2IsSet;
        private Lazy<FluentBuilderGeneratorTests.DTO.Address[]> _array2 = new Lazy<FluentBuilderGeneratorTests.DTO.Address[]>(() => new FluentBuilderGeneratorTests.DTO.Address[0]);
        public MyAddressBuilder WithArray2(params FluentBuilderGeneratorTests.DTO.Address[] value) => WithArray2(() => value);
        public MyAddressBuilder WithArray2(Func<FluentBuilderGeneratorTests.DTO.Address[]> func)
        {
            _array2 = new Lazy<FluentBuilderGeneratorTests.DTO.Address[]>(func);
            _array2IsSet = true;
            return this;
        }
        public MyAddressBuilder WithArray2(Action<FluentBuilderGeneratorTests.DTO.ArrayAddressBuilder> action, bool useObjectInitializer = true) => WithArray2(() =>
        {
            var builder = new FluentBuilderGeneratorTests.DTO.ArrayAddressBuilder();
            action(builder);
            return builder.Build(useObjectInitializer);
        });
        public MyAddressBuilder WithoutArray2()
        {
            WithArray2(() => new FluentBuilderGeneratorTests.DTO.Address[0]);
            _array2IsSet = false;
            return this;
        }

        private bool _thingWithConstructorWithItselfIsSet;
        private Lazy<FluentBuilderGeneratorTests.DTO.ThingWithConstructorWithItself> _thingWithConstructorWithItself = new Lazy<FluentBuilderGeneratorTests.DTO.ThingWithConstructorWithItself>(() => new FluentBuilderGeneratorTests.DTO.ThingWithConstructorWithItself(string.Empty, string.Empty));
        public MyAddressBuilder WithThingWithConstructorWithItself(FluentBuilderGeneratorTests.DTO.ThingWithConstructorWithItself value) => WithThingWithConstructorWithItself(() => value);
        public MyAddressBuilder WithThingWithConstructorWithItself(Func<FluentBuilderGeneratorTests.DTO.ThingWithConstructorWithItself> func)
        {
            _thingWithConstructorWithItself = new Lazy<FluentBuilderGeneratorTests.DTO.ThingWithConstructorWithItself>(func);
            _thingWithConstructorWithItselfIsSet = true;
            return this;
        }
        public MyAddressBuilder WithoutThingWithConstructorWithItself()
        {
            WithThingWithConstructorWithItself(() => new FluentBuilderGeneratorTests.DTO.ThingWithConstructorWithItself(string.Empty, string.Empty));
            _thingWithConstructorWithItselfIsSet = false;
            return this;
        }

        private bool _thingWithConstructorWith2ParametersIsSet;
        private Lazy<FluentBuilderGeneratorTests.DTO.ThingWithConstructorWith2Parameters> _thingWithConstructorWith2Parameters = new Lazy<FluentBuilderGeneratorTests.DTO.ThingWithConstructorWith2Parameters>(() => new FluentBuilderGeneratorTests.DTO.ThingWithConstructorWith2Parameters(default(int), default(int)));
        public MyAddressBuilder WithThingWithConstructorWith2Parameters(FluentBuilderGeneratorTests.DTO.ThingWithConstructorWith2Parameters value) => WithThingWithConstructorWith2Parameters(() => value);
        public MyAddressBuilder WithThingWithConstructorWith2Parameters(Func<FluentBuilderGeneratorTests.DTO.ThingWithConstructorWith2Parameters> func)
        {
            _thingWithConstructorWith2Parameters = new Lazy<FluentBuilderGeneratorTests.DTO.ThingWithConstructorWith2Parameters>(func);
            _thingWithConstructorWith2ParametersIsSet = true;
            return this;
        }
        public MyAddressBuilder WithoutThingWithConstructorWith2Parameters()
        {
            WithThingWithConstructorWith2Parameters(() => new FluentBuilderGeneratorTests.DTO.ThingWithConstructorWith2Parameters(default(int), default(int)));
            _thingWithConstructorWith2ParametersIsSet = false;
            return this;
        }

        private bool _thingWithoutDefaultConstructorIsSet;
        private Lazy<FluentBuilderGeneratorTests.DTO.ThingWithoutDefaultConstructor> _thingWithoutDefaultConstructor = new Lazy<FluentBuilderGeneratorTests.DTO.ThingWithoutDefaultConstructor>(() => new FluentBuilderGeneratorTests.DTO.ThingWithoutDefaultConstructor(default(int)));
        public MyAddressBuilder WithThingWithoutDefaultConstructor(FluentBuilderGeneratorTests.DTO.ThingWithoutDefaultConstructor value) => WithThingWithoutDefaultConstructor(() => value);
        public MyAddressBuilder WithThingWithoutDefaultConstructor(Func<FluentBuilderGeneratorTests.DTO.ThingWithoutDefaultConstructor> func)
        {
            _thingWithoutDefaultConstructor = new Lazy<FluentBuilderGeneratorTests.DTO.ThingWithoutDefaultConstructor>(func);
            _thingWithoutDefaultConstructorIsSet = true;
            return this;
        }
        public MyAddressBuilder WithoutThingWithoutDefaultConstructor()
        {
            WithThingWithoutDefaultConstructor(() => new FluentBuilderGeneratorTests.DTO.ThingWithoutDefaultConstructor(default(int)));
            _thingWithoutDefaultConstructorIsSet = false;
            return this;
        }

        private bool _thingWithPrivateConstructorIsSet;
        private Lazy<FluentBuilderGeneratorTests.DTO.ThingWithPrivateConstructor> _thingWithPrivateConstructor = new Lazy<FluentBuilderGeneratorTests.DTO.ThingWithPrivateConstructor>(() => default(FluentBuilderGeneratorTests.DTO.ThingWithPrivateConstructor)!);
        public MyAddressBuilder WithThingWithPrivateConstructor(FluentBuilderGeneratorTests.DTO.ThingWithPrivateConstructor value) => WithThingWithPrivateConstructor(() => value);
        public MyAddressBuilder WithThingWithPrivateConstructor(Func<FluentBuilderGeneratorTests.DTO.ThingWithPrivateConstructor> func)
        {
            _thingWithPrivateConstructor = new Lazy<FluentBuilderGeneratorTests.DTO.ThingWithPrivateConstructor>(func);
            _thingWithPrivateConstructorIsSet = true;
            return this;
        }
        public MyAddressBuilder WithoutThingWithPrivateConstructor()
        {
            WithThingWithPrivateConstructor(() => default(FluentBuilderGeneratorTests.DTO.ThingWithPrivateConstructor)!);
            _thingWithPrivateConstructorIsSet = false;
            return this;
        }

        private bool _thingIsSet;
        private Lazy<FluentBuilderGeneratorTests.DTO.Thing> _thing = new Lazy<FluentBuilderGeneratorTests.DTO.Thing>(() => new FluentBuilderGeneratorTests.DTO.Thing());
        public MyAddressBuilder WithThing(FluentBuilderGeneratorTests.DTO.Thing value) => WithThing(() => value);
        public MyAddressBuilder WithThing(Func<FluentBuilderGeneratorTests.DTO.Thing> func)
        {
            _thing = new Lazy<FluentBuilderGeneratorTests.DTO.Thing>(func);
            _thingIsSet = true;
            return this;
        }
        public MyAddressBuilder WithoutThing()
        {
            WithThing(() => new FluentBuilderGeneratorTests.DTO.Thing());
            _thingIsSet = false;
            return this;
        }

        private bool _thingIsStructIsSet;
        private Lazy<FluentBuilderGeneratorTests.DTO.ThingIsStruct> _thingIsStruct = new Lazy<FluentBuilderGeneratorTests.DTO.ThingIsStruct>(() => default(FluentBuilderGeneratorTests.DTO.ThingIsStruct));
        public MyAddressBuilder WithThingIsStruct(FluentBuilderGeneratorTests.DTO.ThingIsStruct value) => WithThingIsStruct(() => value);
        public MyAddressBuilder WithThingIsStruct(Func<FluentBuilderGeneratorTests.DTO.ThingIsStruct> func)
        {
            _thingIsStruct = new Lazy<FluentBuilderGeneratorTests.DTO.ThingIsStruct>(func);
            _thingIsStructIsSet = true;
            return this;
        }
        public MyAddressBuilder WithoutThingIsStruct()
        {
            WithThingIsStruct(() => default(FluentBuilderGeneratorTests.DTO.ThingIsStruct));
            _thingIsStructIsSet = false;
            return this;
        }

        private bool _iReadOnlyCollectionIsSet;
        private Lazy<System.Collections.Generic.IReadOnlyCollection<string>> _iReadOnlyCollection = new Lazy<System.Collections.Generic.IReadOnlyCollection<string>>(() => new List<string>());
        public MyAddressBuilder WithIReadOnlyCollection(System.Collections.Generic.IReadOnlyCollection<string> value) => WithIReadOnlyCollection(() => value);
        public MyAddressBuilder WithIReadOnlyCollection(Func<System.Collections.Generic.IReadOnlyCollection<string>> func)
        {
            _iReadOnlyCollection = new Lazy<System.Collections.Generic.IReadOnlyCollection<string>>(func);
            _iReadOnlyCollectionIsSet = true;
            return this;
        }
        public MyAddressBuilder WithoutIReadOnlyCollection()
        {
            WithIReadOnlyCollection(() => new List<string>());
            _iReadOnlyCollectionIsSet = false;
            return this;
        }

        private bool _readOnlyCollectionIsSet;
        private Lazy<System.Collections.ObjectModel.ReadOnlyCollection<long>> _readOnlyCollection = new Lazy<System.Collections.ObjectModel.ReadOnlyCollection<long>>(() => new System.Collections.ObjectModel.ReadOnlyCollection<long>(new List<long>()));
        public MyAddressBuilder WithReadOnlyCollection(System.Collections.ObjectModel.ReadOnlyCollection<long> value) => WithReadOnlyCollection(() => value);
        public MyAddressBuilder WithReadOnlyCollection(Func<System.Collections.ObjectModel.ReadOnlyCollection<long>> func)
        {
            _readOnlyCollection = new Lazy<System.Collections.ObjectModel.ReadOnlyCollection<long>>(func);
            _readOnlyCollectionIsSet = true;
            return this;
        }
        public MyAddressBuilder WithoutReadOnlyCollection()
        {
            WithReadOnlyCollection(() => new System.Collections.ObjectModel.ReadOnlyCollection<long>(new List<long>()));
            _readOnlyCollectionIsSet = false;
            return this;
        }

        private bool _enumerableIsSet;
        private Lazy<System.Collections.Generic.IEnumerable<byte>> _enumerable = new Lazy<System.Collections.Generic.IEnumerable<byte>>(() => new byte[0]);
        public MyAddressBuilder WithEnumerable(System.Collections.Generic.IEnumerable<byte> value) => WithEnumerable(() => value);
        public MyAddressBuilder WithEnumerable(Func<System.Collections.Generic.IEnumerable<byte>> func)
        {
            _enumerable = new Lazy<System.Collections.Generic.IEnumerable<byte>>(func);
            _enumerableIsSet = true;
            return this;
        }
        public MyAddressBuilder WithEnumerable(Action<FluentBuilderGeneratorTests.FluentBuilder.IEnumerableBuilder<byte>> action, bool useObjectInitializer = true) => WithEnumerable(() =>
        {
            var builder = new FluentBuilderGeneratorTests.FluentBuilder.IEnumerableBuilder<byte>();
            action(builder);
            return builder.Build(useObjectInitializer);
        });
        public MyAddressBuilder WithoutEnumerable()
        {
            WithEnumerable(() => new byte[0]);
            _enumerableIsSet = false;
            return this;
        }

        private bool _enumerable2IsSet;
        private Lazy<System.Collections.Generic.IEnumerable<FluentBuilderGeneratorTests.DTO.Address>> _enumerable2 = new Lazy<System.Collections.Generic.IEnumerable<FluentBuilderGeneratorTests.DTO.Address>>(() => new FluentBuilderGeneratorTests.DTO.Address[0]);
        public MyAddressBuilder WithEnumerable2(System.Collections.Generic.IEnumerable<FluentBuilderGeneratorTests.DTO.Address> value) => WithEnumerable2(() => value);
        public MyAddressBuilder WithEnumerable2(Func<System.Collections.Generic.IEnumerable<FluentBuilderGeneratorTests.DTO.Address>> func)
        {
            _enumerable2 = new Lazy<System.Collections.Generic.IEnumerable<FluentBuilderGeneratorTests.DTO.Address>>(func);
            _enumerable2IsSet = true;
            return this;
        }
        public MyAddressBuilder WithEnumerable2(Action<FluentBuilderGeneratorTests.DTO.IEnumerableAddressBuilder> action, bool useObjectInitializer = true) => WithEnumerable2(() =>
        {
            var builder = new FluentBuilderGeneratorTests.DTO.IEnumerableAddressBuilder();
            action(builder);
            return builder.Build(useObjectInitializer);
        });
        public MyAddressBuilder WithoutEnumerable2()
        {
            WithEnumerable2(() => new FluentBuilderGeneratorTests.DTO.Address[0]);
            _enumerable2IsSet = false;
            return this;
        }

        private bool _listIsSet;
        private Lazy<System.Collections.Generic.List<string>> _list = new Lazy<System.Collections.Generic.List<string>>(() => new List<string>());
        public MyAddressBuilder WithList(System.Collections.Generic.List<string> value) => WithList(() => value);
        public MyAddressBuilder WithList(Func<System.Collections.Generic.List<string>> func)
        {
            _list = new Lazy<System.Collections.Generic.List<string>>(func);
            _listIsSet = true;
            return this;
        }
        public MyAddressBuilder WithList(Action<FluentBuilderGeneratorTests.FluentBuilder.IListBuilder<string>> action, bool useObjectInitializer = true) => WithList(() =>
        {
            var builder = new FluentBuilderGeneratorTests.FluentBuilder.IListBuilder<string>();
            action(builder);
            return (System.Collections.Generic.List<string>) builder.Build(useObjectInitializer);
        });
        public MyAddressBuilder WithoutList()
        {
            WithList(() => new List<string>());
            _listIsSet = false;
            return this;
        }

        private bool _list2IsSet;
        private Lazy<System.Collections.Generic.List<FluentBuilderGeneratorTests.DTO.Address>> _list2 = new Lazy<System.Collections.Generic.List<FluentBuilderGeneratorTests.DTO.Address>>(() => new List<FluentBuilderGeneratorTests.DTO.Address>());
        public MyAddressBuilder WithList2(System.Collections.Generic.List<FluentBuilderGeneratorTests.DTO.Address> value) => WithList2(() => value);
        public MyAddressBuilder WithList2(Func<System.Collections.Generic.List<FluentBuilderGeneratorTests.DTO.Address>> func)
        {
            _list2 = new Lazy<System.Collections.Generic.List<FluentBuilderGeneratorTests.DTO.Address>>(func);
            _list2IsSet = true;
            return this;
        }
        public MyAddressBuilder WithList2(Action<FluentBuilderGeneratorTests.DTO.IListAddressBuilder> action, bool useObjectInitializer = true) => WithList2(() =>
        {
            var builder = new FluentBuilderGeneratorTests.DTO.IListAddressBuilder();
            action(builder);
            return (System.Collections.Generic.List<FluentBuilderGeneratorTests.DTO.Address>) builder.Build(useObjectInitializer);
        });
        public MyAddressBuilder WithoutList2()
        {
            WithList2(() => new List<FluentBuilderGeneratorTests.DTO.Address>());
            _list2IsSet = false;
            return this;
        }

        private bool _collectionIsSet;
        private Lazy<System.Collections.Generic.ICollection<long>> _collection = new Lazy<System.Collections.Generic.ICollection<long>>(() => new List<long>());
        public MyAddressBuilder WithCollection(System.Collections.Generic.ICollection<long> value) => WithCollection(() => value);
        public MyAddressBuilder WithCollection(Func<System.Collections.Generic.ICollection<long>> func)
        {
            _collection = new Lazy<System.Collections.Generic.ICollection<long>>(func);
            _collectionIsSet = true;
            return this;
        }
        public MyAddressBuilder WithCollection(Action<FluentBuilderGeneratorTests.FluentBuilder.ICollectionBuilder<long>> action, bool useObjectInitializer = true) => WithCollection(() =>
        {
            var builder = new FluentBuilderGeneratorTests.FluentBuilder.ICollectionBuilder<long>();
            action(builder);
            return builder.Build(useObjectInitializer);
        });
        public MyAddressBuilder WithoutCollection()
        {
            WithCollection(() => new List<long>());
            _collectionIsSet = false;
            return this;
        }

        private bool _collection2IsSet;
        private Lazy<System.Collections.Generic.ICollection<FluentBuilderGeneratorTests.DTO.Address>> _collection2 = new Lazy<System.Collections.Generic.ICollection<FluentBuilderGeneratorTests.DTO.Address>>(() => new List<FluentBuilderGeneratorTests.DTO.Address>());
        public MyAddressBuilder WithCollection2(System.Collections.Generic.ICollection<FluentBuilderGeneratorTests.DTO.Address> value) => WithCollection2(() => value);
        public MyAddressBuilder WithCollection2(Func<System.Collections.Generic.ICollection<FluentBuilderGeneratorTests.DTO.Address>> func)
        {
            _collection2 = new Lazy<System.Collections.Generic.ICollection<FluentBuilderGeneratorTests.DTO.Address>>(func);
            _collection2IsSet = true;
            return this;
        }
        public MyAddressBuilder WithCollection2(Action<FluentBuilderGeneratorTests.DTO.ICollectionAddressBuilder> action, bool useObjectInitializer = true) => WithCollection2(() =>
        {
            var builder = new FluentBuilderGeneratorTests.DTO.ICollectionAddressBuilder();
            action(builder);
            return builder.Build(useObjectInitializer);
        });
        public MyAddressBuilder WithoutCollection2()
        {
            WithCollection2(() => new List<FluentBuilderGeneratorTests.DTO.Address>());
            _collection2IsSet = false;
            return this;
        }

        private bool _iDictionaryIsSet;
        private Lazy<System.Collections.IDictionary> _iDictionary = new Lazy<System.Collections.IDictionary>(() => new Dictionary<object, object>());
        public MyAddressBuilder WithIDictionary(System.Collections.IDictionary value) => WithIDictionary(() => value);
        public MyAddressBuilder WithIDictionary(Func<System.Collections.IDictionary> func)
        {
            _iDictionary = new Lazy<System.Collections.IDictionary>(func);
            _iDictionaryIsSet = true;
            return this;
        }
        public MyAddressBuilder WithoutIDictionary()
        {
            WithIDictionary(() => new Dictionary<object, object>());
            _iDictionaryIsSet = false;
            return this;
        }

        private bool _iDictionary2IsSet;
        private Lazy<System.Collections.Generic.IDictionary<string, int>> _iDictionary2 = new Lazy<System.Collections.Generic.IDictionary<string, int>>(() => new Dictionary<string, int>());
        public MyAddressBuilder WithIDictionary2(System.Collections.Generic.IDictionary<string, int> value) => WithIDictionary2(() => value);
        public MyAddressBuilder WithIDictionary2(Func<System.Collections.Generic.IDictionary<string, int>> func)
        {
            _iDictionary2 = new Lazy<System.Collections.Generic.IDictionary<string, int>>(func);
            _iDictionary2IsSet = true;
            return this;
        }
        public MyAddressBuilder WithIDictionary2(Action<FluentBuilderGeneratorTests.FluentBuilder.IDictionaryBuilder<string, int>> action, bool useObjectInitializer = true) => WithIDictionary2(() =>
        {
            var builder = new FluentBuilderGeneratorTests.FluentBuilder.IDictionaryBuilder<string, int>();
            action(builder);
            return builder.Build(useObjectInitializer);
        });
        public MyAddressBuilder WithoutIDictionary2()
        {
            WithIDictionary2(() => new Dictionary<string, int>());
            _iDictionary2IsSet = false;
            return this;
        }

        private bool _dictionary2IsSet;
        private Lazy<System.Collections.Generic.Dictionary<string, int>> _dictionary2 = new Lazy<System.Collections.Generic.Dictionary<string, int>>(() => new Dictionary<string, int>());
        public MyAddressBuilder WithDictionary2(System.Collections.Generic.Dictionary<string, int> value) => WithDictionary2(() => value);
        public MyAddressBuilder WithDictionary2(Func<System.Collections.Generic.Dictionary<string, int>> func)
        {
            _dictionary2 = new Lazy<System.Collections.Generic.Dictionary<string, int>>(func);
            _dictionary2IsSet = true;
            return this;
        }
        public MyAddressBuilder WithDictionary2(Action<FluentBuilderGeneratorTests.FluentBuilder.IDictionaryBuilder<string, int>> action, bool useObjectInitializer = true) => WithDictionary2(() =>
        {
            var builder = new FluentBuilderGeneratorTests.FluentBuilder.IDictionaryBuilder<string, int>();
            action(builder);
            return (System.Collections.Generic.Dictionary<string, int>) builder.Build(useObjectInitializer);
        });
        public MyAddressBuilder WithoutDictionary2()
        {
            WithDictionary2(() => new Dictionary<string, int>());
            _dictionary2IsSet = false;
            return this;
        }


        public override Address Build(bool useObjectInitializer = true)
        {
            if (Object?.IsValueCreated != true)
            {
                Object = new Lazy<Address>(() =>
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
                            ThingWithConstructorWithItself = _thingWithConstructorWithItself.Value,
                            ThingWithConstructorWith2Parameters = _thingWithConstructorWith2Parameters.Value,
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

                    instance = new Address();
                    if (_houseNumberIsSet) { instance.HouseNumber = _houseNumber.Value; }
                    if (_cityIsSet) { instance.City = _city.Value; }
                    if (_arrayIsSet) { instance.Array = _array.Value; }
                    if (_array2IsSet) { instance.Array2 = _array2.Value; }
                    if (_thingWithConstructorWithItselfIsSet) { instance.ThingWithConstructorWithItself = _thingWithConstructorWithItself.Value; }
                    if (_thingWithConstructorWith2ParametersIsSet) { instance.ThingWithConstructorWith2Parameters = _thingWithConstructorWith2Parameters.Value; }
                    if (_thingWithoutDefaultConstructorIsSet) { instance.ThingWithoutDefaultConstructor = _thingWithoutDefaultConstructor.Value; }
                    if (_thingWithPrivateConstructorIsSet) { instance.ThingWithPrivateConstructor = _thingWithPrivateConstructor.Value; }
                    if (_thingIsSet) { instance.Thing = _thing.Value; }
                    if (_thingIsStructIsSet) { instance.ThingIsStruct = _thingIsStruct.Value; }
                    if (_iReadOnlyCollectionIsSet) { instance.IReadOnlyCollection = _iReadOnlyCollection.Value; }
                    if (_readOnlyCollectionIsSet) { instance.ReadOnlyCollection = _readOnlyCollection.Value; }
                    if (_enumerableIsSet) { instance.Enumerable = _enumerable.Value; }
                    if (_enumerable2IsSet) { instance.Enumerable2 = _enumerable2.Value; }
                    if (_listIsSet) { instance.List = _list.Value; }
                    if (_list2IsSet) { instance.List2 = _list2.Value; }
                    if (_collectionIsSet) { instance.Collection = _collection.Value; }
                    if (_collection2IsSet) { instance.Collection2 = _collection2.Value; }
                    if (_iDictionaryIsSet) { instance.IDictionary = _iDictionary.Value; }
                    if (_iDictionary2IsSet) { instance.IDictionary2 = _iDictionary2.Value; }
                    if (_dictionary2IsSet) { instance.Dictionary2 = _dictionary2.Value; }

                    return instance;
                });
            }

            PostBuild(Object.Value);

            return Object.Value;
        }

        public static Address Default() => new Address();

    }
}
#nullable disable