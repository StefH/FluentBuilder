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

namespace FluentBuilder
{
    public partial class ICollectionAddressBuilder : ICollectionBuilder<Address>
    {
        public override ICollectionAddressBuilder Add(Address item) => Add(() => item);
        public override ICollectionAddressBuilder Add(Func<Address> func)
        {
            _list.Value.Add(func());
            return this;
        }
        public ICollectionAddressBuilder Add(Action<FluentBuilder.AddressBuilder> action, bool useObjectInitializer = true)
        {
            var builder = new FluentBuilder.AddressBuilder();
            action(builder);
            Add(() => builder.Build(useObjectInitializer));
            return this;
        }

    }
}
#nullable disable