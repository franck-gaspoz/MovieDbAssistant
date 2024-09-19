using System.Diagnostics;

using MovieDbAssistant.Lib.Components.Actions.Commands;
using MovieDbAssistant.Lib.Components.Actions.Events;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Errors;
using MovieDbAssistant.Lib.Components.Extensions;
using MovieDbAssistant.Lib.Components.Signal;

namespace MovieDbAssistant.Lib.Components.Actions;

/// <summary>
/// context of a feature action
/// </summary>
[Transient]
public sealed class ActionContext :
    ISignalHandler<ActionEndedEvent>,
    ISignalHandler<ActionErroredEvent>
{
    #region fields & properties

    readonly ISignalR _signal;

    /// <summary>
    /// Gets the sender.
    /// </summary>
    /// <value>An <see cref="object"/></value>
    public object Sender { get; private set; }

    /// <summary>
    /// Gets the listeners.
    /// </summary>
    /// <value>A list of objects.</value>
    public List<object> Listeners { get; private set; } = [];

    /// <summary>
    /// stack of errors
    /// </summary>
    public StackErrors Errors { get; init; }

    ActionFeatureCommandBase? _command;

    /// <summary>
    /// handled command
    /// </summary>
    public ActionFeatureCommandBase Command => _command!;

    #endregion

    #region create & setup

    /// <summary>
    /// new action action action
    /// </summary>
    /// <param name="signal">signaler</param>
    public ActionContext(
        ISignalR signal,
        StackErrors stackErrors)
            => (Sender, _signal, Errors)
                = (this, signal, stackErrors);

    /// <summary>
    /// setup the action action
    /// </summary>
    /// <param name="sender">action sender</param>
    /// <param name="command">command</param>
    /// <param name="listeners">action listeners</param>
    /// <returns></returns>
    public ActionContext Setup(
        object sender,
        ActionFeatureCommandBase command,
        List<object> listeners
        )
    {
        _command = command;
        Sender = sender;
        Listeners.AddRange(listeners);
        Listeners = Listeners.Distinct()
            .ToList();
        return this;
    }

    #endregion

    #region /**----- interface ISignalHandler -----*/

    /// <inheritdoc/>
    public void Handle(object sender, ActionEndedEvent signal)
    {
        if (!HandleCheckMatch(sender, out var feature))
            return;
        HandleInternal(feature!, signal);
    }

    /// <inheritdoc/>
    public void Handle(object sender, ActionErroredEvent @event)
    {
        if (!HandleCheckMatch(sender, out var feature))
            return;
        HandleInternal(feature!, @event);
    }

    bool HandleCheckMatch(object sender, out IActionFeature? feature)
    {
        feature = null;
        if (!sender.CheckIsFeature(out var _feature)) return false;
        feature = _feature;
        if (!feature!.RunInBackground) return false;
        if (!MustHandle(feature)) return false;
        return true;
    }

    #endregion /**----  -----*/

    #region commands implementations

    void HandleInternal(IActionFeature feature, ActionEndedEvent @event)
    {
#if TRACE
        Debug.WriteLine(feature.IdWith("action ended event"));
#endif
        DispatchAction(feature =>
        {
            feature.End(@event.Context, false);
            feature.OnFinally(@event.Context);
        });
    }

    void HandleInternal(IActionFeature feature, ActionErroredEvent @event)
    {
        if (!feature.RunInBackground) return;
        if (!MustHandle(feature)) return;
#if TRACE
        Debug.WriteLine(feature!.IdWith("action errored event: " + @event.ToString()));
#endif
        DispatchAction(feature =>
        {
            feature.Error(@event);
            feature.OnFinally(@event.Context);
        });
    }

    bool MustHandle(object sender)
    => sender == Sender;

    #endregion

    #region operations

    void DispatchAction(Action<IActionFeature> action)
    {
        if (Sender is IActionFeature actionFeature)
            action(actionFeature);
        foreach (var listener in Listeners)
            if (listener is IActionFeature feature)
                action(feature);
    }

    /// <summary>
    /// logs an error in the action context
    /// </summary>
    /// <param name="event">error event</param>
    public void LogError(ActionErroredEvent @event)
        => Errors.Push(new StackError(
            @event.GetError(),
            @event.GetTrace()));

    #endregion
}
