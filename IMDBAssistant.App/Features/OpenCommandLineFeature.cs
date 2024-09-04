using System.Diagnostics;

using IMDBAssistant.Lib.Components.DependencyInjection.Attributes;

using Microsoft.Extensions.Configuration;

namespace IMDBAssistant.App.Features;

/// <summary>
/// The open command line feature.
/// </summary>
[Singleton()]
public sealed class OpenCommandLineFeature
{
    public const string Shell_CommandLine = "Shell:CommandLine";
    public const string Shell_Args = "Shell:Args";

    readonly IConfiguration _config;

    public OpenCommandLineFeature(IConfiguration configuration)
        => _config = configuration;

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
