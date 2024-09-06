using System.Diagnostics;

using Microsoft.Extensions.Configuration;

using MovieDbAssistant.App.Components;
using MovieDbAssistant.App.Components.Tray;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;

using static MovieDbAssistant.Dmn.Components.Settings;

namespace MovieDbAssistant.App.Services.Tray;

/// <summary>
/// The tray menu service.
/// </summary>
[Singleton]
public sealed class TrayMenuService
{
    readonly TrayMenuBuilder _trayMenuBuilder;

    /// <summary>
    /// Gets the notify icon.
    /// </summary>
    /// <value>A <see cref="NotifyIcon"/></value>
    public NotifyIcon NotifyIcon { get; private set; }

    public event EventHandler? BalloonTipClosed;

    readonly IConfiguration _config;
    readonly TrayBackgroundWorker _trayBackgroundWorker;
    Action<TrayMenuService>? _onStopAnimInfo;

    /// <summary>
    /// Initializes a new instance of the <see cref="TrayMenuService"/> class.
    /// </summary>
    /// <param name="config">The config.</param>
    /// <param name="builder">The builder.</param>
    public TrayMenuService(
        IConfiguration config,
        TrayMenuBuilder builder)
    {
        (NotifyIcon, _config) = (builder.NotifyIcon, config);
        _trayMenuBuilder = builder;
        _trayBackgroundWorker = new(this);
        NotifyIcon.BalloonTipClosed += NotifyIcon_BalloonTipClosed;
        NotifyIcon.BalloonTipClicked += NotifyIcon_BalloonTipClosed;
    }

    void NotifyIcon_BalloonTipClosed(
        object? sender,
        EventArgs e)
    {
#if TRACE
        Debug.WriteLine("balloon closed");
#endif
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
    /// <param name="onStop">on stop action</param>
    /// <param name="interval">The interval.</param>
    public void AnimInfo(
        Action<TrayMenuService> action,
        int interval,
        Action<TrayMenuService>? onStop = null,
        bool stopOnBallonTipClosed = true)
    {
        _onStopAnimInfo = onStop;
        _trayBackgroundWorker.RunBackgroundWorker(
                action,
                interval,
                stopOnBallonTipClosed);
    }

    /// <summary>
    /// Anim working info.
    /// </summary>
    /// <param name="info">The info.</param>
    public void AnimWorkInfo(string info)
    {
        var da = new DotAnim(_trayMenuBuilder.Tooltip + ":\n" + info);
        AnimInfo(
            tray =>
            {
#if TRACE
                var msg = da.Next();
                tray.NotifyIcon.Text = msg;
                Debug.WriteLine(msg);
#endif
            },
            Convert.ToInt32(_config[DotAnimInterval]!),
            tray => tray.NotifyIcon.Text = _trayMenuBuilder.Tooltip,
            false);
    }

    /// <summary>
    /// Stop anim info.
    /// </summary>
    public void StopAnimInfo()
    {
        _trayBackgroundWorker.StopAndDestroyBackgroundWorker();
        _onStopAnimInfo?.Invoke(this);
        _onStopAnimInfo = null;
    }

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
