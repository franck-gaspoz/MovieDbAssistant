using System.Diagnostics;

using MediatR;

using Microsoft.Extensions.Configuration;

using MovieDbAssistant.App.Commands;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;

using static MovieDbAssistant.Dmn.Components.Settings;

namespace MovieDbAssistant.App.Features;

/// <summary>
/// The open command line feature.
/// </summary>
[Singleton]
public sealed class FolderExplorer : IRequestHandler<ExploreFolderCommand>
{
    readonly IConfiguration _config;

    public FolderExplorer(IConfiguration configuration)
        => _config = configuration;

    /// <summary>
    /// handle command
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/></returns>
    public async Task Handle(
        ExploreFolderCommand request, 
        CancellationToken cancellationToken) 
    {
        _ = cancellationToken;
        Run(request.Path);
        await Task.CompletedTask;
    }

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
