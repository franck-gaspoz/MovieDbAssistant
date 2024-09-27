using MovieDbAssistant.Lib.Components.Actions.Commands;
using MovieDbAssistant.Lib.Components.Signal;

namespace MovieDbAssistant.Lib.Components.Actions.EventsArgs;

/// <summary>
/// The action base event args.
/// </summary>
/// <param name="Command">The command.</param>
public class ActionBaseEventArgs : EventArgs
{
    public ISignal Command { get; private set; }

    public ActionBaseEventArgs(ISignal command) => Command = command;
}
