using System.Reflection;

using MovieDbAssistant.App.Features;
using MovieDbAssistant.App.Services.Tray.Models;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using static MovieDbAssistant.Dmn.Components.Settings;

namespace MovieDbAssistant.App.Services.Tray;

/// <summary>
/// The tray menu items.
/// </summary>
[Singleton]
public class TrayMenuItems
{
    readonly IConfiguration _config;
    readonly BuildService _buildService;
    readonly OpenCommandLine _openCommandLineFeature;
    readonly OpenUrl _openUrl;
    readonly FolderExplorer _folderExplorer;
    readonly ProcessInputFolder _processInputFolder;
    readonly IServiceProvider _servicesProvider;

    TrayMenuService _trayMenu =>
        _servicesProvider.GetRequiredService<TrayMenuService>();

    public TrayMenuItems(
        IConfiguration config,
        IServiceProvider servicesProvider,
        ProcessInputFolder processInputFolder,
        BuildService buildService,
        OpenCommandLine openCommandLineFeature,
        FolderExplorer folderExplorer,
        OpenUrl openUrl)
    {
        _config = config;
        _servicesProvider = servicesProvider;
        _buildService = buildService;
        _folderExplorer = folderExplorer;
        _processInputFolder = processInputFolder;
        _openUrl = openUrl;
        _openCommandLineFeature = openCommandLineFeature;
    }

    string T(string id) => _config[id]!;

    List<ItemDefinition>? _mainMenuItems;


    /// <summary>
    /// Get main menu items.
    /// </summary>
    /// <param name="itemWidth">The item width.</param>
    /// <returns>A list of <see cref="ItemDefinition"></see></returns>
    public List<ItemDefinition> GetMainMenuItems(int itemWidth)
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
                Width = itemWidth,
                AutoSize=false,
            },null),

            // build from file, clipboard
            (new ToolStripSeparator(),null),  // ------ 
            (new ToolStripMenuItem { Text = T(Label_BuildQueryFile) },
            o => { o.Click += new EventHandler((c,e) => {
                RunBuildAction(
                    () => _buildService.BuildFromQueryFile("")); });}),

            (new ToolStripMenuItem { Text = T(Label_BuildJsonFile) },
            o => { o.Click += new EventHandler((c,e) => {
                RunBuildAction(
                    () => _buildService.BuildFromJsonFile("")); });}),

            (new ToolStripMenuItem { Text = T(Label_BuildFromInputFolder) },
            o => { o.Click += new EventHandler((c,e) => {
                 RunBuildAction(
                    () => _processInputFolder.Run()); });}),

            (new ToolStripMenuItem { Text = T(Label_BuildClipb) },
            o => { o.Click += new EventHandler((c,e) => {
                 RunBuildAction(
                    () => _buildService.BuildFromClipboard()); });}),

            // tools
            (new ToolStripSeparator(),null),  // ------ 
            (new ToolStripMenuItem { Text = T(Label_OpenCmdLine) },
            o => { o.Click += new EventHandler((c,e) => {
                _openCommandLineFeature.Run();
                 });}),

            (new ToolStripMenuItem { Text = T(Label_OpenOutpFolder) },
            o => { o.Click += new EventHandler((c,e) => {
                _folderExplorer.Run(Path_Output);
                 });}),

            (new ToolStripMenuItem { Text = T(Label_OpenInpFolder) },
            o => { o.Click += new EventHandler((c,e) => {
                _folderExplorer.Run(Path_Input);
                 });}),

            // settings, help
            (new ToolStripSeparator(),null),  // ------ 
            (new ToolStripMenuItem { Text = T(Label_Help) },
            o => { o.Click += new EventHandler((c,e) => {
                _openUrl.Run(Url_HelpGitHub);
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
        return _mainMenuItems = items;
    }

    void RunBuildAction(Action act)
    {
        SetBuildItemsEnabled(false);
        try
        {
            act();
        }
        finally {
            SetBuildItemsEnabled(true);
        }
    }

    void SetBuildItemsEnabled(bool isEnabled)
    {
        _mainMenuItems!.ForEach(x =>
        {
            if (x.Item.Text !=null && x.Item.Text.StartsWith("Build"))
                x.Item.Enabled = isEnabled;
        });
    }
}