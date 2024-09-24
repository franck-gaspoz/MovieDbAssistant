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
    IActionFeature
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
        RunInBackground = runInBackground;
        
        _backgroundWorker = new(
            signal,
            config,
            this);

        /*_backgroundWorker.Setup(
            onError: (o, e) =>
            {
                Error(new ActionErroredEvent(
                    _backgroundWorker.Context!, e));
            });*/
    }

    #region action feature prototype

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
    /// action
    /// </summary>
    /// <param name="context">action context</param>
    protected abstract void Action(ActionContext context);

    #region /**----- interface IActionFeature -----*/

    /// <summary>
    /// called on finally, after end , on errors's
    /// </summary>
    /// <param name="context">action context</param>
    public virtual void OnFinally(ActionContext context) { }

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
        Console.Error.WriteLine(this.IdWith("error = " + message));
#endif
        @event.Context.LogError(@event);
        End(@event.Context, true);       
        OnErrorBeforePrompt(@event.Context);
        if (Com!.HandleUI)
            Messages.Err(Message_Error_Unhandled, '\n'+message);
        OnErrorAfterPrompt(@event.Context);
    }

    #endregion /**----  -----*/

    #endregion

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
            _backgroundWorker!
                .For(this)
                .For(this as IActionFeature, context);

            _backgroundWorker!.RunAction((o, e)
                => DoWork(sender, context));
        }
        else
            DoWork(sender, context);
    }

    void DoWork(object sender,ActionContext context)
    {
#if TRACE
        Debug.WriteLine(this.IdWith($"DoWork: background={RunInBackground} handleUI={Com!.HandleUI}"));
#endif
        try
        {
            if (Com!.HandleUI)
                Tray.AnimWorkInfo(
                    sender,
                    Config[_actionOnGoingMessageKey]!);

#if TRACE
            Debug.WriteLine(this.IdWith($"action"));
#endif

            Action(context);

            if (!RunInBackground)
                Signal.Send(this,new ActionEndedEvent(context));
        }
        catch (Exception ex)
        {
#if TRACE
            System.Console.Error.WriteLine(this.IdWith("exception"));
#endif
            Signal.Send(this,new ActionErroredEvent(context,ex));
        }
    }

    /// <inheritdoc/>
    public string GetNamePrefix() => string.Empty;
}
