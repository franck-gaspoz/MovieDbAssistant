using System.Diagnostics;

using Microsoft.Extensions.Configuration;

using MovieDbAssistant.App.Commands;
using MovieDbAssistant.Lib.Components.Signal;

using static MovieDbAssistant.Dmn.Components.Settings;

namespace MovieDbAssistant.App.Features;

/// <summary>
/// The open command line feature.
/// </summary>
sealed class OpenUrl : SignalHandlerBase<OpenUrlCommand>
{
    readonly IConfiguration _config;

    public OpenUrl(IConfiguration configuration)
        => (_config, Handler) =
            (configuration,
            (com, _) => Run(com.Url));

    /// <summary>
    /// run the feature
    /// </summary>
    void Run(string url)
    {
        var path = "\"" + url + "\"";
        var proc = new Process()
        {
            StartInfo = new ProcessStartInfo()
            {
                FileName = _config[OpenBrowser_CommandLine],
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
