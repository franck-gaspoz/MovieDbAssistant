using MovieDbAssistant.Lib.Components.Actions;
using MovieDbAssistant.Lib.Components.Actions.Commands;

namespace MovieDbAssistant.App.Commands;

/// <summary>
/// The build from json file command.
/// </summary>
/// <param name="Path">path</param>
/// <param name="ActionContext">action context</param>
/// <param name="HandleUI">if true, the command handler should handle UI interactions</param>
public sealed record BuildFromJsonFileCommand(
    string Path,
    ActionContext? ActionContext = null,
    bool HandleUI = true
    ) : ActionFeatureCommandBase(ActionContext, HandleUI);
