using Microsoft.Extensions.Configuration;

using MovieDbAssistant.App.Commands;
using MovieDbAssistant.App.Components;
using MovieDbAssistant.Dmn.Components.Builders;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;

namespace MovieDbAssistant.App.Services.Build;

/// <summary>
/// The build service.
/// </summary>
[Singleton]
sealed class BuildFromClipboardService : CommandHandlerBase<BuildFromQueryFileCommand>
{
    readonly IConfiguration _config;
    readonly IServiceProvider _serviceProvider;
    readonly DocumentBuilderServiceFactory _documentBuilderServiceFactory;

    public BuildFromClipboardService(
         IConfiguration config,
         IServiceProvider serviceProvider,
         DocumentBuilderServiceFactory documentBuilderServiceFactory)
         => (_config, _serviceProvider, _documentBuilderServiceFactory, Handler)
            = (config, serviceProvider, documentBuilderServiceFactory,
                (_, _) => Run());

    /// <summary>
    /// Build from clipboard.
    /// </summary>
    public void Run()
    {
        var query = Clipboard.GetText();
    }
}
