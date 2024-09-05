using System.Diagnostics;

using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;

using Microsoft.Extensions.Configuration;

using static MovieDbAssistant.Dmn.Components.Settings;

namespace MovieDbAssistant.App.Features;

/// <summary>
/// The open command line feature.
/// </summary>
[Singleton]
public sealed class OpenUrl
{
    readonly IConfiguration _config;

    public OpenUrl(IConfiguration configuration)
        => _config = configuration;

    /// <summary>
    /// run the feature
    /// </summary>
    public void Run(string pathKey)
    {
        var path = "\"" + _config[pathKey]! + "\"";
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
