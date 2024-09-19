using MovieDbAssistant.Lib.Components.Signal;

namespace MovieDbAssistant.Lib.Components.Actions.Commands;

/// <summary>
/// action feature command base
/// </summary>
/// <param name="ActionContext">action context</param>
/// <param name="HandleUI">if true, the command handler must handle UI interactions</param>
public record class ActionFeatureCommandBase(
    ActionContext? ActionContext = null,
    bool HandleUI = true
    ) : ISignal
;
