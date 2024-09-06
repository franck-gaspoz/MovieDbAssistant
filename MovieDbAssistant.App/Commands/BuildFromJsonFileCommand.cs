using MediatR;

namespace MovieDbAssistant.App.Commands;

/// <summary>
/// The build from json file command.
/// </summary>
public sealed record BuildFromJsonFileCommand(string Path) : IRequest;
