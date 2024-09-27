using MovieDbAssistant.Lib.Components.Signal;

namespace MovieDbAssistant.Lib.Components.Actions.EventsArgs;

/// <summary>
/// The action base event args.
/// </summary>
/// <param name="Command">The command.</param>
public class ActionBaseEventTextArgs : ActionBaseEventContextArgs
{
    /// <summary>
    /// Gets the text.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string Text { get; private set; }

    public ActionBaseEventTextArgs(
        ISignal command,
        object sender,
        ActionContext? context,
        string text) : base(command, sender, context) => Text = text;
}
