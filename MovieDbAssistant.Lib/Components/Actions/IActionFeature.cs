using MovieDbAssistant.Lib.ComponentModels;

namespace MovieDbAssistant.Lib.Components.Actions;

/// <summary>
/// interface of a feature action
/// </summary>
public interface IActionFeature : IIdentifiable
{
    /// <summary>
    /// feature id (typically type name)
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// true if action is a background task, else otherwise
    /// </summary>
    public bool RunInBackground { get; }

    /// <summary>
    /// called on finally, after end , on errors's
    /// </summary>
    public abstract void OnFinally();

    /// <summary>
    /// called on error, before the prompt is displayed. triggered after 'end'
    /// </summary>
    public abstract void OnErrorBeforePrompt();

    /// <summary>
    /// called on error, after the prompt is displayed. triggered after 'end'
    /// </summary>
    public abstract void OnErrorAfterPrompt();
}
