using MovieDbAssistant.Lib.Components.Actions.Commands;

namespace MovieDbAssistant.App.Commands;

/// <summary>
/// The build input folder command.
/// </summary>
/// <param name="Origin">object at origin of the command if different from the command sender, else null</param>
/// <param name="HandleUI">if true, the command handler must handle UI interactions</param>
public sealed record BuildFromInputFolderCommand(
    object? Origin = null,
    bool HandleUI = true
    ) : ActionFeatureCommandBase(Origin, HandleUI);
