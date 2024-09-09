using MovieDbAssistant.Lib.Components.Signal;

namespace MovieDbAssistant.Dmn.Events;

/// <summary>
/// build ended event
/// </summary>
/// <param name="SenderParent">sender parent</param>
/// <param name="ItemId">build item id</param>
public sealed record BuildEndedEvent(
    object SenderParent,
    string ItemId) : ISignal;
