using MovieDbAssistant.Lib.Components.Signal;

namespace MovieDbAssistant.Lib.Components.Actions.Events;

/// <summary>
/// action finalised event
/// </summary>
/// <param name="Context">action context</param>
public sealed record ActionFinalisedEvent(ActionContext Context) 
    : ActionEventBase(Context);
