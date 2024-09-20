using System.Diagnostics;

using MovieDbAssistant.Lib.ComponentModels;
using MovieDbAssistant.Lib.Components.Actions.Commands;
using MovieDbAssistant.Lib.Components.Actions.Events;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Errors;
using MovieDbAssistant.Lib.Components.Extensions;
using MovieDbAssistant.Lib.Components.InstanceCounter;
using MovieDbAssistant.Lib.Components.Signal;

namespace MovieDbAssistant.Lib.Components.Actions;

/// <summary>
/// context of a feature action
/// </summary>
[Transient]
/// <summary>
/// action feature base
/// </summary>
#if DEBUG || TRACE
[DebuggerDisplay("{DbgId()}")]
#endif
public sealed class ActionContext :
    IIdentifiable
{
    #region fields & properties

#if DEBUG || TRACE
    public string DbgId() => this.Id();
#endif

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

    // interface IIdentifier

    /// <inheritdoc/>
    public string Id => this.Id();

    /// <inheritdoc/>
    public SharedCounter InstanceId { get; }

    #endregion

    #region create & setup

    /// <summary>
    /// new action action action
    /// </summary>
    /// <param name="signal">signaler</param>
    public ActionContext(
        ISignalR signal,
        StackErrors stackErrors)
    {
        (Sender, _signal, Errors, InstanceId)
            = (this, signal, stackErrors, new(this));
        _signal.Register<ActionEndedEvent>(this);
        _signal.Register<ActionErroredEvent>(this);
    }

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

    /// <summary>
    /// merge a previous context into this one
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="context">The context.</param>
    /// <returns>An <see cref="ActionContext"/></returns>
    public ActionContext Merge(
        object sender,
        ActionContext context)
    {
        Listeners.AddRange(
            new List<object>(context.Listeners) { context.Sender, Sender });
        Listeners = Listeners.Distinct().ToList();
        Errors.Merge(context.Errors);
        Sender = sender;
        return this;
    }

    /// <summary>
    /// replace values with ones of another context
    /// </summary>
    /// <param name="context">context</param>
    /// <returns>this object</returns>
    public ActionContext Setup(ActionContext context)
    {
        Sender = context.Sender;
        Listeners = new(context.Listeners);
        Errors.Setup(context.Errors); 
        return this;
    }

    #endregion

    #region /**----- signals dispatches handlers -----*/

    /// <inheritdoc/>
    public void Handle(object sender, ActionEndedEvent signal)
    {
        if (!HandleCheckMatch(sender,signal.Context, out var feature))
            return;
        HandleInternal( feature!, signal);
    }

    /// <inheritdoc/>
    public void Handle(object sender, ActionErroredEvent @event)
    {
        if (!HandleCheckMatch(sender, @event.Context, out var feature))
            return;
        HandleInternal( feature!, @event);
    }

    bool HandleCheckMatch(object sender, ActionContext context, out IActionFeature? feature)
    {
        feature = null;
        if (!sender.CheckIsFeature(out var _feature)) return false;
        feature = _feature;
        if (!feature!.RunInBackground) return false;
        if (!MustHandle(feature, context)) return false;
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
            feature.End(@event.Context,false);
            feature.OnFinally(@event.Context);
        });
        DispatchSignal(feature, @event);
        Dispose();
    }

    void HandleInternal(IActionFeature feature, ActionErroredEvent @event)
    {
#if TRACE
        Debug.WriteLine(feature!.IdWith("action errored event: " + @event.ToString()));
#endif
        DispatchAction(feature =>
        {
            feature.Error(@event);
            feature.OnFinally(@event.Context);
        });
        DispatchSignal(feature,@event);
        Dispose();
    }

    void Dispose()
    {
        _signal.Unregister<ActionErroredEvent>(this);
        _signal.Unregister<ActionEndedEvent>(this);
    }

    bool MustHandle(object sender, ActionContext context)
        => sender == Sender
            && context == this;

    #endregion

    #region operations

    void DispatchAction(Action<IActionFeature> action)
    {
        if (Sender is IActionFeature actionFeature)
            action(actionFeature);
    }

    void DispatchSignal<T>(object sender,T signal)
        where T : ISignal
    {
        foreach (var listener in Listeners)
        {
            _signal.TryInvoke(
                typeof(T),
                sender,
                listener,  
                signal );
        }
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
