using System.ComponentModel;
using System.Diagnostics;

using IMDBAssistant.App.Components.Tray;
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
    public NotifyIcon NotifyIcon { get; private set; }
    public event EventHandler? BalloonTipClosed;

    readonly IConfiguration _config;
    //readonly List<EventHandler> _onBalloonTipClosed = [];
    readonly TrayBackgroundWorker _trayBackgroundWorker;

    public TrayMenuService(
        IConfiguration config,
        TrayMenuBuilder builder)
    {
        (NotifyIcon, _config) = (builder.NotifyIcon, config);
        _trayBackgroundWorker = new(_config,this);
        NotifyIcon.BalloonTipClosed += NotifyIcon_BalloonTipClosed;
        NotifyIcon.BalloonTipClicked += NotifyIcon_BalloonTipClosed;
    }

    void NotifyIcon_BalloonTipClosed(
        object? sender,
        EventArgs e) {
#if TRACE
        Debug.WriteLine("balloon closed");
#endif
        //foreach (var handler in _onBalloonTipClosed)
        //    handler?.Invoke(sender,e);
        BalloonTipClosed?.Invoke(sender, e);
    }

    /// <summary>
    /// Show balloon tip start.
    /// </summary>
    public void ShowBalloonTip_Start()
        => ShowBalloonTip(BalloonTip_Start);

    /// <summary>
    /// Show balloon tip end.
    /// </summary>
    public void ShowBalloonTip_End()
        => ShowBalloonTip(BalloonTip_End, icon: ToolTipIcon.Warning);

    /// <summary>
    /// Show the info.
    /// </summary>
    /// <param name="key">The key.</param>
    public void ShowInfo(string text)
        => ShowBalloonTip(
            text: text,
            icon: ToolTipIcon.Info);

    /// <summary>
    /// Anims the info.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <param name="interval">The interval.</param>
    public void AnimInfo(
        Action<TrayMenuService> action,
        int interval,
        bool stopOnBallonTipClosed = true)
            => _trayBackgroundWorker.RunBackgroundWorker(
                action,
                interval,
                stopOnBallonTipClosed);

    /// <summary>
    /// Ballon tip close background worker handler.
    /// </summary>
    /// <param name="o">sender.</param>
    /// <param name="e">event args</param>
    public void BallonTipCloseBackgroundWorkerHandler(object? o, EventArgs e)
        => _trayBackgroundWorker.StopAndDestroyBackgroundWorker();

    /// <summary>
    /// Show balloon tip.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="text">The text.</param>
    /// <param name="icon">The icon.</param>
    public void ShowBalloonTip(
        string? key = null,
        string? text = null,
        ToolTipIcon icon = ToolTipIcon.Info) => NotifyIcon.ShowBalloonTip(
            Convert.ToInt32(_config[BalloonTip_Delay]),
            _config[AppTitle]!,
            text ?? _config[key!]!,
            icon);
}
