using MovieDbAssistant.Lib.Components.Actions;
using MovieDbAssistant.Lib.Components.Actions.Commands;

namespace MovieDbAssistant.App.Commands;

/// <summary>
/// The build from clipboard command.
/// </summary>
/// <param name="ActionContext">action context</param>
/// <param name="HandleUI">if true, the command handler must handle UI interactions</param>
public sealed record BuildFromClipboardCommand(
    ActionContext? ActionContext = null,
    bool HandleUI = true
) : ActionFeatureCommandBase(ActionContext, HandleUI);
