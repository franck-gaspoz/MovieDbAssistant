using MovieDbAssistant.Lib.Components.Signal;

namespace MovieDbAssistant.Lib.Components.Actions;

/// <summary>
/// The action base event args.
/// </summary>
/// <param name="Command">The command.</param>
public class ActionBaseEventContextArgs : ActionBaseEventArgs
{
    /// <summary>
    /// action context
    /// </summary>
    public ActionContext? Context { get; private set; }

    /// <summary>
    /// sender
    /// </summary>
    public object Sender { get; private set; }

    public ActionBaseEventContextArgs(
        ISignal command,
        object sender,
        ActionContext? context) : base(command)
    {
        Sender = sender;
        Context = context;
    }
}
