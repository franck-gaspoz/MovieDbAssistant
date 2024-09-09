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
[Scoped]
sealed class OpenUrl : ISignalHandler<OpenUrlCommand>
{
    readonly IConfiguration _config;

    public OpenUrl(IConfiguration configuration)
         => _config = configuration;

    /// <summary>
    /// run the feature
    /// </summary>
    public void Handle(object sender, OpenUrlCommand com)
    {
        var url = com.Url;
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
