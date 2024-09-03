using Microsoft.Extensions.Configuration;

using IMDBAssistant.Lib.Components.DependencyInjection.Attributes;

namespace IMDBAssistant.App.Services.Tray;

/// <summary>
/// The tray menu service.
/// </summary>
[Singleton()]
public class TrayMenuService
{
    readonly NotifyIcon _notifyIcon;
    readonly IConfiguration _config;

    const string BalloonTip_Start = "BalloonTips:Start";
    const string BalloonTip_Delay = "BalloonTips:Delay";

    public TrayMenuService(
        IConfiguration config,
        TrayMenuBuilder builder)
        => (_notifyIcon, _config) = (builder.NotifyIcon, config);

    /// <summary>
    /// Show balloon tip start.
    /// </summary>
    public void ShowBalloonTip_Start()
    {
        _notifyIcon.ShowBalloonTip(
            Convert.ToInt32(_config[BalloonTip_Delay]),
            _config[TrayMenuBuilder.AppTitle]!,
            _config[BalloonTip_Start]!,
            ToolTipIcon.Info);
    }
}
