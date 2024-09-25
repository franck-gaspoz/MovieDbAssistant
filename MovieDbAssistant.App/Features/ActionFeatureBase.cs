using System.Diagnostics;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using MovieDbAssistant.App.Services;
using MovieDbAssistant.App.Services.Tray;
using MovieDbAssistant.Dmn.Components;
using MovieDbAssistant.Lib.Components;
using MovieDbAssistant.Lib.Components.Actions;
using MovieDbAssistant.Lib.Components.Actions.Commands;
using MovieDbAssistant.Lib.Components.Actions.Events;
using MovieDbAssistant.Lib.Components.Extensions;
using MovieDbAssistant.Lib.Components.InstanceCounter;
using MovieDbAssistant.Lib.Components.Logger;
using MovieDbAssistant.Lib.Components.Signal;

using static MovieDbAssistant.Dmn.Components.Settings;

namespace MovieDbAssistant.App.Features;

#pragma warning disable CA2254 // Le modèle doit être une expression statique.

/// <summary>
/// action feature base
/// </summary>
#if DEBUG || TRACE
[DebuggerDisplay("{DbgId()}")]
#endif
abstract class ActionFeatureBase<TCommand> :
    IActionFeature,
    ISignalMethodHandler<ActionEndedEvent>,
    ISignalMethodHandler<ActionErroredEvent>
    where TCommand : ActionFeatureCommandBase
{
    #region fields & properties

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
    public bool RunInBackground { get; protected set; } = true;

    readonly ILogger<ActionFeatureBase<TCommand>> _logger;
    protected readonly IConfiguration Config;
    protected readonly ISignalR Signal;
    protected readonly IServiceProvider ServiceProvider;
    protected readonly Settings Settings;
    protected readonly Messages Messages;
    protected TCommand? Com;
    readonly string _actionOnGoingMessageKey;
    readonly BackgroundWorkerWrapper? _backgroundWorker;
    protected TrayMenuService Tray => ServiceProvider
        .GetRequiredService<TrayMenuService>();

    #endregion

    #region build & init

    public ActionFeatureBase(
        ILogger<ActionFeatureBase<TCommand>> logger,
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
        _logger = logger;
        Config = config;
        _actionOnGoingMessageKey = actionOnGoingMessageKey;
        RunInBackground = runInBackground;

        _backgroundWorker = new(
            logger,
            signal,
            config,
            this);
        _backgroundWorker.AddListener(this, this, signal);
        this.AddListener(this, this, signal);
    }

    #endregion

    #region action feature prototype

    /// <summary>
    /// action
    /// </summary>
    /// <param name="context">action context</param>
    protected abstract void Action(ActionContext context);

    #endregion

    #region /**----- signals handlers -----*/

    /// <summary>
    /// setup feature state ended
    /// </summary>
    /// <param name="context">action context</param>
    /// <param name="error">is end due to error</param>
    public virtual void Handle(
        object sender,
        ActionEndedEvent @event)
    {
        _logger.LogTrace(this,"END");
        if (Com!.HandleUI)
            Tray.StopAnimInfo();

        Signal.Send(this, new ActionEndingEvent(@event.Context));

        if (!@event.Context.IsErrored)
        {
            Signal.Send(this, new ActionSuccessfullyEnded(@event.Context));

            Buzy = false;

            Signal.Send(this, new ActionFinalisedEvent(@event.Context));

            Unsubscribe();
        }

        Buzy = false;
    }

    /// <summary>
    /// setup feature state error
    /// </summary>
    /// <param name="sender">sender</param>
    /// <param name="event">action errored event</param>
    public virtual void Handle(object sender, ActionErroredEvent @event)
    {
        var message = @event.ToString();

        _logger.LogError(this,message);

        @event.Context.LogError(@event);
        @event.Context.IsErrored = true;

        Signal.Send(this, new ActionEndedEvent(@event.Context));

        Signal.Send(this, new ActionBeforePromptEvent(@event.Context));
        if (Com!.HandleUI)
            Messages.Err(Message_Error_Unhandled, '\n' + message);

        Signal.Send(this, new ActionAfterPromptEvent(@event.Context));

        Signal.Send(this, new ActionFinalisedEvent(@event.Context));
    }

    void Unsubscribe()
    {
        Signal.Unsubscribe(this, this, _backgroundWorker!);
        Signal.Unsubscribe(this, this, this);
    }

    #endregion /**----  -----*/

    #region operations

    /// <summary>
    /// run the feature in or not in background worker
    /// </summary>
    /// <param name="sender">sender</param>
    /// <param name="com">command</param>
    protected void Run(object sender, TCommand com)
    {
        ActionContext? context = null;
        try
        {
            if (Buzy)
            {
                if (com.HandleUI)
                    Messages.Warn(Builder_Busy);
                _logger.LogWarning(this,"feature is not ready");
                return;
            }
            Com = com;
            Buzy = true;

            context = ServiceProvider
                .GetRequiredService<ActionContext>()
                .Setup(this, com, [sender]);

            if (com.ActionContext != null)
            {
                context.Merge(
                    this,
                    com.ActionContext);
                com.ActionContext.Setup(context);
            }
            else
                com.Setup(context);

            if (RunInBackground)
            {
                // run action in background thread
                _backgroundWorker!.RunAction(
                    this,
                    context,
                    (ctx, @from, @event)
                        => DoWork(context, sender));
            }
            else
            {
                DoWork(context, sender);
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(this,"exception: "+ex.Message);
            if (context != null)
                Signal.Send(this, new ActionErroredEvent(context!, ex));
            else
                _logger.LogError(this,"context is not defined");
        }
    }

    void DoWork(ActionContext context, object sender)
    {
        _logger.LogTrace(this,$"DoWork: background={RunInBackground} handleUI={Com!.HandleUI}");
        try
        {
            if (Com!.HandleUI)
                Tray.AnimWorkInfo(
                    _logger,
                    context,
                    sender,
                    Config[_actionOnGoingMessageKey]!);

            _logger.LogTrace(this,$"action");

            Action(context);

            if (!RunInBackground)
            {
                _logger.LogTrace(this,$"action ended");
                Signal.Send(this, new ActionEndedEvent(context));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(this,"exception: "+ex.Message);
            Signal.Send(this, new ActionErroredEvent(context, ex));
        }
        finally
        {
            _backgroundWorker.RemoveListener(this, this, Signal);
        }
    }

    #endregion
}
