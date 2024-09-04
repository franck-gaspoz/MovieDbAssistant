using System.Reflection;

using IMDBAssistant.App.Features;
using IMDBAssistant.Lib.Components.DependencyInjection.Attributes;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IMDBAssistant.App.Services.Tray;

/// <summary>
/// The tray menu.
/// </summary>
[Singleton()]
public sealed class TrayMenuBuilder
{
    #region consts

    public const string IconFile = "IconFile";
    public const string AppTitle = "App:Title";

    public const string Label_Exit = "Texts:Exit";
    public const string Label_OpenCmdLine = "Texts:OpenCmdLine";
    public const string Label_OpenOutpFolder = "Texts:OpenOutpFolder";
    public const string Label_OpenInpFolder = "Texts:OpenInpFolder";
    public const string Label_Help = "Texts:Help";
    public const string Label_Settings = "Texts:Settings";
    public const string Label_BuildQueryFile = "Texts:BuildFromQueryFile";
    public const string Label_BuildJsonFile = "Texts:BuildFromJsonFile";
    public const string Label_BuildClipb = "Texts:BuildFromClipboard";
    public const string Label_BuildFromInputFolder = "Texts:BuildFromInputFolder";

    public const string Path_Assets = "AssetsPath";

    const int MenuHeightAdd = -3 * 26;
    const int ItemWidth = 200;
    const int ItemHeight = 22;
    const int ItemContainerHeight = 24;
    const int MenuWidthAdd = 32;

    #endregion

    #region attrs

    readonly IConfiguration _config;
    readonly IServiceProvider _servicesProvider;
    readonly BuildService _buildService;
    readonly OpenCommandLineFeature _openCommandLineFeature;
    readonly FolderExplorer _folderExplorer;

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

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="TrayMenuBuilder"/> class.
    /// </summary>
    /// <param name="config">The config.</param>
    public TrayMenuBuilder(
        IConfiguration config,
        IServiceProvider servicesProvider,
        BuildService buildService,
        OpenCommandLineFeature openCommandLineFeature,
        FolderExplorer folderExplorer)
    {
        _openCommandLineFeature = openCommandLineFeature;
        _servicesProvider = servicesProvider;
        _config = config;
        _buildService = buildService;
        _folderExplorer = folderExplorer;

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

    string T(string id) => _config[id]!;

    void BuildContextMenu()
    {
        var items = GetMainMenu();
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

    List<ItemDefinition> GetMainMenu()
    {
        var version = Assembly.GetExecutingAssembly().GetName().Version!.ToString();
        var items = new List<ItemDefinition>()
        {
            // deco

            (new ToolStripLabel {
                Text = T(AppTitle)+" "+version,
                BackColor = Color.Black,
                ForeColor = Color.DodgerBlue,
                Padding = new Padding(8),
                Width = ItemWidth,
                AutoSize=false,
            },null),

            // build from file, clipboard
            (new ToolStripSeparator(),null),  // ------ 
            (new ToolStripMenuItem { Text = T(Label_BuildQueryFile) },
            o => { o.Click += new EventHandler((c,e) => {
                _buildService.BuildFromQueryFile(); });}),

            (new ToolStripMenuItem { Text = T(Label_BuildJsonFile) },
            o => { o.Click += new EventHandler((c,e) => {
                _buildService.BuildFromJsonFile(); });}),

            (new ToolStripMenuItem { Text = T(Label_BuildFromInputFolder) },
            o => { o.Click += new EventHandler((c,e) => {
                 _buildService.BuildFromInputFolder(); });}),

            (new ToolStripMenuItem { Text = T(Label_BuildClipb) },
            o => { o.Click += new EventHandler((c,e) => {
                 _buildService.BuildFromClipboard(); });}),

            // tools
            (new ToolStripSeparator(),null),  // ------ 
            (new ToolStripMenuItem { Text = T(Label_OpenCmdLine) },
            o => { o.Click += new EventHandler((c,e) => {
                _openCommandLineFeature.Run();
                 });}),

            (new ToolStripMenuItem { Text = T(Label_OpenOutpFolder) },
            o => { o.Click += new EventHandler((c,e) => {
                _folderExplorer.Run(FolderExplorer.FolderExplorer_Path_Output);
                 });}),

            (new ToolStripMenuItem { Text = T(Label_OpenInpFolder) },
            o => { o.Click += new EventHandler((c,e) => {
                _folderExplorer.Run(FolderExplorer.FolderExplorer_Path_Input);
                 });}),

            // settings, help
            (new ToolStripSeparator(),null),  // ------ 
            (new ToolStripMenuItem { Text = T(Label_Help) },
            o => { o.Click += new EventHandler((c,e) => {
                 });}),

            (new ToolStripMenuItem { Text = T(Label_Settings) },
            o => { o.Click += new EventHandler((c,e) => {
                 });}),

            // exit
            (new ToolStripSeparator(),null), // ------ 
            (new ToolStripMenuItem { Text = T(Label_Exit) },
            o => { o.Click += new EventHandler((c,e) =>  {
                _trayMenu.ShowBalloonTip_End();
                Environment.Exit(0); });})
        };
        return items;
    }
}

record ItemDefinition(ToolStripItem Item, Action<ToolStripItem>? Init)
{
    public static implicit operator (ToolStripItem item, Action<ToolStripItem>? init)(ItemDefinition value) => (value.Item, value.Init);
    public static implicit operator ItemDefinition((ToolStripItem item, Action<ToolStripItem>? init) value) => new(value.item, value.init);
}