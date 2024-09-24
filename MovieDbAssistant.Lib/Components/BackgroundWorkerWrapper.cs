using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;

using MovieDbAssistant.Lib.ComponentModels;
using MovieDbAssistant.Lib.Components.Actions;
using MovieDbAssistant.Lib.Components.Actions.Events;
using MovieDbAssistant.Lib.Components.Extensions;
using MovieDbAssistant.Lib.Components.InstanceCounter;
using MovieDbAssistant.Lib.Components.Signal;

using static MovieDbAssistant.Lib.Globals;

namespace MovieDbAssistant.Lib.Components;

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
#if DEBUG || TRACE
    /// <summary>
    /// Dbgs the id.
    /// </summary>
    /// <returns>A <see cref="string"/></returns>
    public string DbgId() => this.Id();
#endif

    /// <summary>
    /// true if already setted up
    /// </summary>
    public bool SettedUp { get; protected set; } = false;

    /// <inheritdoc/>
    public SharedCounter InstanceId { get; }

    readonly ISignalR _signal;
    IConfiguration? _config;
    readonly string _errorBackgroundWorkerWrapperNotInitializedKey;
    BackgroundWorker? _backgroundWorker;
    Action<object?, DoWorkEventArgs>? _action;
    Action? _preDoWork;
    int? _interval;
    bool? _autoRepeat;
    Action? _onStop;
    Action<BackgroundWorkerWrapper,Exception>? _onError;

    /// <summary>
    /// Gets or sets a value indicating whether the worker must ends if running, or if is ended
    /// </summary>
    /// <value>A <see cref="bool"/></value>
    public bool End { get; protected set; } = false;

    /// <summary>
    /// name of the owner / background worker
    /// </summary>
    public string? Name => Owner != null ? GetName(Owner) : string.Empty;

    /// <summary>
    /// owner
    /// </summary>
    public object? Owner { get; set; } = null;

    /// <summary>
    /// feature if action linked to any
    /// </summary>
    public IActionFeature? Feature { get; protected set; }

    /// <summary>
    /// context of current feature action
    /// </summary>
    public ActionContext? Context { get; protected set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BackgroundWorkerWrapper"/> class.
    /// </summary>
    /// <param name="signal">signalR</param>
    /// <param name="owner">owner</param>
    /// <param name="errorBackgroundWorkerWrapperNotInitializedKey">The error background worker wrapper not initialized key.</param>
    public BackgroundWorkerWrapper(
        ISignalR signal,
        object? owner = null,
        string errorBackgroundWorkerWrapperNotInitializedKey
            = Error_BackgroundWorkerWrapper_Not_Initialized)
    {
        (_signal, Owner, _errorBackgroundWorkerWrapperNotInitializedKey, InstanceId)
            = (signal, owner, errorBackgroundWorkerWrapperNotInitializedKey, new(this));
        SetupDefaultErrorHandler();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BackgroundWorkerWrapper"/> class.
    /// </summary>
    /// <param name="signal">signalR</param>
    /// <param name="config">The config.</param>
    /// <param name="owner">owner</param>
    /// <param name="errorBackgroundWorkerWrapperNotInitializedKey">The error background worker wrapper not initialized key.</param>
    public BackgroundWorkerWrapper(
    ISignalR signal,
    IConfiguration config,
    object? owner = null,
    string errorBackgroundWorkerWrapperNotInitializedKey
        = Error_BackgroundWorkerWrapper_Not_Initialized)
    {
        Owner = owner;
        _signal = signal;
        _config = config;
        _errorBackgroundWorkerWrapperNotInitializedKey
            = errorBackgroundWorkerWrapperNotInitializedKey;
        InstanceId = new(this);

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

    static string GetName(object owner) => owner.GetType().Name;

    /// <summary>
    /// indicates for who the background worker belong
    /// </summary>
    /// <param name="owner">owner</param>
    /// <returns>this object</returns>
    public BackgroundWorkerWrapper For(object owner)
    {
        Owner = owner;
        return this;
    }

    /// <summary>
    /// assign a feature to the background worker
    /// </summary>
    /// <param name="feature">feature</param>
    /// <param name="context">context</param>
    /// <returns>this object</returns>
    public BackgroundWorkerWrapper For(IActionFeature? feature, ActionContext context)
    {
        (Feature, Context) = (feature, context);
        return this;
    }

    /// <summary>
    /// assign a feature to the background worker
    /// </summary>
    /// <param name="feature">feature</param>
    /// <param name="context">context</param>
    /// <returns>this object</returns>
    public BackgroundWorkerWrapper For(object? feature,IActionFeature? actionFeature, ActionContext context)
    {
        (Owner,Feature, Context) = (feature,actionFeature, context);
        return this;
    }

    /// <inheritdoc/>
    public string GetNamePrefix()
        => Name ?? "¤ ";

    /// <summary>
    /// setup the background worker
    /// <para>must be called prior to <code>Run()</code></para>
    /// </summary>
    /// <param name="config">configuration</param>
    /// <param name="action">do work action</param>
    /// <param name="interval">do work interval</param>
    /// <param name="preDoWork">pre do work</param>
    /// <param name="onStop">on stop</param>
    /// <param name="autoRepeat">auto repeat</param>
    public BackgroundWorkerWrapper Setup(
        IConfiguration config,
        Action<object?, DoWorkEventArgs> action,
        int interval,
        Action? preDoWork = null,
        Action? onStop = null,
        bool autoRepeat = false)
    {
        _onStop = onStop;
        _config = config;
        _action = action;
        _preDoWork = preDoWork;
        _interval = interval;
        _autoRepeat = autoRepeat;
        SettedUp = true;
        return this;
    }

    public BackgroundWorkerWrapper Setup(
        Action<BackgroundWorkerWrapper, Exception>? onError = null)
    {
        _onError = onError;
        return this;
    }

    /// <summary>
    /// setup properties
    /// </summary>
    /// <param name="preDoWork">The pre does work.</param>
    /// <param name="onStop">The on stop.</param>
    /// <param name="onError">The on error.</param>
    /// <param name="autoRepeat">If true, auto repeat.</param>
    /// <returns>A <see cref="BackgroundWorker"/></returns>
    public BackgroundWorkerWrapper Setup(
        Action? preDoWork = null,
        Action? onStop = null,
        Action<BackgroundWorkerWrapper, Exception>? onError = null,
        bool autoRepeat = true)
    {
        _preDoWork = preDoWork;
        _onError = onError;
        _onStop = onStop;
        _autoRepeat = autoRepeat;
        return this;
    }

    /// <summary>
    /// run an action in a background thread then terminates it
    /// </summary>
    /// <param name="action">action</param>
    /// <param name="config">config. if null use _config</param>
    /// <returns>this object</returns>
    public BackgroundWorkerWrapper RunAction(
        Action<object?, DoWorkEventArgs> action,
        IConfiguration? config = null
        )
    {
        Setup(
            config ?? _config!,
            action,
            0,
            autoRepeat: false,
            onStop: () => Stop());
        return Run(Owner!);
    }

    /// <summary>
    /// Stop and destroy background worker.
    /// </summary>
    public virtual void Stop()
    {
#if DEBUG
        Debug.WriteLine(this.IdWith("stop"));
#endif
        End = true;
        if (_backgroundWorker is null) return;
        _backgroundWorker.CancelAsync();
        _backgroundWorker.Dispose();
        _backgroundWorker = null;
    }

    /// <summary>
    /// run a new background worker. stop and destroy any previous one
    /// </summary>
    /// <returns>this object</returns>
    public virtual BackgroundWorkerWrapper Run(object caller)
    {
        For(caller);
#if DEBUG
        Debug.WriteLine(this.IdWith("run"));
#endif
        if (!SettedUp) throw new InvalidOperationException(
            _config![_errorBackgroundWorkerWrapperNotInitializedKey]);

        if (_backgroundWorker != null && _backgroundWorker.IsBusy)
            Stop();

        if (_backgroundWorker == null)
            _backgroundWorker = new BackgroundWorker
            { WorkerSupportsCancellation = true };

        _preDoWork?.Invoke();

        _backgroundWorker.DoWork += (o, e) =>
        {
            try
            {
                End = false;
                while (!End)
                {
                    _action!(o, e);
                    End |= e.Cancel | !_autoRepeat!.Value;
                    if (!End)
                        Thread.Sleep(_interval!.Value);
                }
#if DEBUG
                Debug.WriteLine(this.IdWith("end"));
#endif
                _onStop?.Invoke();
            }
            catch (Exception ex)
            {
                _onError?.Invoke(this,ex);
                throw;
            }
        };

        if (!_backgroundWorker.IsBusy)
            _backgroundWorker.RunWorkerAsync();

        return this;
    }

    protected virtual void OnError(Exception ex)
    {
        if (Owner != null && Context != null)
            _signal.Send(Owner!, new ActionErroredEvent(Context!, ex));
    }
}
