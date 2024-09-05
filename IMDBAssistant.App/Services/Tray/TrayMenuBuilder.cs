using IMDBAssistant.App.Services.Tray.Models;
using IMDBAssistant.Lib.Components.DependencyInjection.Attributes;

using Microsoft.Extensions.Configuration;

using static IMDBAssistant.Dmn.Components.Settings;

namespace IMDBAssistant.App.Services.Tray;

/// <summary>
/// The tray menu.
/// </summary>
[Singleton]
public sealed class TrayMenuBuilder
{
    #region consts

    const int MenuHeightAdd = -3 * 26;
    const int ItemWidth = 200;
    const int ItemHeight = 22;
    const int ItemContainerHeight = 24;
    const int MenuWidthAdd = 32;

    #endregion

    #region attrs

    readonly IConfiguration _config;
    readonly string _iconPath = "";
    readonly string _appTitle = "";
    readonly TrayMenuItems _trayMenuItems;

    NotifyIcon? _notifyIcon { get; set; }

    ContextMenuStrip? _contextMenuStrip { get; set; }

    /// <summary>
    /// Gets the notify icon.
    /// </summary>
    /// <value>A <see cref="NotifyIcon"/></value>
    public NotifyIcon NotifyIcon => _notifyIcon!;

    /// <summary>
    /// Gets the context menu strip.
    /// </summary>
    public ContextMenuStrip ContextMenuStrip => _contextMenuStrip!;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="TrayMenuBuilder"/> class.
    /// </summary>
    /// <param name="config">The config.</param>
    public TrayMenuBuilder(
        IConfiguration config,
        TrayMenuItems trayMenuItems
        )
    {
        _trayMenuItems = trayMenuItems;
        _config = config;

        var iconFile = config[IconFile]!;
        _appTitle = config[AppTitle]!;

        _iconPath = Path.GetFullPath(
            Path.Combine(
                Directory.GetCurrentDirectory(),
                config[Path_Assets]!,
                iconFile));
    }

    /// <summary>
    /// builds the tray menu
    /// </summary>
    public TrayMenuBuilder Build()
    {
        _notifyIcon = new()
        {
            Icon = new Icon(_iconPath),
            Visible = true,
            Text = _appTitle,
        };
        BuildContextMenu();
        return this;
    }

    void BuildContextMenu()
    {
        var items = _trayMenuItems.GetMainMenuItems(ItemWidth);
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
