using MediatR;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MovieDbAssistant.App.Commands;
using MovieDbAssistant.App.Services.Tray.Models;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;

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
    readonly IMediator _mediator;

    TrayMenuService _trayMenu =>
        _servicesProvider.GetRequiredService<TrayMenuService>();

    public TrayMenuItems(
        IConfiguration config,
        IServiceProvider servicesProvider,
        IMediator mediator)
    {
        _mediator = mediator;
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
                Text = T(AppTitle)+" "+version,
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
                Text = T(Label_BuildQueryFile), },
                o => { o.Click += new EventHandler((c,e) => {
                    RunBuildAction(
                        () => _mediator.Send(new BuildFromQueryFileCommand("")));
            });}),

            (new ToolStripMenuItem {
                Tag = Item_Id_Build_Json,
                Text = T(Label_BuildJsonFile) },
                o => { o.Click += new EventHandler((c,e) => {
                    RunBuildAction(
                        () => _mediator.Send(new BuildFromJsonFileCommand("")));
            });}),

            (new ToolStripMenuItem {
                Tag = Item_Id_Build_Input,
                Text = T(Label_BuildFromInputFolder) },
                o => { o.Click += new EventHandler((c,e) => {
                     RunBuildAction(
                        () => _mediator.Send(new ProcessInputFolderCommand()));
            });}),

            (new ToolStripMenuItem {
                Tag = Item_Id_Build_Clipboard,
                Text = T(Label_BuildClipb) },
                o => { o.Click += new EventHandler((c,e) => {
                     RunBuildAction(
                        () => _mediator.Send(new BuildFromClipboardCommand()));
            });}),

            // tools
            (new ToolStripSeparator(),null),  // ------ 
            (new ToolStripMenuItem { Text = T(Label_OpenCmdLine) },
            o => { o.Click += new EventHandler((c,e) => {
                _mediator.Send(new OpenCommandLineCommand());
            });}),

            (new ToolStripMenuItem { Text = T(Label_OpenOutpFolder) },
            o => { o.Click += new EventHandler((c,e) => {
                _mediator.Send(new ExploreFolderCommand(_config[Path_Output]!));
            });}),

            (new ToolStripMenuItem { Text = T(Label_OpenInpFolder) },
            o => { o.Click += new EventHandler((c,e) => {
                _mediator.Send(new ExploreFolderCommand(_config[Path_Input]!));
            });}),

            // settings, help
            (new ToolStripSeparator(),null),  // ------ 
            (new ToolStripMenuItem { Text = T(Label_Help) },
            o => { o.Click += new EventHandler((c,e) => {
                _mediator.Send(new OpenUrlCommand(_config[Url_HelpGitHub]!));
                 });}),

            (new ToolStripMenuItem { Text = T(Label_Settings) },
            o => { o.Click += new EventHandler((c,e) => {
                // TODO: implements
            });}),

            // exit
            (new ToolStripSeparator(),null), // ------ 
            (new ToolStripMenuItem { Text = T(Label_Exit) },
            o => { o.Click += new EventHandler((c,e) =>  {
                _trayMenu.ShowBalloonTip_End();
                _mediator.Send(new ExitCommand());
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