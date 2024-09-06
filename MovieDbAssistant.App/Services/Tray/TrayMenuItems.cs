using System.Reflection;

using MediatR;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MovieDbAssistant.App.Commands;
using MovieDbAssistant.App.Services.Tray.Models;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;

using static MovieDbAssistant.Dmn.Components.Settings;

namespace MovieDbAssistant.App.Services.Tray;

/// <summary>
/// The tray menu items.
/// </summary>
[Singleton]
public class TrayMenuItems
{
    const string LabelPrefixBuildCommand = "Build";

    readonly IConfiguration _config;
    readonly IServiceProvider _servicesProvider;
    readonly IMediator _mediator;

    TrayMenuService _trayMenu =>
        _servicesProvider.GetRequiredService<TrayMenuService>();

    public TrayMenuItems(
        IConfiguration config,
        IServiceProvider servicesProvider,
        IMediator mediator
    )
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
                    () => _mediator.Send(new BuildFromQueryFileCommand("")));
            });}),

            (new ToolStripMenuItem { Text = T(Label_BuildJsonFile) },
            o => { o.Click += new EventHandler((c,e) => {
                RunBuildAction(
                    () => _mediator.Send(new BuildFromJsonFileCommand("")));
            });}),

            (new ToolStripMenuItem { Text = T(Label_BuildFromInputFolder) },
            o => { o.Click += new EventHandler((c,e) => {
                 RunBuildAction(
                    () => _mediator.Send(new ProcessInputFolderCommand()));
            });}),

            (new ToolStripMenuItem { Text = T(Label_BuildClipb) },
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
        try
        {
            act();
        }
        finally
        {
            SetBuildItemsEnabled(true);
        }
    }

    void SetBuildItemsEnabled(bool isEnabled) =>
        _mainMenuItems!.ForEach(x =>
        {
            if (x.Item.Text != null && x.Item.Text.StartsWith(LabelPrefixBuildCommand))
                x.Item.Enabled = isEnabled;
        });
}