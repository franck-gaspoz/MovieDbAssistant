using MediatR;

namespace MovieDbAssistant.App.Commands;

/// <summary>
/// The open url command.
/// </summary>
public sealed record class OpenUrlCommand(string Url) : IRequest;

