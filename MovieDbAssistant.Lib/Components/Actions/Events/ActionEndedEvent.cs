using MovieDbAssistant.Lib.Components.Signal;

namespace MovieDbAssistant.Lib.Components.Actions.Events;

/// <summary>
/// action errored event
/// </summary>
/// <param name="Sender">sender</param>
public sealed record ActionEndedEvent() : ISignal;
