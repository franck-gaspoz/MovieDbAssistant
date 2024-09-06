using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;

using MovieDbAssistant.App.Services.Tray;

using static MovieDbAssistant.Dmn.Components.Settings;

namespace MovieDbAssistant.App.Components.Tray;

/// <summary>
/// tray icon animator
/// </summary>
sealed class TrayIconAnimator : BackgroundWorkerWrapper
{
    readonly IConfiguration _config;
    readonly TrayMenuService _trayMenuService;
    int _n = 0;

    public TrayIconAnimator(
        IConfiguration config,
        TrayMenuService trayMenuService
        )
    {
        _config = config;
        _trayMenuService = trayMenuService;
    }

    /// <summary>
    /// run the animator
    /// </summary>
    public override void Run()
    {
        Setup(
            _config,
            (o, e) => Next(),
            Convert.ToInt32(_config[DotAnimInterval]!),
            onStop: () => Stop());
        base.Run();
    }

    /// <summary>
    /// setup next animation frame
    /// </summary>
    public void Next()
    {
        //_trayMenuService.NotifyIcon.Icon = "";
    }
}
