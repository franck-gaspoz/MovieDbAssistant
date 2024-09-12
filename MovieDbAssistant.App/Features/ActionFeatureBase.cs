using System.Diagnostics;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MovieDbAssistant.App.Services;
using MovieDbAssistant.App.Services.Tray;
using MovieDbAssistant.Dmn.Components;
using MovieDbAssistant.Dmn.Events;
using MovieDbAssistant.Lib.ComponentModels;
using MovieDbAssistant.Lib.Components;
using MovieDbAssistant.Lib.Components.Actions.Commands;
using MovieDbAssistant.Lib.Components.Actions.Events;
using MovieDbAssistant.Lib.Components.Extensions;
using MovieDbAssistant.Lib.Components.InstanceCounter;
using MovieDbAssistant.Lib.Components.Signal;

using static MovieDbAssistant.Dmn.Components.Settings;

namespace MovieDbAssistant.App.Features;

/// <summary>
/// action feature base
/// </summary>
#if DEBUG
[DebuggerDisplay("{DbgId()}")]
#endif
abstract class ActionFeatureBase<TCommand> :
    ISignalHandler<ActionEndedEvent>,
    ISignalHandler<ActionErroredEvent>,
    IIdentifiable
    where TCommand : ActionFeatureCommandBase
{
#if DEBUG
    public string DbgId() => this.Id();
#endif

    /// <summary>
    /// instance id
    /// </summary>
    public SharedCounter InstanceId { get; }

    /// <summary>
    /// true if buzy
    /// </summary>
    public bool Buzy { get; protected set; } = false;

    protected readonly IConfiguration Config;
    protected readonly ISignalR Signal;
    protected readonly IServiceProvider ServiceProvider;
    protected readonly Settings Settings;
    protected readonly Messages Messages;
    protected TCommand? Com;

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
    /// called on end
    /// </summary>
    protected abstract void OnEnd();

    /// <summary>
    /// called on error, before the prompt is displayed
    /// </summary>
    protected abstract void OnErrorBeforePrompt();

    /// <summary>
    /// called on error, after the prompt is displayed
    /// </summary>
    protected abstract void OnErrorAfterPrompt();

    /// <summary>
    /// called on finally
    /// </summary>
    protected abstract void OnFinally();

    /// <summary>
    /// action
    /// </summary>
    protected abstract void Action();

    /// <summary>
    /// run the feature in a background worker
    /// </summary>
    protected void Run(TCommand com)
    {
        if (Buzy)
        {
            Messages.Warn(Builder_Busy);
            return;
        }
        Com = com;
        Buzy = true;
        _backgroundWorker!.RunAction((o, e) => DoWork());
    }

    void End(bool error = false)
    {
#if TRACE
        Debug.WriteLine(DbgId() + ": end");
#endif
        if (Com!.HandleUI)
            Tray.StopAnimInfo();
        OnEnd();
        if (!error)
            OnSucessEnd();
        Buzy = false;
    }

    void Error(string error)
    {
#if TRACE
        Debug.WriteLine(DbgId() + ": error = " + error);
#endif
        End(true);
        OnErrorBeforePrompt();       
        if (Com!.HandleUI)
            Messages.Err(Message_Error_Unhandled, error);
        OnErrorAfterPrompt();
    }

    void DoWork()
    {
#if TRACE
        Debug.WriteLine(DbgId() + ": DoWork");
#endif
        var error = false;
        try
        {
            if (Com!.HandleUI)
                Tray.AnimWorkInfo(Config[_actionOnGoingMessageKey]!);

            Action();

            // TODO: handle all feature actions as if always in background
            // action impl must handle errors and send completed / errored

            if (!_runInBackground)
                End();
        }
        catch (Exception ex)
        {
            // never here when action is threaded.
            // called thread crashes: no exception here
#if TRACE
            Debug.WriteLine(DbgId() + ": exception");
#endif
            error = true;
            Error(ex.Message);
        }
        finally
        {
            // when first thread goes out from here it doesn't know if subtask is still running
            // thus the finally won't occurs (avoided by !_runInBackground)
            // the role of the HandleUI is not certain here...
#if TRACE
            Debug.WriteLine(DbgId() + ": finally?");
#endif
            if ((error || !_runInBackground) && Com!.HandleUI)
            {
#if TRACE
                Debug.WriteLine(DbgId() + ": finally");
#endif
                OnFinally();
            }
        }
    }

    public void Handle(object sender, ActionEndedEvent @event)
    {
        if (!_runInBackground) return;
#if TRACE
        Debug.WriteLine(DbgId() + ": action ended event");
#endif
        if (MatchAction(sender))
        {
            End();
            OnFinally();
        }
    }

    public void Handle(object sender, ActionErroredEvent @event)
    {
        if (!_runInBackground) return;
#if TRACE
        Debug.WriteLine(DbgId() + ": error event");
#endif        
        if (MatchAction(sender))
        {
            Error(@event.Error);
            OnFinally();
        }
    }

    bool MatchAction(object sender)
        => sender == this;
}
