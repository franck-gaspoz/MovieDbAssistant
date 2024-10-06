using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using MovieDbAssistant.App.Commands;
using MovieDbAssistant.App.Configuration;
using MovieDbAssistant.App.Services.Tray.Models;
using MovieDbAssistant.Dmn.Configuration;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Signal;

using static MovieDbAssistant.Dmn.Components.Settings;
using static MovieDbAssistant.Dmn.Globals;

namespace MovieDbAssistant.App.Services.Tray;

/// <summary>
/// The tray menu items.
/// </summary>
[Singleton]
sealed class TrayMenuItems
{
    readonly IConfiguration _config;
    readonly IServiceProvider _servicesProvider;
    readonly ISignalR _signal;
    readonly IOptions<AppSettings> _appSettings;
    readonly IOptions<DmnSettings> _dmnSettings;

    TrayMenuService _trayMenu =>
        _servicesProvider.GetRequiredService<TrayMenuService>();

    public TrayMenuItems(
        IConfiguration config,
        IServiceProvider servicesProvider,
        ISignalR signal,
        IOptions<DmnSettings> dmnSettings,
        IOptions<AppSettings> appSettings)
    {
        _dmnSettings = dmnSettings;
        _signal = signal;
        _appSettings = appSettings;
        _config = config;
        _servicesProvider = servicesProvider;
    }

    string T(string id) => _config[id]!;

    List<ItemDefinition>? _mainMenuItems;

    /// <summary>
    /// Get main menu items.
    /// </summary>
    /// <param name="itemWidth">The item width.</param>
    /// <returns>A list of <see cref="ItemDefinition"></see></returns>
    public List<ItemDefinition> GetMainMenuItems(
        string version,
        int itemWidth)
    {
        var items = new List<ItemDefinition>()
        {
            // deco

            (new ToolStripLabel {
                Text = _dmnSettings.Value.App.Title+" "+version,
                BackColor = Color.Black,
                ForeColor = Color.DodgerBlue,
                Padding = new Padding(8),
                Width = itemWidth,
                AutoSize=false,
            },null),

            // build from file, clipboard
            (new ToolStripSeparator(),null),  // ------ 
            (new ToolStripMenuItem {
                Tag = Item_Id_Build_Query,
                Text = _appSettings.Value.Texts.BuildFromQueryFile },
                o => { o.Click += new EventHandler((c,e) => {
                    RunBuildAction(
                        () => _signal.Send(this,new BuildQueryFileCommand("")));
            });}),

            (new ToolStripMenuItem {
                Tag = Item_Id_Build_Json,
                Text = _appSettings.Value.Texts.BuildFromJsonFile },
                o => { o.Click += new EventHandler((c,e) => {
                    RunBuildAction(
                        () => _signal.Send(this, new BuildJsonFileCommand("")));
            });}),

            (new ToolStripMenuItem {
                Tag = Item_Id_Build_Input,
                Text = _appSettings.Value.Texts.BuildFromInputFolder },
                o => { o.Click += new EventHandler((c,e) => {
                     RunBuildAction(
                        () => _signal.Send(this, new BuildInputFolderCommand()));
            });}),

            (new ToolStripMenuItem {
                Tag = Item_Id_Build_Clipboard,
                Text = _appSettings.Value.Texts.BuildFromClipboard },
                o => { o.Click += new EventHandler((c,e) => {
                     RunBuildAction(
                        () => _signal.Send(this, new BuildClipboardCommand()));
            });}),

            // tools
            (new ToolStripSeparator(),null),  // ------ 
            (new ToolStripMenuItem { Text = _appSettings.Value.Texts.OpenCmdLine },
            o => { o.Click += new EventHandler((c,e) => {
                _signal.Send(this, new OpenCommandLineCommand());
            });}),

            (new ToolStripMenuItem { Text = _appSettings.Value.Texts.OpenOutpFolder },
            o => { o.Click += new EventHandler((c,e) => {
                _signal.Send(this, new ExploreFolderCommand(_dmnSettings.Value.Paths.Output));
            });}),

            (new ToolStripMenuItem { Text = _appSettings.Value.Texts.OpenInpFolder },
            o => { o.Click += new EventHandler((c,e) => {
                _signal.Send(this, new ExploreFolderCommand(_dmnSettings.Value.Paths.Input));
            });}),

            // settings, help
            (new ToolStripSeparator(),null),  // ------ 
            (new ToolStripMenuItem { Text = _appSettings.Value.Texts.Help },
            o => { o.Click += new EventHandler((c,e) => {
                _signal.Send(this, new OpenUrlCommand(_config[Url_HelpGitHub]!));
                 });}),

            (new ToolStripMenuItem { Text = _appSettings.Value.Texts.Settings },
            o => { o.Click += new EventHandler((c,e) => {
                // TODO: implements
            });}),

            // exit
            (new ToolStripSeparator(),null), // ------ 
            (new ToolStripMenuItem { Text = _appSettings.Value.Texts.Exit },
            o => { o.Click += new EventHandler((c,e) =>  {
                _trayMenu.ShowBalloonTip_End();
                _signal.Send(this, new ExitCommand());
            });})
        };
        return _mainMenuItems = items;
    }

    void RunBuildAction(Action act)
    {
        SetBuildItemsEnabled(false);
        act();
    }

    /// <summary>
    /// set build items enabled states
    /// </summary>
    /// <param name="isEnabled">is enabled</param>
    public void SetBuildItemsEnabled(bool isEnabled) =>
        _mainMenuItems!.ForEach(x =>
        {
            if (x.Item.Tag != null
                && ((string)x.Item.Tag)
                    .StartsWith(Item_Id_Build))
                x.Item.Enabled = isEnabled;
        });
}