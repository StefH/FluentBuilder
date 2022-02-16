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
using System.Collections;
using System.Collections.Generic;
using FluentBuilder;
using FluentBuilderGeneratorTests.DTO;

namespace FluentBuilderGeneratorTests.DTO
{
    public partial class IListAddressBuilder : Builder<IList<FluentBuilderGeneratorTests.DTO.Address>>
    {
        private readonly Lazy<List<FluentBuilderGeneratorTests.DTO.Address>> _list = new Lazy<List<FluentBuilderGeneratorTests.DTO.Address>>(() => new List<FluentBuilderGeneratorTests.DTO.Address>());
        public IListAddressBuilder Add(Address item) => Add(() => item);
        public IListAddressBuilder Add(Func<Address> func)
        {
            _list.Value.Add(func());
            return this;
        }
        public IListAddressBuilder Add(Action<AddressBuilder> action, bool useObjectInitializer = true)
        {
            var builder = new AddressBuilder();
            action(builder);
            Add(() => builder.Build(useObjectInitializer));
            return this;
        }


        public override IList<FluentBuilderGeneratorTests.DTO.Address> Build(bool useObjectInitializer = true)
        {
            if (Object?.IsValueCreated != true)
            {
                Object = new Lazy<IList<FluentBuilderGeneratorTests.DTO.Address>>(() =>
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