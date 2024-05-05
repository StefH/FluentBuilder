//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by https://github.com/StefH/FluentBuilder version 0.9.0.0
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
using FluentBuilderGeneratorTests.Issue60;

namespace FluentBuilderGeneratorTests.Issue60
{
    public static partial class MyEntityBuilderExtensions
    {
        public static MyEntityBuilder AsBuilder(this FluentBuilderGeneratorTests.Issue60.MyEntity instance)
        {
            return new MyEntityBuilder().UsingInstance(instance);
        }
    }

    public partial class MyEntityBuilder : Builder<FluentBuilderGeneratorTests.Issue60.MyEntity>
    {
        private bool _eTagIsSet;
        private Lazy<global::Azure.ETag> _eTag = new Lazy<global::Azure.ETag>(() => default(global::Azure.ETag));
        public MyEntityBuilder WithETag(global::Azure.ETag value) => WithETag(() => value);
        public MyEntityBuilder WithETag(Func<global::Azure.ETag> func)
        {
            _eTag = new Lazy<global::Azure.ETag>(func);
            _eTagIsSet = true;
            return this;
        }
        private bool _partitionKeyIsSet;
        private Lazy<string> _partitionKey = new Lazy<string>(() => string.Empty);
        public MyEntityBuilder WithPartitionKey(string value) => WithPartitionKey(() => value);
        public MyEntityBuilder WithPartitionKey(Func<string> func)
        {
            _partitionKey = new Lazy<string>(func);
            _partitionKeyIsSet = true;
            return this;
        }
        private bool _rowKeyIsSet;
        private Lazy<string> _rowKey = new Lazy<string>(() => string.Empty);
        public MyEntityBuilder WithRowKey(string value) => WithRowKey(() => value);
        public MyEntityBuilder WithRowKey(Func<string> func)
        {
            _rowKey = new Lazy<string>(func);
            _rowKeyIsSet = true;
            return this;
        }
        private bool _timestampIsSet;
        private Lazy<global::System.DateTimeOffset?> _timestamp = new Lazy<global::System.DateTimeOffset?>(() => default(global::System.DateTimeOffset?));
        public MyEntityBuilder WithTimestamp(global::System.DateTimeOffset? value) => WithTimestamp(() => value);
        public MyEntityBuilder WithTimestamp(Func<global::System.DateTimeOffset?> func)
        {
            _timestamp = new Lazy<global::System.DateTimeOffset?>(func);
            _timestampIsSet = true;
            return this;
        }
        private bool _stringTestIsSet;
        private Lazy<string> _stringTest = new Lazy<string>(() => string.Empty);
        public MyEntityBuilder WithStringTest(string value) => WithStringTest(() => value);
        public MyEntityBuilder WithStringTest(Func<string> func)
        {
            _stringTest = new Lazy<string>(func);
            _stringTestIsSet = true;
            return this;
        }
        private bool _intTestIsSet;
        private Lazy<int> _intTest = new Lazy<int>(() => default(int));
        public MyEntityBuilder WithIntTest(int value) => WithIntTest(() => value);
        public MyEntityBuilder WithIntTest(Func<int> func)
        {
            _intTest = new Lazy<int>(func);
            _intTestIsSet = true;
            return this;
        }

        private bool _Constructor_1858959149_IsSet;
        private Lazy<FluentBuilderGeneratorTests.Issue60.MyEntity> _Constructor_1858959149 = new Lazy<FluentBuilderGeneratorTests.Issue60.MyEntity>(() => new FluentBuilderGeneratorTests.Issue60.MyEntity());
        public MyEntityBuilder UsingConstructor()
        {
            _Constructor_1858959149 = new Lazy<FluentBuilderGeneratorTests.Issue60.MyEntity>(() =>
            {
                return new FluentBuilderGeneratorTests.Issue60.MyEntity
                (

                );
            });
            _Constructor_1858959149_IsSet = true;

            return this;
        }


        public MyEntityBuilder UsingInstance(MyEntity value) => UsingInstance(() => value);

        public MyEntityBuilder UsingInstance(Func<MyEntity> func)
        {
            Instance = new Lazy<MyEntity>(func);
            return this;
        }

        public override MyEntity Build() => Build(true);

        public override MyEntity Build(bool useObjectInitializer)
        {
            if (Instance is null)
            {
                Instance = new Lazy<MyEntity>(() =>
                {
                    MyEntity instance;
                    if (useObjectInitializer)
                    {
                        instance = new MyEntity
                        {
                            ETag = _eTag.Value,
                            PartitionKey = _partitionKey.Value,
                            RowKey = _rowKey.Value,
                            Timestamp = _timestamp.Value,
                            StringTest = _stringTest.Value,
                            IntTest = _intTest.Value
                        };

                        return instance;
                    }

                    if (_Constructor_1858959149_IsSet) { instance = _Constructor_1858959149.Value; }
                    else { instance = Default(); }

                    return instance;
                });
            }

            if (_eTagIsSet) { Instance.Value.ETag = _eTag.Value; }
            if (_partitionKeyIsSet) { Instance.Value.PartitionKey = _partitionKey.Value; }
            if (_rowKeyIsSet) { Instance.Value.RowKey = _rowKey.Value; }
            if (_timestampIsSet) { Instance.Value.Timestamp = _timestamp.Value; }
            if (_stringTestIsSet) { Instance.Value.StringTest = _stringTest.Value; }
            if (_intTestIsSet) { Instance.Value.IntTest = _intTest.Value; }

            PostBuild(Instance.Value);

            return Instance.Value;
        }

        public static MyEntity Default() => new MyEntity();

    }
}
#nullable disable