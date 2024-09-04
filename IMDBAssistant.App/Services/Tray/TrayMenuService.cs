using IMDBAssistant.Lib.Components.DependencyInjection.Attributes;

using Microsoft.Extensions.Configuration;

using static IMDBAssistant.Dmn.Components.Settings;

namespace IMDBAssistant.App.Services.Tray;

/// <summary>
/// The tray menu service.
/// </summary>
[Singleton]
public sealed class TrayMenuService
{
    readonly NotifyIcon _notifyIcon;
    readonly IConfiguration _config;

    public TrayMenuService(
        IConfiguration config,
        TrayMenuBuilder builder)
        => (_notifyIcon, _config) = (builder.NotifyIcon, config);

    /// <summary>
    /// Show balloon tip start.
    /// </summary>
    public void ShowBalloonTip_Start()
        => ShowBallonTip(BalloonTip_Start);

    /// <summary>
    /// Show balloon tip end.
    /// </summary>
    public void ShowBalloonTip_End()
        => ShowBallonTip(BalloonTip_End, icon: ToolTipIcon.Warning);

    /// <summary>
    /// Show the info.
    /// </summary>
    /// <param name="key">The key.</param>
    public void ShowInfo(string text)
        => ShowBallonTip(
            text: text,
            icon: ToolTipIcon.Info);

    /// <summary>
    /// Update the info.
    /// </summary>
    /// <param name="key">The key.</param>
    public void UpdateInfo(string key) => _notifyIcon.BalloonTipText = _config[key]!;

    void ShowBallonTip(
        string? key = null,
        string? text = null,
        ToolTipIcon icon = ToolTipIcon.Info) => _notifyIcon.ShowBalloonTip(
            Convert.ToInt32(_config[BalloonTip_Delay]),
            _config[AppTitle]!,
            text ?? _config[key!]!,
            icon);
}
