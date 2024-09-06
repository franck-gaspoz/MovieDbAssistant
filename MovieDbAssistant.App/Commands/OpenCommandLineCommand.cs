using MediatR;

namespace MovieDbAssistant.App.Commands;

/// <summary>
/// The open command line command.
/// </summary>
public sealed record class OpenCommandLineCommand() : IRequest;

