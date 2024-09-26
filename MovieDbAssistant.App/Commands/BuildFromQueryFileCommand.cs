using MovieDbAssistant.Lib.Components.Actions;
using MovieDbAssistant.Lib.Components.Actions.Commands;

namespace MovieDbAssistant.App.Commands;

/// <summary>
/// The build from query file command.
/// </summary>
/// <param name="Path">path</param>
/// <param name="ActionContext">action context</param>
/// <param name="HandleUI">if true, the command handler must handle UI interactions</param>
public sealed record BuildFromQueryFileCommand(
    string Path,
    ActionContext? ActionContext = null,
    bool HandleUI = true
    ) : ActionCommandBase(ActionContext, HandleUI);
