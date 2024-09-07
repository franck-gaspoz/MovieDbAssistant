using MediatR;

namespace MovieDbAssistant.Dmn.Events;

/// <summary>
/// build ended event
/// </summary>
public sealed record BuildEndedEvent(
    object Sender,
    string ItemId) : IRequest;
