using System.Diagnostics;

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
    /// <param name="listeners">action listeners</param>
    /// <returns></returns>
    public ActionContext Setup(
        object sender,
        List<object> listeners
        )
    {
        Sender = sender;
        Listeners.AddRange(listeners);
        Listeners = Listeners.Distinct()
            .ToList();
        return this;
    }

    /// <summary>
    /// checks an object is a feature. if true, provides it in feature
    /// </summary>
    /// <param name="o">object to be checked</param>
    /// <param name="feature">feature or null</param>
    /// <param name="ignoreErrors">if true simply ignore errors, dot not trace them (default false)</param>
    /// <returns>true if feature, false otherwise</returns>
    static bool CheckIsFeature(object o, out IActionFeature? feature, bool ignoreErrors = true)
    {
        if (o is not IActionFeature _feature)
        {
            if (!ignoreErrors)
                Console.Error.WriteLine($"error: sender type mismatch. expected {nameof(IActionFeature)} but got {o.GetType().Name} ");

            feature = null;
            return false;
        }
        feature = _feature;
        return true;
    }

    bool HandleCheckMatch(object sender, out IActionFeature? feature)
    {
        feature = null;
        if (!CheckIsFeature(sender, out var _feature)) return false;
        feature = _feature;
        if (!feature!.RunInBackground) return false;
        if (!MustHandle(feature)) return false;
        return true;
    }

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

    void DispatchAction(Action<IActionFeature> action)
    {
        if (Sender is IActionFeature actionFeature)
            action(actionFeature);
        foreach (var listener in Listeners)
            if (listener is IActionFeature feature)
                action(feature);
    }

    void LogError(ActionErroredEvent @event)
        => Errors.Push(new StackError(
            @event.GetError(),
            @event.GetTrace()));

    bool MustHandle(object sender)
        => sender == Sender;
}
