using MediatR;

namespace MovieDbAssistant.App.Commands;

/// <summary>
/// The build from query file command.
/// </summary>
public sealed record BuildFromQueryFileCommand(string Path) : IRequest;
