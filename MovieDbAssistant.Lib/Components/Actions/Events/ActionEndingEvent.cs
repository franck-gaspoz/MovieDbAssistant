using MovieDbAssistant.Lib.Components.Signal;

namespace MovieDbAssistant.Lib.Components.Actions.Events;

/// <summary>
/// action ending event
/// </summary>
/// <param name="context">action context</param>
public sealed record ActionEndingEvent(ActionContext Context) 
    : ActionEventBase(Context);
