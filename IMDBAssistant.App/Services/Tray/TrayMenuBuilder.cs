using Microsoft.Extensions.Configuration;

using IMDBAssistant.Lib.Components.DependencyInjection.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace IMDBAssistant.App.Services.Tray;

/// <summary>
/// The tray menu.
/// </summary>
[Singleton()]
public sealed class TrayMenuBuilder
{
    public const string IconFile = "IconFile";
    public const string AppTitle = "App:Title";
    public const string Label_Exit = "Texts:Exit";
    public const string Path_Assets = "AssetsPath";

    const int MenuHeightAdd = -16;
    const int ItemWidth = 200;
    const int ItemHeight = 22;
    const int ItemContainerHeight = 24;
    const int MenuWidthAdd = 32;

    readonly IConfiguration _config;
    readonly IServiceProvider _servicesProvider;
    readonly string _iconPath = "";
    readonly string _appTitle = "";

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

    /// <summary>
    /// Initializes a new instance of the <see cref="TrayMenuBuilder"/> class.
    /// </summary>
    /// <param name="config">The config.</param>
    public TrayMenuBuilder(
        IConfiguration config,
        IServiceProvider servicesProvider)
    {
        _servicesProvider = servicesProvider;
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

    TrayMenuService _trayMenu =>
        _servicesProvider.GetRequiredService<TrayMenuService>();

    void BuildContextMenu()
    {
        string t(string id) => _config[id]!;

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

        var items = new List<(ToolStripItem item, Action<ToolStripItem>? init)>()
        {
            // deco

            (new ToolStripLabel {
                Text = t(AppTitle),
                BackColor = Color.Black,
                ForeColor = Color.DodgerBlue,
                Padding = new Padding(8),
                Width = ItemWidth,
                AutoSize=false,
            },null),

            // separator

            (new ToolStripSeparator(),null),

            // test ballon

            (new ToolStripMenuItem {
                Text = "balloon"
            },
            o => {
                o.Click += new EventHandler((c,e) =>
                {
                    _trayMenu.ShowBalloonTip_Start();
                });
            }),

            // exit

            (new ToolStripMenuItem {
                Text = t(Label_Exit)
            },
            o => {
                o.Click += new EventHandler((c,e) =>
                {
                    _trayMenu.ShowBalloonTip_End();
                    Environment.Exit(0);
                });
            })
        };

        ContextMenuStrip.Height = ItemContainerHeight * (items.Count) + MenuHeightAdd;
        ContextMenuStrip.DropShadowEnabled = true;
        ContextMenuStrip.RenderMode = ToolStripRenderMode.System;

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

        items.ForEach(it =>
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

        ContextMenuStrip.Items.AddRange(
            items.Select(x => x.item)
            .ToArray());

        ContextMenuStrip.Name = "TrayIconContextMenu";

        ContextMenuStrip.ResumeLayout(false);
        NotifyIcon.ContextMenuStrip = ContextMenuStrip;
    }
}
