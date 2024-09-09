using MovieDbAssistant.Lib.Components.Signal;

namespace MovieDbAssistant.App.Commands;

/// <summary>
/// The build from query file command.
/// </summary>
/// <param name="Path">path</param>
/// <param name="Origin">origin of signal</param>
public sealed record BuildFromQueryFileCommand(
    string Path,
    object? Origin = null
    ) : ISignal;
