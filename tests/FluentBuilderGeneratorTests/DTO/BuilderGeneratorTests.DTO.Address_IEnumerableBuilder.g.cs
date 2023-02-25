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
using FluentBuilderGeneratorTests.FluentBuilder;
using FluentBuilderGeneratorTests.DTO;

namespace FluentBuilderGeneratorTests.DTO
{
    public partial class IEnumerableAddressBuilder : Builder<IEnumerable<FluentBuilderGeneratorTests.DTO.Address>>
    {
        private readonly Lazy<List<FluentBuilderGeneratorTests.DTO.Address>> _list = new Lazy<List<FluentBuilderGeneratorTests.DTO.Address>>(() => new List<FluentBuilderGeneratorTests.DTO.Address>());
        public IEnumerableAddressBuilder Add(Address item) => Add(() => item);
        public IEnumerableAddressBuilder Add(Func<Address> func)
        {
            _list.Value.Add(func());
            return this;
        }
        public IEnumerableAddressBuilder Add(Action<FluentBuilderGeneratorTests.DTO.AddressBuilder> action, bool useObjectInitializer = true)
        {
            var builder = new FluentBuilderGeneratorTests.DTO.AddressBuilder();
            action(builder);
            Add(() => builder.Build(useObjectInitializer));
            return this;
        }


        public override IEnumerable<FluentBuilderGeneratorTests.DTO.Address> Build() => Build(true);

        public override IEnumerable<FluentBuilderGeneratorTests.DTO.Address> Build(bool useObjectInitializer)
        {
            if (Object?.IsValueCreated != true)
            {
                Object = new Lazy<IEnumerable<FluentBuilderGeneratorTests.DTO.Address>>(() =>
                {
                    return _list.Value;
                });
            }

            PostBuild(Object.Value);

            return Object.Value;
        }

    }
}
#nullable disable