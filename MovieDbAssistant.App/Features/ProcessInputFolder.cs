using MediatR;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MovieDbAssistant.App.Commands;
using MovieDbAssistant.App.Components;
using MovieDbAssistant.App.Services.Tray;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;

using static MovieDbAssistant.Dmn.Components.Settings;

namespace MovieDbAssistant.App.Features;

/// <summary>
/// process input folder.
/// </summary>
[Singleton]
sealed class ProcessInputFolder : CommandHandlerBase<ProcessInputFolderCommand>
{
    readonly IConfiguration _config;
    readonly BuildService _buildService;
    readonly IServiceProvider _serviceProvider;

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
        BuildService buildService,
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _buildService = buildService;
        _config = config;
        Handler = (_,_) => Run();
    }

    /// <summary>
    /// run the feature
    /// </summary>
    void Run()
    {        
        _tray.AnimWorkInfo(_config[ProcInpFold]!);

        ProcessJsons();
        ProcessLists();

        Thread.Sleep(5000);

        _tray.StopAnimInfo();
    }

    void ProcessLists()
    {
        var lists = GetListsFiles();
        lists.ToList()
            .ForEach(file => _buildService.BuildFromQueryFile(file));
    }

    void ProcessJsons()
    {
        var jsons = GetJsonFiles();
        jsons.ToList()
            .ForEach(file => _buildService.BuildFromJsonFile(file));
    }

    IEnumerable<string> GetListsFiles()
        => EnabledFiles(Directory.GetFiles(InputPath, _config[SearchPattern_Txt]!));

    IEnumerable<string> GetJsonFiles()
        => EnabledFiles(Directory.GetFiles(InputPath, _config[SearchPattern_Json]!));

    bool FileIsDisabled(string x) => x.StartsWith(_config[PrefixFileDisabled]!);

    IEnumerable<string> EnabledFiles(IEnumerable<string> files)
        => files.Where(x => !FileIsDisabled(x));
}
