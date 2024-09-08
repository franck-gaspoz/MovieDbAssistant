using MediatR;

namespace MovieDbAssistant.Dmn.Events;

/// <summary>
/// build ended event
/// </summary>
/// <param name="Sender">sender</param>
/// <param name="ItemId">build item id</param>
/// <param name="Exception">exception (optional)</param>
/// <param name="Message">message (optional)</param>
public sealed record BuildErroredEvent(    
    object Sender,
    string ItemId,
    Exception? Exception = null,
    string? Message = null) : IRequest;
