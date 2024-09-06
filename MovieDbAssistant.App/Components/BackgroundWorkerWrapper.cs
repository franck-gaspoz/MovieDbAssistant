using System.ComponentModel;

namespace MovieDbAssistant.App.Components;

/// <summary>
/// The background worker wrapper.
/// </summary>
class BackgroundWorkerWrapper
{
    BackgroundWorker? _backgroundWorker;
    readonly Action<object?, DoWorkEventArgs> _action;
    readonly Action? _preDoWork;
    readonly int _interval;
    readonly bool _autoRepeat;

    public bool End { get; protected set; } = false;

    public BackgroundWorkerWrapper(
        Action<object?, DoWorkEventArgs> action,
        int interval,
        Action? preDoWork = null,
        bool autoRepeat = true)
    {
        _action = action;
        _preDoWork = preDoWork;
        _interval = interval;
        _autoRepeat = autoRepeat;
    }

    /// <summary>
    /// Stop and destroy background worker.
    /// </summary>
    public void Stop()
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
    public void Run()
    {
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
                _action(o, e);
                End = e.Cancel | !_autoRepeat;
                if (!End)
                    Thread.Sleep(_interval);
            }
        };

        if (!_backgroundWorker.IsBusy)
            _backgroundWorker.RunWorkerAsync();
    }
}
