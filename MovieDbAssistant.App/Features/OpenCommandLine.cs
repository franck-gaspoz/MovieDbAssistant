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
sealed class OpenCommandLine : ISignalHandler<OpenCommandLineCommand>
{
    readonly IConfiguration _config;
    readonly IOptions<AppSettings> _appSettings;

    public OpenCommandLine(
        IConfiguration configuration,
        IOptions<AppSettings> appSettings)
    {
        _config = configuration;
        _appSettings = appSettings;
    }

    /// <summary>
    /// run the feature
    /// </summary>
    public void Handle(
        object sender,
        OpenCommandLineCommand com)
    {
        var proc = new Process()
        {
            StartInfo = new ProcessStartInfo()
            {
                FileName = _appSettings.Value.Tools.Shell.CommandLine,
                Arguments = _appSettings.Value.Tools.Shell.Args ?? string.Empty,
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
