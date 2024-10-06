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
sealed class OpenUrl : ISignalHandler<OpenUrlCommand>
{
    readonly IConfiguration _config;
    readonly IOptions<AppSettings> _appSettings;

    public OpenUrl(
        IConfiguration configuration,
        IOptions<AppSettings> appSettings)
    {
        _config = configuration;
        _appSettings = appSettings;
    }

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
                FileName = _appSettings.Value.Tools.OpenBrowser.CommandLine,
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
