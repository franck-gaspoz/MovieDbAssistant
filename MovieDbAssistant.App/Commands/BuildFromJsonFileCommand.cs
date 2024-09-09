using MovieDbAssistant.Lib.Components.Actions.Commands;

namespace MovieDbAssistant.App.Commands;

/// <summary>
/// The build from json file command.
/// </summary>
/// <param name="Path">path</param>
/// <param name="Origin">object at origin of the command if different from the command sender, else null</param>
/// <param name="HandleUI">if true, the command handler must handle UI interactions</param>
public sealed record BuildFromJsonFileCommand(
    string Path,
    object? Origin = null,
    bool HandleUI = true
    ) : ActionFeatureCommandBase(Origin, HandleUI);
