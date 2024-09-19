﻿using System.Diagnostics;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MovieDbAssistant.App.Services;
using MovieDbAssistant.App.Services.Tray;
using MovieDbAssistant.Dmn.Components;
using MovieDbAssistant.Lib.Components;
using MovieDbAssistant.Lib.Components.Actions;
using MovieDbAssistant.Lib.Components.Actions.Commands;
using MovieDbAssistant.Lib.Components.Actions.Events;
using MovieDbAssistant.Lib.Components.Errors;
using MovieDbAssistant.Lib.Components.Extensions;
using MovieDbAssistant.Lib.Components.InstanceCounter;
using MovieDbAssistant.Lib.Components.Signal;

using Windows.Devices.Display.Core;

using static MovieDbAssistant.Dmn.Components.Settings;

namespace MovieDbAssistant.App.Features;

/// <summary>
/// action feature base
/// </summary>
#if DEBUG || TRACE
[DebuggerDisplay("{DbgId()}")]
#endif
abstract class ActionFeatureBase<TCommand> :
    IActionFeature,
    ISignalHandler<ActionEndedEvent>,
    ISignalHandler<ActionErroredEvent>
    where TCommand : ActionFeatureCommandBase
{
#if DEBUG || TRACE
    public string DbgId() => this.Id();
#endif

    /// <inheritdoc/>
    public string Id => this.Id();

    /// <summary>
    /// instance id
    /// </summary>
    public SharedCounter InstanceId { get; }

    /// <summary>
    /// true if buzy
    /// </summary>
    public bool Buzy { get; protected set; } = false;

    /// <inheritdoc/>
    public bool RunInBackground => _runInBackground;

    protected readonly IConfiguration Config;
    protected readonly ISignalR Signal;
    protected readonly IServiceProvider ServiceProvider;
    protected readonly Settings Settings;
    protected readonly Messages Messages;
    protected TCommand? Com;
    readonly StackErrors _errors = new();
    readonly string _actionOnGoingMessageKey;
    readonly bool _runInBackground;
    readonly BackgroundWorkerWrapper? _backgroundWorker;

    protected TrayMenuService Tray => ServiceProvider
        .GetRequiredService<TrayMenuService>();

    public ActionFeatureBase(
        IConfiguration config,
        ISignalR signal,
        IServiceProvider serviceProvider,
        Settings settings,
        Messages messages,
        string actionOnGoingMessageKey,
        bool runInBackground)
    {
        InstanceId = new(this);
        ServiceProvider = serviceProvider;
        Settings = settings;
        Messages = messages;
        Signal = signal;
        Config = config;
        _actionOnGoingMessageKey = actionOnGoingMessageKey;
        _runInBackground = runInBackground;
        _backgroundWorker = new(config);
    }

    /// <summary>
    /// called on end if no error
    /// </summary>
    /// <param name="context">action context</param>
    protected virtual void OnSucessEnd(ActionContext context) { }

    /// <summary>
    /// called on end, before onError's
    /// </summary>
    /// <param name="context">action context</param>
    protected virtual void OnEnd(ActionContext context) { }

    /// <summary>
    /// called on error, before the prompt is displayed. triggered after 'end'
    /// </summary>
    /// <param name="context">action context</param>
    protected virtual void OnErrorBeforePrompt(ActionContext context) { }

    /// <summary>
    /// called on error, after the prompt is displayed. triggered after 'end'
    /// </summary>
    /// <param name="context">action context</param>
    protected virtual void OnErrorAfterPrompt(ActionContext context) { }

    /// <summary>
    /// called on finally, after end , on errors's
    /// </summary>
    /// <param name="context">action context</param>
    public virtual void OnFinally(ActionContext context) { }

    /// <summary>
    /// action
    /// </summary>
    /// <param name="context">action context</param>
    protected abstract void Action(ActionContext context);

    /// <summary>
    /// run the feature in a background worker
    /// </summary>
    /// <param name="sender">sender</param>
    /// <param name="com">command</param>
    protected void Run(object sender, TCommand com)
    {
        if (Buzy)
        {
            Messages.Warn(Builder_Busy);
            return;
        }
        Com = com;
        Buzy = true;

        var context = com.ActionContext ?? ServiceProvider
            .GetRequiredService<ActionContext>()
            .Setup(this, [sender]);

        if (com.ActionContext != null)
            context.Setup(
                this, [context.Listeners, sender]
                );

        // always a background action ?
        _backgroundWorker!.RunAction((o, e) => DoWork(context));
    }

    /// <summary>
    /// setup feature state ended
    /// </summary>
    /// <param name="context">action context</param>
    /// <param name="error">is end due to error</param>
    public virtual void End(
        ActionContext context,
        bool error = false)
    {
#if TRACE
        Debug.WriteLine(this.IdWith("end"));
#endif
        if (Com!.HandleUI)
            Tray.StopAnimInfo();
        OnEnd(context);
        if (!error)
            OnSucessEnd(context);
        Buzy = false;
    }

    /// <summary>
    /// setup feature state error
    /// </summary>
    /// <param name="event">action errored event</param>
    public virtual void Error(ActionErroredEvent @event)
    {
        var message = @event.ToString();
#if TRACE
        Debug.WriteLine(this.IdWith("error = " + message));
#endif
        LogError(@event);
        End(@event.Context, true);
        OnErrorBeforePrompt(@event.Context);
        if (Com!.HandleUI)
            Messages.Err(Message_Error_Unhandled, message);
        OnErrorAfterPrompt(@event.Context);
    }

    void DoWork(ActionContext context)
    {
#if TRACE
        Debug.WriteLine(this.IdWith("DoWork"));
#endif
        var error = false;
        try
        {
            if (Com!.HandleUI)
                Tray.AnimWorkInfo(Config[_actionOnGoingMessageKey]!);

#if TRACE
            Debug.WriteLine(this.IdWith($"action (handleUI={Com.HandleUI})"));
#endif

            Action(context);

            // -----------------------------------------------------------
            // TODO: handle all feature actions as if always in background
            // action impl must handle errors and send completed / errored*
            // NO: simply add event handlers coming from called action
            // so improve Action() method signature
            // -----------------------------------------------------------

            if (!_runInBackground)
                End(context);
        }
        catch (Exception ex)
        {
            // never here when action is threaded.
            // called thread crashes: no exception here
#if TRACE
            System.Console.Error.WriteLine(this.IdWith("exception"));
#endif
            error = true;
            Error(new ActionErroredEvent(context,ex));
        }
        finally
        {
            // when first thread goes out from here it doesn't know if subtask is still running
            // thus the finally won't occurs (avoided by !_runInBackground)
#if TRACE
            Debug.WriteLine(this.IdWith("finally?"));
#endif
            if ((error || !_runInBackground))
            {
#if TRACE
                Debug.WriteLine(this.IdWith("finally"));
#endif
                OnFinally(context);
            }
            // else if !_runInBackground : event from action
        }
    }

    public void Handle(object sender, ActionEndedEvent @event)
    {
        if (!_runInBackground) return;

        if (MustHandle(sender))
        {
#if TRACE
            Debug.WriteLine(this.IdWith("action ended event"));
#endif
            End(@event.Context);
            OnFinally(@event.Context);
        }
    }

    public void Handle(object sender, ActionErroredEvent @event)
    {
        if (!_runInBackground) return;

        if (MustHandle(sender))
        {
#if TRACE
            Debug.WriteLine(this.IdWith("error event"));
#endif  
            Error(@event);
            OnFinally(@event.Context);
        }
    }

    bool MustHandle(object sender)
        => sender == this;

    void LogError(ActionErroredEvent errorEvent)
        => _errors.Push(new StackError(
                errorEvent.GetError(),
                errorEvent.GetTrace()));
}
