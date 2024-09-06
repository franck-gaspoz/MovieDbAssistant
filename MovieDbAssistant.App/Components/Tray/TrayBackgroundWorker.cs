using System.ComponentModel;
using System.Diagnostics;

using MovieDbAssistant.App.Services.Tray;

namespace MovieDbAssistant.App.Components.Tray;

/// <summary>
/// The tray background worker.
/// </summary>
public sealed class TrayBackgroundWorker
{
    readonly TrayMenuService _trayMenuService;

    BackgroundWorker? _backgroundWorker;
#if TRACE
    static int _bwinstance = 0;
#endif
    bool _end = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="TrayBackgroundWorker"/> class.
    /// </summary>
    /// <param name="trayMenuService">The tray menu service.</param>
    public TrayBackgroundWorker(
        TrayMenuService trayMenuService)
        => _trayMenuService = trayMenuService;

    /// <summary>
    /// Run background worker.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <param name="interval">The interval.</param>
    /// <param name="stopOnBallonTipClosed">If true, stop on ballon tip closed.</param>
    public void RunBackgroundWorker(
        Action<TrayMenuService> action,
        int interval,
        bool stopOnBallonTipClosed = true)
    {
        if (_backgroundWorker != null && _backgroundWorker.IsBusy)
            StopAndDestroyBackgroundWorker();

        if (_backgroundWorker == null)
            _backgroundWorker = new BackgroundWorker
            { WorkerSupportsCancellation = true };

        _trayMenuService.BalloonTipClosed -=
            _trayMenuService.BallonTipCloseBackgroundWorkerHandler;
        if (stopOnBallonTipClosed)
            _trayMenuService.BalloonTipClosed +=
                _trayMenuService.BallonTipCloseBackgroundWorkerHandler;

        _backgroundWorker.DoWork += (o, e) =>
        {
#if TRACE
            _bwinstance++;
#endif
            _end = false;
            while (!_end)
            {
#if TRACE
                Debug.Write(_bwinstance.ToString() + '.');
#endif
                action?.Invoke(_trayMenuService);
                _end = e.Cancel;
                if (!_end)
                    Thread.Sleep(interval);
            }
        };

        if (!_backgroundWorker.IsBusy)
        {
            _backgroundWorker.RunWorkerAsync();
        }
    }

    /// <summary>
    /// Stop and destroy background worker.
    /// </summary>
    public void StopAndDestroyBackgroundWorker()
    {
        _end = true;
        if (_backgroundWorker is null) return;
        _backgroundWorker.CancelAsync();
        _backgroundWorker.Dispose();
        _backgroundWorker = null;
    }
}
