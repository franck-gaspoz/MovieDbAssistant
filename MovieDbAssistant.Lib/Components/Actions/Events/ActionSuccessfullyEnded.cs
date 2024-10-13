namespace MovieDbAssistant.Lib.Components.Actions.Events;

/// <summary>
/// action successfully ended
/// </summary>
/// <param name="context">action context</param>
public sealed record ActionSuccessfullyEnded(ActionContext Context)
    : ActionEventBase(Context);
