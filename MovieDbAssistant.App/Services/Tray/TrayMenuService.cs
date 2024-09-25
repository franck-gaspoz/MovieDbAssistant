﻿using System.Diagnostics;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using MovieDbAssistant.App.Components;
using MovieDbAssistant.App.Components.Tray;
using MovieDbAssistant.Dmn.Components;
using MovieDbAssistant.Lib.Components.Actions;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Extensions;
using MovieDbAssistant.Lib.Components.Signal;

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
    readonly ILogger<TrayMenuService> _logger;
    readonly ISignalR _signal;
    readonly IConfiguration _config;
    readonly TrayBackgroundWorker _trayBackgroundWorker;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="TrayMenuService"/> class.
    /// </summary>
    /// <param name="signal">signal</param>
    /// <param name="config">The config.</param>
    /// <param name="builder">The builder.</param>
    public TrayMenuService(
        ILogger<TrayMenuService> logger,
        ISignalR signal,
        IConfiguration config,
        TrayMenuBuilder builder,
        Settings settings)
    {
        (NotifyIcon, _logger, _signal, _config, _settings) 
            = (builder.NotifyIcon, logger, signal, config, settings);
        _trayMenuBuilder = builder;
        _trayBackgroundWorker = new(
            logger,
            _signal,
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
    /// <param name="context">action context</param>
    /// <param name="caller">caller</param>
    /// <param name="action">The action.</param>
    /// <param name="interval">The interval.</param>
    /// <param name="stopOnBallonTipClosed">If true, stop on ballon tip closed.</param>
    /// <param name="autoRepeat">If true, auto repeat.</param>
    /// <param name="onStop">The on stop.</param>
    public void AnimInfo(
        ActionContext context,
        object caller,
        Action<TrayMenuService> action,
        int? interval = null,
        bool? stopOnBallonTipClosed = null,
        bool? autoRepeat = null,
        Action? onStop = null
        )
        => _trayBackgroundWorker.Run(
            context,
            caller,
            action,
            interval,
            stopOnBallonTipClosed,
            autoRepeat,
            onStop);

    /// <summary>
    /// Anim working info.
    /// </summary>
    /// <param name="context">action contexxt</param>
    /// <param name="caller">caller</param>
    /// <param name="info">The info.</param>
    public void AnimWorkInfo(
        ILogger logger,
        ActionContext context,
        object caller,
        string info)
    {
        // balloon tip status text with anim
        var da = new DotAnimator(_trayMenuBuilder.Tooltip + ":\n" + info);
        // animated tray icon
        var ta = new TrayIconAnimator(_logger,_signal, _config, this, _settings);
        ta.Setup(() =>
        {
            ta.OnStop(this);
            _trayMenuBuilder.SetIcon();
            NotifyIcon.Text = _trayMenuBuilder.Tooltip;
        })
        .Run(context, caller);

        AnimInfo(
            context,
            caller,
            tray =>
            {
                var msg = da.Next();
                tray.NotifyIcon.Text = msg;
            },
            _config.GetInt(Anim_Interval_Dot),
            stopOnBallonTipClosed: false,
            onStop: () =>
            {
                ta.Stop(this);
            });
    }

    /// <summary>
    /// Stop anim info.
    /// </summary>
    public void StopAnimInfo()
        => _trayBackgroundWorker.Stop(this);

    /// <summary>
    /// Ballon tip close background worker handler.
    /// </summary>
    /// <param name="o">sender.</param>
    /// <param name="e">event args</param>
    public void BallonTipCloseBackgroundWorkerHandler(object? o, EventArgs e)
        => _trayBackgroundWorker.Stop(this);

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
