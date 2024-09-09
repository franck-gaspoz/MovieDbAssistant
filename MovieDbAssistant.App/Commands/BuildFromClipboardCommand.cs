using MovieDbAssistant.Lib.Components.Signal;

namespace MovieDbAssistant.App.Commands;

/// <summary>
/// The build from clipboard command.
/// </summary>
/// <param name="Origin">origin of signal</param>
public sealed record BuildFromClipboardCommand(
    object? Origin = null) : ISignal;
