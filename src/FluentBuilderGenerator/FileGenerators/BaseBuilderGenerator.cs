// This source code is based on https://justsimplycode.com/2020/12/06/auto-generate-builders-using-source-generator-in-net-5

using FluentBuilderGenerator.Extensions;
using FluentBuilderGenerator.Models;
using FluentBuilderGenerator.Types;

namespace FluentBuilderGenerator.FileGenerators;

internal class BaseBuilderGenerator(string assemblyName, bool supportsNullable) : IFileGenerator
{
    private const string Name = "FluentBuilder.BaseBuilder.g.cs";

    public FileData GenerateFile()
    {
        var text =
            $$"""
              {{Header.Text}}

              {{supportsNullable.IIf("#nullable enable")}}
              using System;

              namespace {{assemblyName}}.FluentBuilder
              {
                  public abstract class Builder<T>
                  {
                      protected Lazy<T>{{supportsNullable.IIf("?")}} Instance;
              
                      protected Type InstanceType = typeof(T);
              
                      public abstract T Build();
              
                      public abstract T Build(bool useObjectInitializer);
                 
                      protected virtual void PostBuild(T value) {}
                  }
              }
              {{supportsNullable.IIf("#nullable disable")}}
              """;

        return new FileData
        (
            FileDataType.Base,
            Name,
            text
        );
    }
}