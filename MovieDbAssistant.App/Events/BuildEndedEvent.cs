using MediatR;

namespace MovieDbAssistant.App.Events;

/// <summary>
/// build ended
/// </summary>
public sealed record BuildEndedEvent(string ItemId) : IRequest;
