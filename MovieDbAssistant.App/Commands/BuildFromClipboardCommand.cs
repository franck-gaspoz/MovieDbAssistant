using MediatR;

namespace MovieDbAssistant.App.Commands;

/// <summary>
/// The build from clipboard command.
/// </summary>
public sealed record BuildFromClipboardCommand() : IRequest;
