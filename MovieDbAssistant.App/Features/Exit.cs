using MovieDbAssistant.App.Commands;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Signal;

namespace MovieDbAssistant.App.Features;

[Transient]
sealed class Exit : ISignalHandler<ExitCommand>
{
    public void Handle(object sender, ExitCommand com)
        => Environment.Exit(0);
}
