using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MovieDbAssistant.App.Configuration;
using MovieDbAssistant.App.Services.Tray;
using MovieDbAssistant.Dmn.Components;
using MovieDbAssistant.Lib.Components.Actions;
using MovieDbAssistant.Lib.Components.Extensions;
using MovieDbAssistant.Lib.Components.Signal;
using MovieDbAssistant.Lib.Components.Sys;

namespace MovieDbAssistant.App.Components.Tray;

/// <summary>
/// tray icon animator
/// </summary>
sealed class TrayIconAnimator : BackgroundWorkerWrapper
{
    readonly IConfiguration _config;
    readonly TrayMenuService _trayMenuService;
    readonly Settings _settings;
    readonly IOptions<AppSettings> _appSettings;
    int _n = 0;

    public TrayIconAnimator(
        ILogger logger,
        ISignalR signal,
        IConfiguration config,
        TrayMenuService trayMenuService,
        Settings settings,
        IOptions<AppSettings> appSettings
        ) : base(logger, signal, string.Empty)
    {
        Owner = this;
        _settings = settings;
        _appSettings = appSettings;
        _trayMenuService = trayMenuService;
        _config = config;
    }

    /// <summary>
    /// run the animator
    /// </summary>
    public new TrayIconAnimator Run(
        ActionContext context,
        object caller)
    {
        Setup(
            _config,
            (ctx, o, e) => Next(),
            _appSettings.Value.Anims.Interval.WaitTrayIcon);
        base.Run(context, caller);
        return this;
    }

    /// <summary>
    /// setup next animation frame
    /// </summary>
    public void Next()
    {
        var t = _appSettings.Value.Anims.WaitIcons;
        var ico = new Icon(_settings.AssetPath(t[_n]!));
        _trayMenuService.NotifyIcon.Icon = ico;
        if (++_n > t.Length - 1) _n = 0;
    }

    protected override string LogPrefix()
        => LogNativePrefix();
}
