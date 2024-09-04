using IMDBAssistant.App.Services.Tray;
using IMDBAssistant.Lib.Components.DependencyInjection.Attributes;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IMDBAssistant.App.Features;

/// <summary>
/// process input folder.
/// </summary>
[Singleton()]
public sealed class ProcessInputFolder
{
    public const string Path_Input = "Paths:Input";
    public const string ProcInpFold = "Texts:ProcInpFold";
    const string SearchPatternJson = "*.json";
    const string SearchPatternTxt = "*.txt";
    const char PrefixFileDisabled = '-';
    
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
    }

    /// <summary>
    /// run the feature
    /// </summary>
    public void Run()
    {
        _tray.ShowInfo(_config[ProcInpFold]!);
        ProcessJsons();
        ProcessLists();
    }

    private void ProcessLists()
    {
        var lists = GetListsFiles();
        lists.ToList()
            .ForEach(file => _buildService.BuildFromQueryFile(file));
    }

    private void ProcessJsons()
    {
        var jsons = GetJsonFiles();
        jsons.ToList()
            .ForEach(file => _buildService.BuildFromJsonFile(file));
    }

    IEnumerable<string> GetListsFiles()
        => EnabledFiles(Directory.GetFiles(InputPath, SearchPatternTxt));

    IEnumerable<string> GetJsonFiles()
        => EnabledFiles(Directory.GetFiles(InputPath, SearchPatternJson));

    static bool FileIsDisabled(string x) => x.StartsWith(PrefixFileDisabled);

    static IEnumerable<string> EnabledFiles(IEnumerable<string> files)
        => files.Where(x => !FileIsDisabled(x));
}
