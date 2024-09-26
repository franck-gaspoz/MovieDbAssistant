using MovieDbAssistant.Lib.Components.Signal;

namespace MovieDbAssistant.Lib.Components.Actions.Commands;

/// <summary>
/// action command base
/// </summary>
/// <param name="ActionContext">action context</param>
/// <param name="HandleUI">if true, the command handler must handle UI interactions</param>
/// <param name="Key">a key than can be used to map the command, for example in a handler filter</param>
public record class ActionCommandBase(
    ActionContext? ActionContext = null,
    bool HandleUI = true,
    string? Key = null
    ) : ISignal
{
    /// <summary>
    /// Gets the action context.
    /// </summary>
    /// <value>An <see cref="ActionContext? "/></value>
    public ActionContext? ActionContext { get; private set; } = ActionContext;

    /// <summary>
    /// setup the context
    /// </summary>
    /// <param name="context">action context</param>
    /// <returns>this object</returns>
    public ActionCommandBase Setup(ActionContext context)
    {
        ActionContext = context;
        return this;
    }
}

