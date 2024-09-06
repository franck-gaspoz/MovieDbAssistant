using MediatR;

namespace MovieDbAssistant.App.Commands;

/// <summary>
/// The explore folder command.
/// </summary>
public sealed record ExploreFolderCommand(string Path) : IRequest;
