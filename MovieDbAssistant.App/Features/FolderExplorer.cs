using System.Diagnostics;

using Microsoft.Extensions.Configuration;

using MovieDbAssistant.App.Commands;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Signal;

using static MovieDbAssistant.Dmn.Components.Settings;

namespace MovieDbAssistant.App.Features;

/// <summary>
/// The open command line feature.
/// </summary>
[Transient]
sealed class FolderExplorer : ISignalHandler<ExploreFolderCommand>
{
    readonly IConfiguration _config;

    public FolderExplorer(IConfiguration configuration)
        => _config = configuration;

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
                FileName = _config[FolderExplorer_CommandLine],
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
