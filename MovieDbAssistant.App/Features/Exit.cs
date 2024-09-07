using MovieDbAssistant.App.Commands;
using MovieDbAssistant.App.Components;

namespace MovieDbAssistant.App.Features;

sealed class Exit : SignalHandlerBase<ExitCommand>
{
    public Exit() => Handler = (_, _) => Environment.Exit(0);
}
