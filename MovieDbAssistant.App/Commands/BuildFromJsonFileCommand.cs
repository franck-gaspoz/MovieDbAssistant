using MovieDbAssistant.Lib.Components.Signal;

namespace MovieDbAssistant.App.Commands;

/// <summary>
/// The build from json file command.
/// </summary>
/// <param name="Path">path</param>
/// <param name="Origin">origin of signal</param>
public sealed record BuildFromJsonFileCommand(
    string Path,
    object? Origin = null
    ) : ISignal;
