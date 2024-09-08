using MediatR;

namespace MovieDbAssistant.Dmn.Events;

/// <summary>
/// build ended event
/// </summary>
public sealed record BuildErroredEvent(
    object Sender,
    string ItemId) : IRequest;
