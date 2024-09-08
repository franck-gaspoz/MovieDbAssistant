using MediatR;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MovieDbAssistant.App.Services;
using MovieDbAssistant.App.Services.Tray;
using MovieDbAssistant.Dmn.Components;
using MovieDbAssistant.Lib.Components;

using static MovieDbAssistant.Dmn.Components.Settings;

namespace MovieDbAssistant.App.Features;

/// <summary>
/// action feature base
/// </summary>
abstract class ActionFeatureBase
{
    /// <summary>
    /// true if buzy
    /// </summary>
    protected abstract bool IsBuzy();

    /// <summary>
    /// set the buzy state
    /// </summary>
    /// <param name="buzy">buzy</param>
    protected abstract void SetBuzy(bool buzy);

    protected CancellationToken? CancellationToken;

    protected readonly IConfiguration Config;
    protected readonly IMediator Mediator;
    protected readonly IServiceProvider ServiceProvider;
    protected readonly Settings Settings;
    protected readonly Messages Messages;

    readonly string _actionDoneMessageKey;
    readonly string _actionOnGoingMessageKey;

    readonly BackgroundWorkerWrapper? _backgroundWorker;

    protected TrayMenuService Tray => ServiceProvider
        .GetRequiredService<TrayMenuService>();

    public ActionFeatureBase(
        IConfiguration config,
        IMediator mediator,
        IServiceProvider serviceProvider,
        Settings settings,
        Messages messages,
        string actionDoneMessageKey,
        string actionOnGoingMessageKey)
    {
        ServiceProvider = serviceProvider;
        Settings = settings;
        Messages = messages;
        _actionDoneMessageKey = actionDoneMessageKey;
        _actionOnGoingMessageKey = actionOnGoingMessageKey;
        Mediator = mediator;
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

    void DoWork()
    {
        void End(bool error = false)
        {
            Tray.StopAnimInfo();
            OnEnd();
            if (!error)
                OnSucessEnd();
            SetBuzy(false);
        }

        try
        {
            Tray.AnimWorkInfo(Config[_actionOnGoingMessageKey]!);

            Action();

            End();
        }
        catch (Exception ex)
        {
            End(true);
            OnErrorBeforePrompt();
            Messages.Err(Message_Error_Unhandled, ex.Message);
            OnErrorAfterPrompt();
        }
        finally
        {
            OnFinally();
        }
    }
}
