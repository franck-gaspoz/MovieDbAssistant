using Microsoft.Extensions.Configuration;

using MovieDbAssistant.App.Commands;
using MovieDbAssistant.App.Components;
using MovieDbAssistant.Dmn.Components.Builders;

namespace MovieDbAssistant.App.Services.Build;

/// <summary>
/// The build service.
/// </summary>
sealed class BuildFromQueryFileService : CommandHandlerBase<BuildFromQueryFileCommand>
{
    readonly IConfiguration _config;
    readonly IServiceProvider _serviceProvider;
    readonly DocumentBuilderServiceFactory _documentBuilderServiceFactory;

    public BuildFromQueryFileService(
         IConfiguration config,
         IServiceProvider serviceProvider,
         DocumentBuilderServiceFactory documentBuilderServiceFactory)
         => (_config, _serviceProvider, _documentBuilderServiceFactory, Handler)
            = (config, serviceProvider, documentBuilderServiceFactory,
                (com, _) => Run(com.Path));

    /// <summary>
    /// Build from query file.
    /// </summary>
    public void Run(string file) => _ = file;

    /// <summary>
    /// Build from clipboard.
    /// </summary>
    public void BuildFromClipboard()
    {
        var query = Clipboard.GetText();
    }
}
