using IMDBAssistant.App.Services.Tray;
using IMDBAssistant.Lib.Components.DependencyInjection.Attributes;

using Microsoft.Extensions.Configuration;

namespace IMDBAssistant.App.Features;

/// <summary>
/// The process input folder.
/// </summary>
[Singleton()]
public sealed class ProcessInputFolder
{
    public const string Path_Input = "Paths:Input";
    public const string ProcInpFold = "Texts:ProcInpFold";

    readonly IConfiguration _config;
    readonly TrayMenuService _tray;

    public string InputPath => Path.Combine(
        Directory.GetCurrentDirectory(),
        _config[Path_Input]!);

    public ProcessInputFolder(IConfiguration config,
        TrayMenuService tray)
    {
        _config = config;
        _tray = tray;
    }

    /// <summary>
    /// run the feature
    /// </summary>
    public void Run()
    {
        _tray.ShowInfo(ProcInpFold);
    }
}
