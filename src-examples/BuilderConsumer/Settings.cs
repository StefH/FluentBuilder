using System.Collections.Generic;

namespace BuilderConsumer
{
    public class Settings
    {
        public IReadOnlyCollection<Content> Contents { get; set; } = new List<Content>();
    }
}