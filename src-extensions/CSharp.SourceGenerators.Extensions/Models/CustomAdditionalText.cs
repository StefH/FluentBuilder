using System.IO;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace CSharp.SourceGenerators.Extensions.Models
{
    internal sealed class CustomAdditionalText : AdditionalText
    {
        private readonly string _text;

        public override string Path { get; }

        public CustomAdditionalText(string path)
        {
            Path = path;
            _text = File.ReadAllText(path);
        }

        public CustomAdditionalText(SourceFile source)
        {
            Path = source.Path;
            _text = source.Text;
        }

        public override SourceText GetText(CancellationToken cancellationToken = new CancellationToken())
        {
            return SourceText.From(_text);
        }
    }
}