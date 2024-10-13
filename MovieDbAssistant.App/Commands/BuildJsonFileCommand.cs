using MovieDbAssistant.Lib.Components.Actions;
using MovieDbAssistant.Lib.Components.Actions.Commands;

namespace MovieDbAssistant.App.Commands;

/// <summary>
/// The build from json file command.
/// </summary>
/// <param name="Path">path</param>
/// <param name="ActionContext">action context</param>
/// <param name="HandleUI">if true, the command handler should handle UI interactions</param>
/// <param name="Key">a key than can be used to map the command, for example in a handler filter</param>
public sealed record BuildJsonFileCommand(
    string Path,
    ActionContext? ActionContext = null,
    bool HandleUI = true,
    string? Key = null
    ) : ActionCommandBase(ActionContext, HandleUI, Key),
        ICommandWithPath;
