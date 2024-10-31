using System.Reflection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using MovieDbAssistant.App.Configuration;
using MovieDbAssistant.App.Services.Tray.Models;
using MovieDbAssistant.Dmn.Configuration;
using MovieDbAssistant.Dmn.Configuration.Extensions;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;

namespace MovieDbAssistant.App.Services.Tray;

/// <summary>
/// The tray menu.
/// </summary>
[Singleton]
sealed class TrayMenuBuilder
{
    #region consts

    const int MenuHeightAdd = -3 * 28 - 1;
    const int ItemWidth = 200;
    const int ItemHeight = 22;
    const int ItemContainerHeight = 24;
    const int MenuWidthAdd = 32;

    #endregion

    #region attrs

    /// <summary>
    /// Gets the version.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public static string Version => string.Join(
        '.',
        Assembly.GetExecutingAssembly().GetName()
            .Version!
            .ToString()
            .Split('.')[..^1]);

    /// <summary>
    /// Gets the notify icon.
    /// </summary>
    /// <value>A <see cref="NotifyIcon"/></value>
    public NotifyIcon NotifyIcon => _notifyIcon!;

    /// <summary>
    /// Gets the tool tip.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string Tooltip { get; private set; } = "";

    /// <summary>
    /// Gets the context menu strip.
    /// </summary>
    public ContextMenuStrip ContextMenuStrip => _contextMenuStrip!;

    readonly IConfiguration _config;
    string _iconPath = "";
    readonly TrayMenuItems _trayMenuItems;
    readonly IOptions<AppSettings> _appSettings;

    NotifyIcon? _notifyIcon { get; set; }
    ContextMenuStrip? _contextMenuStrip { get; set; }

    readonly IOptions<DmnSettings> _dmnSettings;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="TrayMenuBuilder"/> class.
    /// </summary>
    /// <param name="config">The config.</param>
    public TrayMenuBuilder(
        IConfiguration config,
        TrayMenuItems trayMenuItems,
        IOptions<DmnSettings> dmnSettings,
        IOptions<AppSettings> appSettings
        )
    {
        _dmnSettings = dmnSettings;
        _trayMenuItems = trayMenuItems;
        _appSettings = appSettings;
        _config = config;

        SetupIcon();
    }

    void SetupIcon()
    {
        var iconFile = _appSettings.Value.Assets.Icons.Tray;
        Tooltip = _dmnSettings.Value.App.Title;
        _iconPath = _dmnSettings.Value.AssetPath(iconFile);
    }

    /// <summary>
    /// Set the icon.
    /// </summary>
    public void SetIcon() => _notifyIcon!.Icon = new Icon(_iconPath);

    /// <summary>
    /// builds the tray menu
    /// </summary>
    public TrayMenuBuilder Build()
    {
        _notifyIcon = new()
        {
            Icon = new Icon(_iconPath),
            Visible = true,
            Text = Tooltip,
        };
        BuildContextMenu();
        return this;
    }

    void BuildContextMenu()
    {
        var items = _trayMenuItems.GetMainMenuItems(Version, ItemWidth);
        BuildMainMenuContainer(items);
        SetupItems(items);

        ContextMenuStrip.Items.AddRange(
            items.Select(x => x.Item)
            .ToArray());

        ContextMenuStrip.ResumeLayout(false);
        NotifyIcon.ContextMenuStrip = ContextMenuStrip;
    }

    static void SetupItems(List<ItemDefinition> items) => items.ForEach(it =>
    {
        var (item, init) = it;

        if (item is not ToolStripLabel
        && item is not ToolStripSeparator)
        {
            item.AutoSize = false;
            item.Size = new Size(ItemWidth + MenuWidthAdd, ItemHeight);
            AddHighlightBehavior(item);
        }
        if (init != null) init!(item);
    });
    void BuildMainMenuContainer(List<ItemDefinition> items)
    {
        _contextMenuStrip = new()
        {
            BackColor = Color.Black,
            ForeColor = Color.White,
            DropShadowEnabled = true,
            ShowImageMargin = false,
            ShowCheckMargin = false,
            AutoSize = false,
            Width = ItemWidth + MenuWidthAdd,
            ShowItemToolTips = true,
        };
        ContextMenuStrip.SuspendLayout();

        ContextMenuStrip.Height = ItemContainerHeight * items.Count + MenuHeightAdd;
        ContextMenuStrip.DropShadowEnabled = true;
        ContextMenuStrip.RenderMode = ToolStripRenderMode.System;
    }

    static void AddHighlightBehavior(ToolStripItem item)
    {
        item.MouseEnter += new EventHandler((c, e) =>
        {
            item.ForeColor = Color.WhiteSmoke;
            item.BackColor = Color.DarkBlue;
        });
        item.MouseLeave += new EventHandler((c, e) =>
        {
            item.ForeColor = Color.WhiteSmoke;
            item.BackColor = Color.Black;
        });
    }
}
