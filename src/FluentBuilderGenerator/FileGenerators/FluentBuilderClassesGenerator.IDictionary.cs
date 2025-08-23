using System.Text;
using FluentBuilderGenerator.Extensions;
using FluentBuilderGenerator.Interfaces;
using FluentBuilderGenerator.Models;
using Microsoft.CodeAnalysis;

namespace FluentBuilderGenerator.FileGenerators;

internal partial class FluentBuilderClassesGenerator
{
    private StringBuilder GenerateWithIDictionaryBuilderActionMethod(
        ClassSymbol classSymbol,
        IPropertyOrParameterSymbol property,
        (INamedTypeSymbol key, INamedTypeSymbol value)? tuple)
    {
        var className = classSymbol.BuilderClassName;

        string types = string.Empty;
        if (tuple != null)
        {
            var keyClassName = tuple.Value.key?.GenerateFullTypeName() ?? "object";
            var valueClassName = tuple.Value.value?.GenerateFullTypeName() ?? "object";

            types = $"{keyClassName}, {valueClassName}";
        }

        string dictionaryBuilderName = $"IDictionaryBuilder{(tuple == null ? string.Empty : "<" + types + ">")}";

        // If the property.Type is an interface, no cast is needed. Else cast the interface to the real type.
        var cast = property.Type.TypeKind == TypeKind.Interface ? "" : $"({property.Type}) ";

        return new StringBuilder()
            .AppendLine($"        public {className} With{property.Name}(Action<{_context.AssemblyName}.FluentBuilder.{dictionaryBuilderName}> action, bool useObjectInitializer = true) => With{property.Name}(() =>")
            .AppendLine("        {")
            .AppendLine($"            var builder = new {_context.AssemblyName}.FluentBuilder.{dictionaryBuilderName}();")
            .AppendLine("            action(builder);")
            .AppendLine($"            return {cast}builder.Build(useObjectInitializer);")
            .AppendLine("        });");
    }
}