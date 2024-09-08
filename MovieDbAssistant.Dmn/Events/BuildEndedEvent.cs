using MediatR;

namespace MovieDbAssistant.Dmn.Events;

/// <summary>
/// build ended event
/// </summary>
/// <param name="Sender">sender</param>
/// <param name="ItemId">build item id</param>
public sealed record BuildEndedEvent(
    object Sender,
    string ItemId) : IRequest;
