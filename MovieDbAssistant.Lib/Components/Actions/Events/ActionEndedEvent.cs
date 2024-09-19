using MovieDbAssistant.Lib.Components.Signal;

namespace MovieDbAssistant.Lib.Components.Actions.Events;

/// <summary>
/// action errored event
/// </summary>
/// <param name="context">action context</param>
public sealed record ActionEndedEvent(ActionContext Context) : ISignal;
