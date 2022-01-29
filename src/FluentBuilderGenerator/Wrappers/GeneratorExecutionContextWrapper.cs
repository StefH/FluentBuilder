using Microsoft.CodeAnalysis;

namespace FluentBuilderGenerator.Wrappers;

internal class GeneratorExecutionContextWrapper : IGeneratorExecutionContextWrapper
{
    private readonly GeneratorExecutionContext _context;

    public GeneratorExecutionContextWrapper(GeneratorExecutionContext context)
    {
        _context = context;
    }

    public INamedTypeSymbol? GetTypeByMetadataName(string fullyQualifiedMetadataName)
    {
        return _context.Compilation.GetTypeByMetadataName(fullyQualifiedMetadataName);
    }
}