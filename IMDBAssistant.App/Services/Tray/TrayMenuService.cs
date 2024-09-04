using System.ComponentModel;
using System.Diagnostics;

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
    public NotifyIcon NotifyIcon;
    readonly IConfiguration _config;
    BackgroundWorker? _backgroundWorker;
    readonly List<EventHandler> _onBalloonTipClosed = [];
    static int _bwinstance = 0;

    public TrayMenuService(
        IConfiguration config,
        TrayMenuBuilder builder)
    {
        (NotifyIcon, _config) = (builder.NotifyIcon, config);
        NotifyIcon.BalloonTipClosed += NotifyIcon_BalloonTipClosed;
        NotifyIcon.BalloonTipClicked += NotifyIcon_BalloonTipClosed;
    }

    void NotifyIcon_BalloonTipClosed(
        object? sender,
        EventArgs e) {
        Debug.WriteLine("balloon closed");
        foreach (var handler in _onBalloonTipClosed)
            handler?.Invoke(sender,e);
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
        bool stopOnBallonTipClosed = true,
        Action? onBalloonTipClosed = null)
            => RunBackgroundWorker(
                action,
                interval,
                stopOnBallonTipClosed,
                onBalloonTipClosed);

    /// <summary>
    /// Update the info.
    /// </summary>
    /// <param name="key">The key.</param>
    public void UpdateInfo(string text) 
        => NotifyIcon.BalloonTipText = text;

    void BallonTipCloseBackgroundWorkerHandler(object? o, EventArgs e)
    {
        StopAndDestroyBackgroundWorker();
    }

    void RunBackgroundWorker(
        Action<TrayMenuService> action,
        int interval,
        bool stopOnBallonTipClosed = true,
        Action? onBalloonTipClosed = null)
    {
        if (_backgroundWorker != null && _backgroundWorker.IsBusy)
            StopAndDestroyBackgroundWorker();

        if (_backgroundWorker == null)
            _backgroundWorker = new BackgroundWorker
            { WorkerSupportsCancellation = true };

        _onBalloonTipClosed.Remove(BallonTipCloseBackgroundWorkerHandler);
        if (stopOnBallonTipClosed)
            _onBalloonTipClosed.Add(BallonTipCloseBackgroundWorkerHandler);

        _backgroundWorker.DoWork += (o,e) =>
        {
            bool end = false;
            while (!end)
            {
                Debug.Write('.');
                action?.Invoke(this);
                end = e.Cancel;
                if (!end)
                    Thread.Sleep(interval);
            }
        };
        if (!_backgroundWorker.IsBusy)
            _backgroundWorker.RunWorkerAsync();
    }

    void StopAndDestroyBackgroundWorker()
    {
        if (_backgroundWorker is null) return;
        _backgroundWorker.CancelAsync();
        _backgroundWorker.Dispose();
        _backgroundWorker = null;
    }

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
