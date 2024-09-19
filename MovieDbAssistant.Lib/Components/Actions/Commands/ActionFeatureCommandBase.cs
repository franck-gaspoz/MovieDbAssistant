using MovieDbAssistant.Lib.Components.Signal;

namespace MovieDbAssistant.Lib.Components.Actions.Commands;

/// <summary>
/// action feature command base
/// </summary>
/// <param name="ActionContext">action context</param>
/// <param name="HandleUI">if true, the command handler must handle UI interactions</param>
/// <param name="Key">a key than can be used to map the command, for example in a handler filter</param>
public record class ActionFeatureCommandBase(
    ActionContext? ActionContext = null,
    bool HandleUI = true,
    string? Key = null
    ) : ISignal
;
