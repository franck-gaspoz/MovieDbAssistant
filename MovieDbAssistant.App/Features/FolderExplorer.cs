using System.Diagnostics;

using Microsoft.Extensions.Configuration;

using MovieDbAssistant.App.Commands;
using MovieDbAssistant.Lib.Components.Signal;

using static MovieDbAssistant.Dmn.Components.Settings;

namespace MovieDbAssistant.App.Features;

/// <summary>
/// The open command line feature.
/// </summary>
sealed class FolderExplorer : SignalHandlerBase<ExploreFolderCommand>
{
    readonly IConfiguration _config;

    public FolderExplorer(IConfiguration configuration)
        => (_config, Handler) =
            (configuration,
            (com, _) => Run(com.Path));

    /// <summary>
    /// run the feature
    /// </summary>
    void Run(string path)
    {
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
