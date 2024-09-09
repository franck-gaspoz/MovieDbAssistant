using MovieDbAssistant.Lib.Components.Signal;

namespace MovieDbAssistant.App.Commands;

/// <summary>
/// The explore folder command.
/// </summary>
public sealed record class ExploreFolderCommand(string Path) : ISignal;
