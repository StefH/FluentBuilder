using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

namespace FluentBuilderGenerator.Extensions
{
    internal static class NamedTypeSymbolExtensions
    {
        public static string GetFileName(this INamedTypeSymbol namedTypeSymbol)
        {
            var typeName = namedTypeSymbol.GetFullType();
            return !(typeName.Contains('<') && typeName.Contains('>')) ?
                typeName :
                $"{typeName.Replace('.', '_').Replace('<', '_').Replace('>', '_').Replace(", ", "-")}_{typeName.Count(c => c == ',') + 1}";
        }

        public static string GetFullType(this INamedTypeSymbol namedTypeSymbol)
        {
            // https://www.codeproject.com/Articles/861548/Roslyn-Code-Analysis-in-Easy-Samples-Part
            //var str = new StringBuilder(namedTypeSymbol.Name);

            //if (namedTypeSymbol.TypeArguments.Count() > 0)
            //{
            //    str.AppendFormat("<{0}>", string.Join(", ", namedTypeSymbol.TypeArguments.OfType<INamedTypeSymbol>().Select(typeArg => typeArg.GetFullType())));
            //}

            return namedTypeSymbol.OriginalDefinition.ToString();// str.ToString();
        }

        public static string ResolveInterfaceNameWithOptionalTypeConstraints(this INamedTypeSymbol namedTypeSymbol, string interfaceName)
        {
            if (!namedTypeSymbol.IsGenericType)
            {
                return interfaceName;
            }

            var str = new StringBuilder($"{interfaceName}<{string.Join(", ", namedTypeSymbol.TypeArguments.Select(ta => ta.Name))}>");

            foreach (var typeParameterSymbol in namedTypeSymbol.TypeArguments.OfType<ITypeParameterSymbol>())
            {
                str.Append(typeParameterSymbol.GetWhereStatement());
            }

            return str.ToString();
        }

        /// <summary>
        /// See https://stackoverflow.com/questions/24157101/roslyns-gettypebymetadataname-and-generic-types
        /// </summary>
        public static string ResolveProxyClassName(this INamedTypeSymbol namedTypeSymbol)
        {
            return !namedTypeSymbol.IsGenericType ?
                $"{namedTypeSymbol.Name}Proxy" :
                $"{namedTypeSymbol.Name}Proxy<{string.Join(", ", namedTypeSymbol.TypeArguments.Select(ta => ta.Name))}>";
        }
    }
}