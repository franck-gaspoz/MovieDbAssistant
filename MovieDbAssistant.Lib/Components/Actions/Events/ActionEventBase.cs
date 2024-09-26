using MovieDbAssistant.Lib.Components.Signal;

namespace MovieDbAssistant.Lib.Components.Actions.Events;

/// <summary>
/// action event base
/// </summary>
public record ActionEventBase(ActionContext Context) : ISignal;
