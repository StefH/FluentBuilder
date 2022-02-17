using System.Text;
using FluentBuilderGenerator.Extensions;
using FluentBuilderGenerator.Models;
using Microsoft.CodeAnalysis;

namespace FluentBuilderGenerator.FileGenerators;

internal partial class FluentBuilderClassesGenerator
{
    private static StringBuilder GenerateWithIDictionaryBuilderActionMethod(
        ClassSymbol classSymbol,
        IPropertySymbol property,
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

        var sb = new StringBuilder();
        sb.AppendLine($"        public {className} With{property.Name}(Action<FluentBuilder.{dictionaryBuilderName}> action, bool useObjectInitializer = true) => With{property.Name}(() =>");
        sb.AppendLine("        {");
        sb.AppendLine($"            var builder = new FluentBuilder.{dictionaryBuilderName}();");
        sb.AppendLine("            action(builder);");
        sb.AppendLine($"            return {cast}builder.Build(useObjectInitializer);");
        sb.AppendLine("        });");
        return sb;
    }
}