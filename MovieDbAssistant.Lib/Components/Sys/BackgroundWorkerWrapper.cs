using System.ComponentModel;
using System.Diagnostics;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using MovieDbAssistant.Lib.ComponentModels;
using MovieDbAssistant.Lib.Components.Actions;
using MovieDbAssistant.Lib.Components.Actions.Events;
using MovieDbAssistant.Lib.Components.Extensions;
using MovieDbAssistant.Lib.Components.InstanceCounter;
using MovieDbAssistant.Lib.Components.Logger;
using MovieDbAssistant.Lib.Components.Signal;

namespace MovieDbAssistant.Lib.Components.Sys;

/// <summary>
/// a background worker wrapper.
/// <para>can be inherited</para>
/// </summary>
#if DEBUG || TRACE
[DebuggerDisplay("{DbgId()}")]
#endif
public class BackgroundWorkerWrapper :
    IIdentifiable
{
    #region properties

#if DEBUG || TRACE
    /// <summary>
    /// Dbgs the id.
    /// </summary>
    /// <returns>A <see cref="string"/></returns>
    public string DbgId() => this.Id();
#endif

    /// <inheritdoc/>
    public SharedCounter InstanceId { get; }

    protected const string TraceLevelPrefix = "----- ";
    const string Postfix_NativePrefix = "-- ";

    readonly ILogger _logger;
    readonly ISignalR _signal;
    IConfiguration? _config;
    BackgroundWorker? _backgroundWorker;
    static readonly object _backgroundWorkerLock = new();
    Action<ActionContext, object?, DoWorkEventArgs>? _action;
    Action? _preDoWork;
    int? _interval;
    bool? _autoRepeat;
    Action<BackgroundWorkerWrapper>? _onStop;
    Action<BackgroundWorkerWrapper, Exception>? _onError;

    /// <summary>
    /// Gets or sets a value indicating whether the worker must ends if running, or if is ended
    /// </summary>
    /// <value>A <see cref="bool"/></value>
    public bool End { get; protected set; } = false;

    /// <summary>
    /// owner
    /// </summary>
    public object? Owner { get; set; } = null;

    /// <summary>
    /// context of current feature action
    /// </summary>
    public ActionContext? Context { get; protected set; }

    #endregion

    #region build and setup

    /// <summary>
    /// Initializes a new instance of the <see cref="BackgroundWorkerWrapper"/> class.
    /// </summary>
    /// <param name="logger">logger</param>
    /// <param name="signal">signalR</param>
    /// <param name="owner">owner</param>
    public BackgroundWorkerWrapper(
        ILogger logger,
        ISignalR signal,
        object? owner = null)
    {
        _logger = logger;
        (_signal, Owner, InstanceId)
            = (signal, owner, new(this));

        SetupDefaultStopHandler();
        SetupDefaultErrorHandler();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BackgroundWorkerWrapper"/> class.
    /// </summary>
    /// <param name="logger">logger</param>
    /// <param name="signal">signalR</param>
    /// <param name="config">The config.</param>
    /// <param name="owner">owner</param>
    public BackgroundWorkerWrapper(
        ILogger logger,
        ISignalR signal,
        IConfiguration config,
        object? owner = null)
    {
        Owner = owner;
        _logger = logger;
        _signal = signal;
        _config = config;
        InstanceId = new(this);

        SetupDefaultStopHandler();
        SetupDefaultErrorHandler();
    }

    protected BackgroundWorkerWrapper SetupDefaultErrorHandler()
    {
        Setup(
            (o, e) =>
            {
                OnError(e);
            });
        return this;
    }

    protected BackgroundWorkerWrapper SetupDefaultStopHandler()
    {
        Setup(o => OnStop(o));
        return this;
    }

    /// <summary>
    /// setup the background worker
    /// <para>must be called prior to <code>Run()</code></para>
    /// </summary>
    /// <param name="config">configuration</param>
    /// <param name="action">do work action</param>
    /// <param name="interval">do work interval</param>
    /// <param name="preDoWork">pre do work</param>
    /// <param name="autoRepeat">auto repeat</param>
    public BackgroundWorkerWrapper Setup(
        IConfiguration config,
        Action<ActionContext, object?, DoWorkEventArgs> action,
        int interval,
        Action? preDoWork = null,
        bool autoRepeat = false)
    {
        _config = config;
        _action = action;
        _preDoWork = preDoWork;
        _interval = interval;
        _autoRepeat = autoRepeat;
        return this;
    }

    /// <summary>
    /// setup the onError callback
    /// </summary>
    /// <param name="onError">The on error.</param>
    /// <returns>A <see cref="BackgroundWorkerWrapper"/></returns>
    public BackgroundWorkerWrapper Setup(
        Action<BackgroundWorkerWrapper, Exception>? onError = null)
    {
        _onError = onError;
        return this;
    }

    /// <summary>
    /// setup the onStop callback
    /// </summary>
    /// <param name="onError">The on error.</param>
    /// <returns>A <see cref="BackgroundWorkerWrapper"/></returns>
    public BackgroundWorkerWrapper Setup(
        Action<BackgroundWorkerWrapper>? onStop = null)
    {
        _onStop = onStop;
        return this;
    }

    /// <summary>
    /// setup properties
    /// </summary>
    /// <param name="preDoWork">The pre does work.</param>
    /// <param name="autoRepeat">If true, auto repeat.</param>
    /// <returns>A <see cref="BackgroundWorker"/></returns>
    public BackgroundWorkerWrapper Setup(
        Action? preDoWork = null,
        bool autoRepeat = true)
    {
        _preDoWork = preDoWork;
        _autoRepeat = autoRepeat;
        return this;
    }

    #endregion

    /// <summary>
    /// run an action in a background thread then terminates it
    /// </summary>
    /// <param name="owner">action owner</param>
    /// <param name="context">action context</param>
    /// <param name="action"><code>action ( ActionContext context, object? owner, DoWorkEventArgs args)</code></param>
    /// <param name="config">config. if null use _config</param>
    /// <returns>this object</returns>
    public BackgroundWorkerWrapper RunAction(
        object owner,
        ActionContext context,
        Action<ActionContext, object?, DoWorkEventArgs> action,
        IConfiguration? config = null
        )
    {
        Owner = owner;
        Setup(
            config ?? _config!,
            action,
            0,
            autoRepeat: false);
        return Run(context, Owner);
    }

    /// <summary>
    /// Stop and destroy background worker.
    /// </summary>
    public virtual void Stop(object sender)
    {
        _logger.LogDebug(Owner ?? this, LogPrefix() + "stop");
        lock (_backgroundWorkerLock)
            End = true;
    }

    /// <summary>
    /// default on stop handler
    /// </summary>
    /// <param name="sender">The sender.</param>
    public virtual void OnStop(object sender)
    {
        _logger.LogDebug(Owner ?? this, LogPrefix() + "STOPPED");
        lock (_backgroundWorkerLock)
        {
            End = true;
            if (_backgroundWorker is null) return;
            _backgroundWorker?.CancelAsync();
            _backgroundWorker?.Dispose();
            _backgroundWorker = null;
        }
    }

    /// <summary>
    /// run a new background worker. stop and destroy any previous one
    /// </summary>
    /// <param name="context">action context</param>
    /// <param name="caller">caller</param>
    /// <returns>this object</returns>
    public virtual BackgroundWorkerWrapper Run(
        ActionContext context,
        object caller
        )
    {
        Context = context;
        _logger.LogDebug(Owner ?? this, LogPrefix() + "run");
        lock (_backgroundWorkerLock)
        {
            if (_backgroundWorker != null && _backgroundWorker.IsBusy)
            {
                Stop(this);
                OnStop(this);
            }

            if (_backgroundWorker == null)
                _backgroundWorker = new BackgroundWorker
                { WorkerSupportsCancellation = true };

            _preDoWork?.Invoke();

            if (_backgroundWorker != null)
            {
                _backgroundWorker.DoWork += (o, e) =>
                {
                    try
                    {
                        End = false;
                        while (!End)
                        {
                            _action!(context, o, e);
                            End |= e.Cancel | !_autoRepeat!.Value;
                            if (!End)
                                Thread.Sleep(_interval!.Value);
                        }
                        _logger.LogDebug(Owner ?? this, LogPrefix() + "end");
                        _onStop?.Invoke(this);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(Owner ?? this, "error: " + ex.Message);
                        _onError?.Invoke(this, ex);
                    }
                };

                if (_backgroundWorker != null && !_backgroundWorker.IsBusy)
                    _backgroundWorker.RunWorkerAsync();
            }
        }

        return this;
    }

    /// <summary>
    /// on error default handler
    /// </summary>
    /// <param name="ex">exception</param>
    public virtual void OnError(Exception ex)
    {
        _logger.LogDebug(Owner ?? this, LogPrefix() + "OnError");
        Stop(this);
        OnStop(this);
        if (Owner != null && Context != null)
            _signal.Send(Owner!, new ActionErroredEvent(Context!, ex));
    }

    protected virtual string LogPrefix()
        => this.Id()
            + " "
            + TraceLevelPrefix
            + ": ";

    protected string LogNativePrefix()
        => Postfix_NativePrefix
            + nameof(BackgroundWorkerWrapper)
            + this.InstanceIdPostfix()
            + " "
            + TraceLevelPrefix
            + ": ";
}
