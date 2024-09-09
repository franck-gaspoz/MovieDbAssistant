using MovieDbAssistant.Lib.Components.Signal;

namespace MovieDbAssistant.Dmn.Events;

/// <summary>
/// build ended event
/// </summary>
/// <param name="SenderParent">sender parent</param>
/// <param name="ItemId">build item id</param>
/// <param name="Exception">exception (optional)</param>
/// <param name="Message">message (optional)</param>
public sealed record BuildErroredEvent(
    object SenderParent,
    string ItemId,
    Exception? Exception = null,
    string? Message = null) : ISignal;
