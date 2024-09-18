using System.Diagnostics;

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
    protected abstract void OnSucessEnd();

    /// <summary>
    /// called on end, before onError's
    /// </summary>
    protected abstract void OnEnd();

    /// <summary>
    /// called on error, before the prompt is displayed. triggered after 'end'
    /// </summary>
    public abstract void OnErrorBeforePrompt();

    /// <summary>
    /// called on error, after the prompt is displayed. triggered after 'end'
    /// </summary>
    public abstract void OnErrorAfterPrompt();

    /// <summary>
    /// called on finally, after end , on errors's
    /// </summary>
    public abstract void OnFinally();

    /// <summary>
    /// action
    /// </summary>
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

        var context = ServiceProvider
            .GetRequiredService<ActionContext>()
            .Setup(this, [sender]);

        // always a background action ?
        _backgroundWorker!.RunAction((o, e) => DoWork(context));
    }

    void End(bool error = false)
    {
#if TRACE
        Debug.WriteLine(this.IdWith("end"));
#endif
        if (Com!.HandleUI)
            Tray.StopAnimInfo();
        OnEnd();
        if (!error)
            OnSucessEnd();
        Buzy = false;
    }

    void LogError(ActionErroredEvent errorEvent)
        => _errors.Push(new StackError(
                errorEvent.Error, 
                errorEvent.Trace));

    void Error(ActionErroredEvent errorEvent)
    {
        var message = errorEvent.ToString();
#if TRACE
        Debug.WriteLine(this.IdWith("error = " + message));
#endif
        LogError(errorEvent);
        End(true);
        OnErrorBeforePrompt();
        if (Com!.HandleUI)
            Messages.Err(Message_Error_Unhandled, message);
        OnErrorAfterPrompt();
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
                End();
        }
        catch (Exception ex)
        {
            // never here when action is threaded.
            // called thread crashes: no exception here
#if TRACE
            System.Console.Error.WriteLine(this.IdWith("exception"));
#endif
            error = true;
            Error(new ActionErroredEvent(ex));
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
                OnFinally();
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
            End();
            OnFinally();
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
            OnFinally();
        }
    }

    bool MustHandle(object sender)
        => sender == this;
}
