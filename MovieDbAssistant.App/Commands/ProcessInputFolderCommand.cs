using MediatR;

namespace MovieDbAssistant.App.Commands;

/// <summary>
/// The process input folder command.
/// </summary>
public sealed record ProcessInputFolderCommand() : IRequest;
