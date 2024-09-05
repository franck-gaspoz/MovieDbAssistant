using System.ComponentModel;
using System.Diagnostics;

using MovieDbAssistant.App.Services.Tray;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;

using Microsoft.Extensions.Configuration;

namespace MovieDbAssistant.App.Components.Tray;

/// <summary>
/// The tray background worker.
/// </summary>
public sealed class TrayBackgroundWorker
{
    readonly IConfiguration _config;

    readonly TrayMenuService _trayMenuService;
    BackgroundWorker? _backgroundWorker;
    static int _bwinstance = 0;
    bool _end = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="TrayBackgroundWorker"/> class.
    /// </summary>
    /// <param name="config">The config.</param>
    /// <param name="trayMenuService">The tray menu service.</param>
    public TrayBackgroundWorker(
        IConfiguration config,
        TrayMenuService trayMenuService)
        => (_trayMenuService, _config) = (trayMenuService, config);

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
            _end = false;
            while (!_end)
            {
                Debug.Write(_bwinstance.ToString() + '.');
                action?.Invoke(_trayMenuService);
                _end = e.Cancel;
                if (!_end)
                    Thread.Sleep(interval);
            }
        };
        if (!_backgroundWorker.IsBusy)
        {
            _bwinstance++;
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
