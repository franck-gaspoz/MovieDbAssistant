using MovieDbAssistant.Lib.Components.Actions.Commands;
using MovieDbAssistant.Lib.Components.Signal;

namespace MovieDbAssistant.Dmn.Events;

/// <summary>
/// build ended event
/// </summary>
/// <param name="ItemId">build item id</param>
/// <param name="Com">command</param>
/// <param name="Exception">exception (optional)</param>
/// <param name="Message">message (optional)</param>
public sealed record BuildErroredEvent(
    string ItemId,
    Lib.Components.Actions.Commands.ActionCommandBase Com,
    Exception? Exception = null,
    string? Message = null) : Lib.Components.Signal.ISignal;
