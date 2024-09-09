using MovieDbAssistant.Lib.Components.Signal;

namespace MovieDbAssistant.Dmn.Events;

/// <summary>
/// action errored event
/// </summary>
/// <param name="Sender">sender</param>
public sealed record ActionEndedEvent() : ISignal;
