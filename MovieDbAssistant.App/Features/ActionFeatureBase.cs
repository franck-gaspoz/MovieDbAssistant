using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MovieDbAssistant.App.Services;
using MovieDbAssistant.App.Services.Tray;
using MovieDbAssistant.Dmn.Components;
using MovieDbAssistant.Dmn.Events;
using MovieDbAssistant.Lib.ComponentModels;
using MovieDbAssistant.Lib.Components;
using MovieDbAssistant.Lib.Components.Extensions;
using MovieDbAssistant.Lib.Components.InstanceCounter;
using MovieDbAssistant.Lib.Components.Signal;

using static MovieDbAssistant.Dmn.Components.Settings;

namespace MovieDbAssistant.App.Features;

/// <summary>
/// action feature base
/// </summary>
abstract class ActionFeatureBase :
    ISignalHandler<ActionEndedEvent>,
    ISignalHandler<ActionErroredEvent>,
    IIdentifiable
{
#if DEBUG
    public string DbgId() => this.Id();
#endif

    /// <summary>
    /// instance id
    /// </summary>
    public SharedCounter InstanceId { get; } = new();

    /// <summary>
    /// true if buzy
    /// </summary>
    protected abstract bool IsBuzy();

    /// <summary>
    /// set the buzy state
    /// </summary>
    /// <param name="buzy">buzy</param>
    protected abstract void SetBuzy(bool buzy);

    protected readonly IConfiguration Config;
    protected readonly ISignalR Signal;
    protected readonly IServiceProvider ServiceProvider;
    protected readonly Settings Settings;
    protected readonly Messages Messages;

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
        ServiceProvider = serviceProvider;
        Settings = settings;
        Messages = messages;
        _actionOnGoingMessageKey = actionOnGoingMessageKey;
        _runInBackground = runInBackground;
        Signal = signal;
        Config = config;
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
    protected void Run()
    {
        if (IsBuzy())
        {
            Messages.Warn(Builder_Busy);
            return;
        }
        SetBuzy(true);
        _backgroundWorker!.RunAction((o, e) => DoWork());
    }

    void End(bool error = false)
    {
        Tray.StopAnimInfo();
        OnEnd();
        if (!error)
            OnSucessEnd();
        SetBuzy(false);
    }

    void ErrorAsync(string error)
    {
        End(true);
        OnErrorBeforePrompt();
        Messages.Err(Message_Error_Unhandled, error);
        OnErrorAfterPrompt();
    }

    void DoWork()
    {
        try
        {
            Tray.AnimWorkInfo(Config[_actionOnGoingMessageKey]!);

            Action();

            if (!_runInBackground)
                End();
        }
        catch (Exception ex)
        {
            ErrorAsync(ex.Message);
        }
        finally
        {
            if (!_runInBackground)
                OnFinally();
        }
    }

    public void Handle(object sender, ActionEndedEvent @event)
    {
        if (!_runInBackground) return;
        if (MatchAction(sender))
        {
            End();
            OnFinally();
        }
    }

    public void Handle(object sender, ActionErroredEvent @event)
    {
        if (!_runInBackground) return;
        if (MatchAction(sender))
            ErrorAsync(@event.Error);
    }

    bool MatchAction(object sender)
        => sender == this;
}
