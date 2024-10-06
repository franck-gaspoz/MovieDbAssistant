using System.Diagnostics;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using MovieDbAssistant.App.Commands;
using MovieDbAssistant.App.Configuration;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Signal;

namespace MovieDbAssistant.App.Features;

/// <summary>
/// The open command line feature.
/// </summary>
[Scoped]
sealed class FolderExplorer : ISignalHandler<ExploreFolderCommand>
{
    readonly IConfiguration _config;
    readonly IOptions<AppSettings> _appSettings;

    public FolderExplorer(
        IConfiguration configuration,
        IOptions<AppSettings> appSettings)
    {
        _config = configuration;
        _appSettings = appSettings;
    }

    /// <summary>
    /// run the feature
    /// </summary>
    public void Handle(object sender, ExploreFolderCommand signal)
    {
        var path = signal.Path;
        path = "\""
            + Path.Combine(
                Directory.GetCurrentDirectory(),
                path)
            + "\"";
        var proc = new Process()
        {
            StartInfo = new ProcessStartInfo()
            {
                FileName = _appSettings.Value.Tools.FolderExplorer.CommandLine,
                Arguments = path,
                UseShellExecute = true,
                RedirectStandardOutput = false,
                RedirectStandardError = false,
                RedirectStandardInput = false,
                CreateNoWindow = true,
            }
        };
        proc.Start();
    }
}
