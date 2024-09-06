using MediatR;

namespace MovieDbAssistant.App.Commands;

/// <summary>
/// The exit command.
/// </summary>
public sealed record class ExitCommand() : IRequest;

