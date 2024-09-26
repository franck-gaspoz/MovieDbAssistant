using MovieDbAssistant.Lib.Components.Actions.Commands;
using MovieDbAssistant.Lib.Components.Signal;

namespace MovieDbAssistant.Dmn.Events;

/// <summary>
/// build completed (no error) event
/// </summary>
/// <param name="ItemId">build item id</param>
/// <param name="Com">command</param>
public sealed record BuildCompletedEvent(
    string ItemId,
    Lib.Components.Actions.Commands.ActionCommandBase Com
    ) : Lib.Components.Signal.ISignal;
