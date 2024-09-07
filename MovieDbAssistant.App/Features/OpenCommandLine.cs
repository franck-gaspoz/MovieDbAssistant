using System.Diagnostics;

using Microsoft.Extensions.Configuration;

using MovieDbAssistant.App.Commands;
using MovieDbAssistant.Lib.Components.Signal;

using static MovieDbAssistant.Dmn.Components.Settings;

namespace MovieDbAssistant.App.Features;

/// <summary>
/// The open command line feature.
/// </summary>
sealed class OpenCommandLine : SignalHandlerBase<OpenCommandLineCommand>
{
    readonly IConfiguration _config;

    public OpenCommandLine(IConfiguration configuration)
        => (_config, Handler) =
            (configuration,
            (_, _) => Run());

    /// <summary>
    /// run the feature
    /// </summary>
    public void Run()
    {
        var proc = new Process()
        {
            StartInfo = new ProcessStartInfo()
            {
                FileName = _config[Shell_CommandLine],
                Arguments = _config[Shell_Args],
                UseShellExecute = true,
                RedirectStandardOutput = false,
                RedirectStandardError = false,
                RedirectStandardInput = false,
                WindowStyle = ProcessWindowStyle.Normal
            }
        };
        proc.Start();
    }
}
