using MovieDbAssistant.Lib.Components.Actions;
using MovieDbAssistant.Lib.Components.Actions.Commands;

namespace MovieDbAssistant.App.Commands;

/// <summary>
/// The build input folder command.
/// </summary>
/// <param name="ActionContext">action context</param>
/// <param name="HandleUI">if true, the command handler must handle UI interactions</param>
public sealed record BuildInputFolderCommand(
    ActionContext? ActionContext = null,
    bool HandleUI = true
    ) : ActionCommandBase(ActionContext, HandleUI);
