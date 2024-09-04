using System.Diagnostics;

using IMDBAssistant.Lib.Components.DependencyInjection.Attributes;

using Microsoft.Extensions.Configuration;

namespace IMDBAssistant.App.Features;

/// <summary>
/// The open command line feature.
/// </summary>
[Singleton()]
public sealed class FolderExplorer
{
    public const string FolderExplorer_CommandLine = "FolderExplorer:CommandLine";
    public const string FolderExplorer_Path_Output = "FolderExplorer:Paths:Output";
    public const string FolderExplorer_Path_Input = "FolderExplorer:Paths:Input";

    readonly IConfiguration _config;

    public FolderExplorer(IConfiguration configuration)
        => _config = configuration;

    /// <summary>
    /// run the feature
    /// </summary>
    public void Run(string pathKey)
    {
        var path = "\"" 
            + Path.Combine(
                Directory.GetCurrentDirectory(),
                _config[pathKey]!) 
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
