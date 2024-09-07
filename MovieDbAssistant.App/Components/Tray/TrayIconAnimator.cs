﻿using Microsoft.Extensions.Configuration;

using MovieDbAssistant.App.Services.Tray;
using MovieDbAssistant.Dmn.Components;
using MovieDbAssistant.Lib.Components;
using MovieDbAssistant.Lib.Components.Extensions;

using static MovieDbAssistant.Dmn.Components.Settings;

namespace MovieDbAssistant.App.Components.Tray;

/// <summary>
/// tray icon animator
/// </summary>
sealed class TrayIconAnimator : BackgroundWorkerWrapper
{
    readonly IConfiguration _config;
    readonly TrayMenuService _trayMenuService;
    readonly Settings _settings;
    int _n = 0;

    public TrayIconAnimator(
        IConfiguration config,
        TrayMenuService trayMenuService,
        Settings settings
        )
    {
        _config = config;
        _trayMenuService = trayMenuService;
        _settings = settings;
    }

    /// <summary>
    /// run the animator
    /// </summary>
    public new TrayIconAnimator Run()
    {
        Setup(
            _config,
            (o, e) => Next(),
            _config.GetInt(Anim_Interval_TrayIcon));
        base.Run();
        return this;
    }

    /// <summary>
    /// setup next animation frame
    /// </summary>
    public void Next()
    {
        var t = _config.GetArray(Anim_WaitIcons);
        var ico = new Icon(_settings.AssetPath(t[_n]!));
        _trayMenuService.NotifyIcon.Icon = ico;
        if (++_n > t.Length - 1) _n = 0;
    }
}
