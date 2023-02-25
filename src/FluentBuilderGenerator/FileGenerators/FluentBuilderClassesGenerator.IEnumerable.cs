using System.Text;
using FluentBuilderGenerator.Helpers;
using FluentBuilderGenerator.Models;

namespace FluentBuilderGenerator.FileGenerators;

internal partial class FluentBuilderClassesGenerator
{
    private string CreateIEnumerableBuilderCode(ClassSymbol classSymbol)
    {
        var type = classSymbol.NamedTypeSymbol.ToString();
        var t = IEnumerableBuilderHelper.GetGenericTypeAndToArray(classSymbol.Type, type).GenericType;

        return $@"//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by https://github.com/StefH/FluentBuilder version {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

{(_context.SupportsNullable ? "#nullable enable" : string.Empty)}
using System;
using System.Collections;
using System.Collections.Generic;
using {_context.AssemblyName}.FluentBuilder;
using {classSymbol.NamedTypeSymbol.ContainingNamespace};

namespace {classSymbol.BuilderNamespace}
{{
    public partial class {classSymbol.BuilderClassName} : Builder<{t}>
    {{
        private readonly Lazy<List<{type}>> _list = new Lazy<List<{type}>>(() => new List<{type}>());
{GenerateAddMethodsForIEnumerableBuilder(classSymbol)}

{GenerateBuildMethodForIEnumerableBuilder(classSymbol)}
    }}
}}
{(_context.SupportsNullable ? "#nullable disable" : string.Empty)}";
    }

    private static StringBuilder GenerateAddMethodsForIEnumerableBuilder(ClassSymbol classSymbol)
    {
        var className = classSymbol.BuilderClassName;
        var itemClassSymbol = classSymbol.NamedTypeSymbol;
        var itemBuilderFullName = classSymbol.ItemBuilderFullName;

        var sb = new StringBuilder();
        sb.AppendLine(8, $"public {className} Add({classSymbol.NamedTypeSymbol.Name} item) => Add(() => item);");
                   
        sb.AppendLine(8, $"public {className} Add(Func<{itemClassSymbol.Name}> func)");
        sb.AppendLine(8, @"{");
        sb.AppendLine(8, @"    _list.Value.Add(func());");
        sb.AppendLine(8, @"    return this;");
        sb.AppendLine(8, @"}");
               
        sb.AppendLine(8, $"public {className} Add(Action<{itemBuilderFullName}> action, bool useObjectInitializer = true)");
        sb.AppendLine(8, @"{");
        sb.AppendLine(8, $"    var builder = new {itemBuilderFullName}();");
        sb.AppendLine(8, @"    action(builder);");
        sb.AppendLine(8, @"    Add(() => builder.Build(useObjectInitializer));");
        sb.AppendLine(8, @"    return this;");
        sb.AppendLine(8, @"}");

        return sb;
    }

    private static string GenerateBuildMethodForIEnumerableBuilder(ClassSymbol classSymbol)
    {
        var (genericType, toArray) = IEnumerableBuilderHelper.GetGenericTypeAndToArray(classSymbol.Type, classSymbol.NamedTypeSymbol.ToString());

        return $@"        public override {genericType} Build() => Build(true);

        public override {genericType} Build(bool useObjectInitializer)
        {{
            if (Object?.IsValueCreated != true)
            {{
                Object = new Lazy<{genericType}>(() =>
                {{
                    return _list.Value{toArray};
                }});
            }}

            PostBuild(Object.Value);

            return Object.Value;
        }}
";
    }
}