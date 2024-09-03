using Microsoft.Extensions.Configuration;

using IMDBAssistant.Lib.Components.DependencyInjection.Attributes;

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

    readonly IConfiguration _config;

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
    /// </summary>"
    public ContextMenuStrip ContextMenuStrip => _contextMenuStrip!;

    /// <summary>
    /// Initializes a new instance of the <see cref="TrayMenuBuilder"/> class.
    /// </summary>
    /// <param name="config">The config.</param>
    public TrayMenuBuilder(IConfiguration config)
    {
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
            Text = _appTitle
        };

        BuildContextMenu();

        return this;
    }

    void BuildContextMenu()
    {
        string t(string id) => _config[id]!;

        const int itemWidth = 200;
        const int itemHeight = 22;
        const int itemContainerHeight = 24;

        _contextMenuStrip = new()
        {
            BackColor = Color.Black,
            ForeColor = Color.White,
            DropShadowEnabled = true,
            ShowImageMargin = false,
            AutoSize = false,
            Width = itemWidth
        };
        ContextMenuStrip.SuspendLayout();

        var items = new List<(ToolStripItem item, Action<ToolStripItem>? init)>()
        {
            // deco

            (new ToolStripLabel {
                Text = t(AppTitle),
                BackColor = Color.DodgerBlue,
                ForeColor = Color.White,
                Margin = new Padding(0,8,0,0)
            },null),

            (new ToolStripSeparator(),null),

            // exit

            (new ToolStripMenuItem {
                Text = t(Label_Exit)
            },
            o => {
                o.Click += new EventHandler((c,e) =>
                {
                    Environment.Exit(0);
                });
            })
        };

        ContextMenuStrip.Height = itemContainerHeight * items.Count;

        static void addHighlightBehavior(ToolStripItem item)
        {
            item.MouseEnter += new EventHandler((c, e) =>
            {
                item.ForeColor = Color.DarkBlue;
            });
            item.MouseLeave += new EventHandler((c, e) =>
            {
                item.ForeColor = Color.WhiteSmoke;
            });
        }

        items.ForEach(it =>
        {
            var (item, init) = it;

            if (item is not ToolStripLabel)
            {
                item.AutoSize = false;
                item.Size = new Size(itemWidth, itemHeight);
                addHighlightBehavior(item);
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
