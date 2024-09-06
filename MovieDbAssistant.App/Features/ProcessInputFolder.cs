using MediatR;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MovieDbAssistant.App.Commands;
using MovieDbAssistant.App.Components;
using MovieDbAssistant.App.Services.Tray;
using MovieDbAssistant.Dmn.Components;

using static MovieDbAssistant.Dmn.Components.Settings;
using MovieDbAssistant.Lib.Components.Extensions;

namespace MovieDbAssistant.App.Features;

/// <summary>
/// process input folder.
/// </summary>
sealed class ProcessInputFolder : CommandHandlerBase<ProcessInputFolderCommand>
{
    readonly IConfiguration _config;
    readonly IMediator _mediator;
    readonly IServiceProvider _serviceProvider;
    readonly Settings _settings;

    TrayMenuService _tray => _serviceProvider.GetRequiredService<TrayMenuService>();

    /// <summary>
    /// Gets the input path.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string InputPath => Path.Combine(
        Directory.GetCurrentDirectory(),
        _config[Path_Input]!);

    public ProcessInputFolder(
        IConfiguration config,
        IMediator mediator,
        IServiceProvider serviceProvider,
        Settings settings)
    {
        _serviceProvider = serviceProvider;
        _settings = settings;
        _mediator = mediator;
        _config = config;
        Handler = (_, _) => Run();
    }

    /// <summary>
    /// run the feature
    /// </summary>
    void Run()
    {
        _tray.AnimWorkInfo(_config[ProcInpFold]!);

        ProcessJsons();
        ProcessLists();

        Thread.Sleep(7000);

        _tray.StopAnimInfo();
        _tray.ShowBalloonTip(InputFolderProcessed);
        if (_config.GetBool(OpenOuputWindowOnBuild))
            _mediator.Send(new ExploreFolderCommand(_settings.OutputPath));
    }

    void ProcessLists()
    {
        var lists = GetListsFiles();
        lists.ToList()
            .ForEach(file => _mediator.Send(
                new BuildFromQueryFileCommand(file)));
    }

    void ProcessJsons()
    {
        var jsons = GetJsonFiles();
        jsons.ToList()
            .ForEach(file => _mediator.Send(
                new BuildFromJsonFileCommand(file)));
    }

    IEnumerable<string> GetListsFiles()
        => EnabledFiles(Directory.GetFiles(InputPath, _config[SearchPattern_Txt]!));

    IEnumerable<string> GetJsonFiles()
        => EnabledFiles(Directory.GetFiles(InputPath, _config[SearchPattern_Json]!));

    bool FileIsDisabled(string x) => x.StartsWith(_config[PrefixFileDisabled]!);

    IEnumerable<string> EnabledFiles(IEnumerable<string> files)
        => files.Where(x => !FileIsDisabled(x));
}
