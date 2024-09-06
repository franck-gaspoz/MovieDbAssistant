using System.Diagnostics;

using Microsoft.Extensions.Configuration;

using MovieDbAssistant.App.Components;
using MovieDbAssistant.App.Components.Tray;
using MovieDbAssistant.Dmn.Components;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Extensions;

using static MovieDbAssistant.Dmn.Components.Settings;

namespace MovieDbAssistant.App.Services.Tray;

/// <summary>
/// The tray menu service.
/// </summary>
[Singleton]
sealed class TrayMenuService
{
    #region attrs

    /// <summary>
    /// Gets the notify icon.
    /// </summary>
    /// <value>A <see cref="NotifyIcon"/></value>
    public NotifyIcon NotifyIcon { get; private set; }

    /// <summary>
    /// tooltip balloon closed event handler
    /// </summary>
    public event EventHandler? BalloonTipClosed;

    readonly TrayMenuBuilder _trayMenuBuilder;
    readonly Settings _settings;
    readonly IConfiguration _config;
    readonly TrayBackgroundWorker _trayBackgroundWorker;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="TrayMenuService"/> class.
    /// </summary>
    /// <param name="config">The config.</param>
    /// <param name="builder">The builder.</param>
    public TrayMenuService(
        IConfiguration config,
        TrayMenuBuilder builder,
        Settings settings)
    {
        (NotifyIcon, _config, _settings) = (builder.NotifyIcon, config, settings);
        _trayMenuBuilder = builder;
        _trayBackgroundWorker = new(
            _config,
            this,
            _config.GetInt(Anim_Interval_Dot),
            false);
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
    /// <param name="interval">The interval.</param>
    /// <param name="stopOnBallonTipClosed">If true, stop on ballon tip closed.</param>
    /// <param name="autoRepeat">If true, auto repeat.</param>
    /// <param name="onStop">The on stop.</param>
    public void AnimInfo(
        Action<TrayMenuService> action,
        int? interval = null,
        bool? stopOnBallonTipClosed = null,
        bool? autoRepeat = null,
        Action? onStop = null
        )
        => _trayBackgroundWorker.Run(
                action,
                interval,
                stopOnBallonTipClosed,
                autoRepeat,
                onStop);

    /// <summary>
    /// Anim working info.
    /// </summary>
    /// <param name="info">The info.</param>
    public void AnimWorkInfo(string info)
    {
        // balloon tip status text with anim
        var da = new DotAnimator(_trayMenuBuilder.Tooltip + ":\n" + info);
        // animated tray icon
        var ta = new TrayIconAnimator(_config, this, _settings)
            .Run();

        AnimInfo(
            tray =>
            {
                var msg = da.Next();
#if TRACE
                Debug.WriteLine(msg);
#endif
                tray.NotifyIcon.Text = msg;
            },
            _config.GetInt(Anim_Interval_Dot),
            stopOnBallonTipClosed: false,
            onStop: () =>
            {
                ta.Stop();
                _trayMenuBuilder.SetIcon();
                NotifyIcon.Text = _trayMenuBuilder.Tooltip;
            });
    }

    /// <summary>
    /// Stop anim info.
    /// </summary>
    public void StopAnimInfo()
        => _trayBackgroundWorker.Stop();

    /// <summary>
    /// Ballon tip close background worker handler.
    /// </summary>
    /// <param name="o">sender.</param>
    /// <param name="e">event args</param>
    public void BallonTipCloseBackgroundWorkerHandler(object? o, EventArgs e)
        => _trayBackgroundWorker.Stop();

    /// <summary>
    /// Show balloon tip.
    /// </summary>
    /// <param name="key">key. if kery not null, text is ignored</param>
    /// <param name="text">text if key is null. is ignored if key is not null</param>
    /// <param name="icon">The icon.</param>
    public void ShowBalloonTip(
        string? key = null,
        string? text = null,
        ToolTipIcon icon = ToolTipIcon.Info) => NotifyIcon.ShowBalloonTip(
            _config.GetInt(BalloonTip_Delay),
            _config[AppTitle]!,
            text ?? _config[key!]!,
            icon);
}
