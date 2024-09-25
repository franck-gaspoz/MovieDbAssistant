using MovieDbAssistant.Lib.ComponentModels;
using MovieDbAssistant.Lib.Components.Actions.Events;

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

#if NO
    /// <summary>
    /// called on finally, after end with errors's or not
    /// </summary>
    /// <param name="context">action context</param>
    public abstract void OnFinally(ActionContext context);

    /// <summary>
    /// setup feature state ended
    /// </summary>
    /// <param name="context">action context</param>
    /// <param name="error">is end due to erreur (default false)</param>    
    public void End(ActionContext context, bool error = false);

    /// <summary>
    /// setup feature state error
    /// </summary>
    /// <param name="event">action errored event</param>
    public void Error(ActionErroredEvent @event);
#endif
}