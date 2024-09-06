using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;

using MovieDbAssistant.App.Services.Tray;
using MovieDbAssistant.Dmn.Components;

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
    public override void Run()
    {
        Setup(
            _config,
            (o, e) => Next(),
            Convert.ToInt32(_config[Anim_Interval_TrayIcon]!),
            onStop: () => Stop());
        base.Run();
    }

    /// <summary>
    /// setup next animation frame
    /// </summary>
    public void Next()
    {
        var s = _config.GetSection(Anim_WaitIcons);

        //var ico = new Icon(_settings.AssetPath(t[_n]!));
        //_trayMenuService.NotifyIcon.Icon = "";
    }
}
