using MovieDbAssistant.Lib.Components.Signal;

namespace MovieDbAssistant.Lib.Components.Actions.Commands;

/// <summary>
/// action feature command base
/// </summary>
/// <param name="Origin">object at origin of the command if different from the command sender, else null</param>
/// <param name="HandleUI">if true, the command handler must handle UI interactions</param>
public record class ActionFeatureCommandBase(
    object? Origin = null,
    bool HandleUI = true
    ) : ISignal
;
