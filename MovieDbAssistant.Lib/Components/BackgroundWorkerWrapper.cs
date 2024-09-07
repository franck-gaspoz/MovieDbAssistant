using System.ComponentModel;

using Microsoft.Extensions.Configuration;

using static MovieDbAssistant.Lib.Globals;

namespace MovieDbAssistant.Lib.Components;

/// <summary>
/// a background worker wrapper.
/// <para>can be inherited</para>
/// </summary>
public class BackgroundWorkerWrapper
{
    /// <summary>
    /// true if already setted up
    /// </summary>
    public bool SettedUp { get; protected set; } = false;

    IConfiguration? _config;
    readonly string _errorBackgroundWorkerWrapperNotInitializedKey;
    BackgroundWorker? _backgroundWorker;
    Action<object?, DoWorkEventArgs>? _action;
    Action? _preDoWork;
    int? _interval;
    bool? _autoRepeat;
    Action? _onStop;

    /// <summary>
    /// Gets or sets a value indicating whether the worker must ends if running, or if is ended
    /// </summary>
    /// <value>A <see cref="bool"/></value>
    public bool End { get; protected set; } = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="BackgroundWorkerWrapper"/> class.
    /// </summary>
    /// <param name="errorBackgroundWorkerWrapperNotInitializedKey">The error background worker wrapper not initialized key.</param>
    public BackgroundWorkerWrapper(
        string errorBackgroundWorkerWrapperNotInitializedKey
            = Error_BackgroundWorkerWrapper_Not_Initialized)
                => _errorBackgroundWorkerWrapperNotInitializedKey
                    = errorBackgroundWorkerWrapperNotInitializedKey;

    /// <summary>
    /// Initializes a new instance of the <see cref="BackgroundWorkerWrapper"/> class.
    /// </summary>
    /// <param name="config">The config.</param>
    /// <param name="errorBackgroundWorkerWrapperNotInitializedKey">The error background worker wrapper not initialized key.</param>
    public BackgroundWorkerWrapper(
        IConfiguration config,
        string errorBackgroundWorkerWrapperNotInitializedKey
            = Error_BackgroundWorkerWrapper_Not_Initialized)
    {
        _config = config;
        _errorBackgroundWorkerWrapperNotInitializedKey
            = errorBackgroundWorkerWrapperNotInitializedKey;
    }

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
        bool autoRepeat = true)
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
        return Run();
    }

    /// <summary>
    /// Stop and destroy background worker.
    /// </summary>
    public virtual void Stop()
    {
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
    public virtual BackgroundWorkerWrapper Run()
    {
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
            End = false;
            while (!End)
            {
                _action!(o, e);
                End |= e.Cancel | !_autoRepeat!.Value;
                if (!End)
                    Thread.Sleep(_interval!.Value);
            }
            _onStop?.Invoke();
        };

        if (!_backgroundWorker.IsBusy)
            _backgroundWorker.RunWorkerAsync();

        return this;
    }
}
