using System.ComponentModel;

using Microsoft.Extensions.Configuration;

using MovieDbAssistant.App.Services.Tray;
using MovieDbAssistant.Lib.Components;

namespace MovieDbAssistant.App.Components.Tray;

/// <summary>
/// The tray background worker.
/// </summary>
sealed class TrayBackgroundWorker : BackgroundWorkerWrapper
{
    readonly IConfiguration _config;
    readonly TrayMenuService _trayMenuService;
    Action<TrayMenuService>? _action;
    readonly int _interval;
    bool _stopOnBallonTipClosed;
    readonly bool _autoRepeat;

    /// <summary>
    /// Initializes a new instance of the <see cref="TrayBackgroundWorker"/> class.
    /// </summary>
    /// <param name="trayMenuService">The tray menu service.</param>
    public TrayBackgroundWorker(
        IConfiguration config,
        TrayMenuService trayMenuService,
        int interval,
        bool stopOnBallonTipClosed = true,
        bool autoRepeat = true)
    {
        _config = config;
        _trayMenuService = trayMenuService;
        _interval = interval;
        _stopOnBallonTipClosed = stopOnBallonTipClosed;
        _autoRepeat = autoRepeat;
    }

    public TrayBackgroundWorker Run(
        Action<TrayMenuService> action,
        int? interval = null,
        bool? stopOnBallonTipClosed = null,
        bool? autoRepeat = null,
        Action? onStop = null)
    {
        _action = action;
        if (stopOnBallonTipClosed != null)
            _stopOnBallonTipClosed = stopOnBallonTipClosed.Value;

        Setup(
            _config,
            DoWorkAction,
            interval ?? _interval,
            PreDoWork,
            onStop,
            autoRepeat ?? _autoRepeat
            );

        base.Run();
        return this;
    }

    void DoWorkAction(object? o, DoWorkEventArgs e)
        => _action?.Invoke(_trayMenuService);

    void PreDoWork()
    {
        _trayMenuService.BalloonTipClosed -=
            _trayMenuService.BallonTipCloseBackgroundWorkerHandler;
        if (_stopOnBallonTipClosed)
            _trayMenuService.BalloonTipClosed +=
                _trayMenuService.BallonTipCloseBackgroundWorkerHandler;
    }
}
