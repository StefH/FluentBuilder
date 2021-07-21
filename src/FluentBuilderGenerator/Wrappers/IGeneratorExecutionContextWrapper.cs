using Microsoft.CodeAnalysis;

namespace FluentBuilderGenerator.Wrappers
{
    internal interface IGeneratorExecutionContextWrapper
    {
        /// <see cref="Compilation.GetTypeByMetadataName(string)"/>
        INamedTypeSymbol? GetTypeByMetadataName(string fullyQualifiedMetadataName);
    }
}