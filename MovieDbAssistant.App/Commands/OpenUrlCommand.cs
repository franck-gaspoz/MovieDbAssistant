using MovieDbAssistant.Lib.Components.Signal;

namespace MovieDbAssistant.App.Commands;

/// <summary>
/// The open url command.
/// </summary>
public sealed record class OpenUrlCommand(string Url) : ISignal;

