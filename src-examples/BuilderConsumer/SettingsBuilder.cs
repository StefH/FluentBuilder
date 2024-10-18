using FluentBuilder;

namespace BuilderConsumer
{
    [AutoGenerateBuilder<Settings>()]
    //[AutoGenerateBuilder(typeof(Settings))]
    public partial class SettingsBuilder
    {
    }
}