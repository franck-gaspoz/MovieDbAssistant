using Microsoft.Extensions.Configuration;

using MovieDbAssistant.App.Commands;
using MovieDbAssistant.App.Components;
using MovieDbAssistant.Dmn.Components.Builders;
using MovieDbAssistant.Dmn.Components.DataProviders;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;

using static MovieDbAssistant.Dmn.Components.Settings;

namespace MovieDbAssistant.App.Services.Build;

/// <summary>
/// The build service.
/// </summary>
[Singleton]
sealed class BuildFromJsonFileService : CommandHandlerBase<BuildFromQueryFileCommand>
{
    readonly IConfiguration _config;
    readonly IServiceProvider _serviceProvider;
    readonly DocumentBuilderServiceFactory _documentBuilderServiceFactory;

    public BuildFromJsonFileService(
         IConfiguration config,
         IServiceProvider serviceProvider,
         DocumentBuilderServiceFactory documentBuilderServiceFactory)
         => (_config, _serviceProvider, _documentBuilderServiceFactory, Handler)
            = (config, serviceProvider, documentBuilderServiceFactory,
                (com, _) => Run(com.Path));

    /// <summary>
    /// Build from json file.
    /// </summary>
    public void Run(string file)
        => _documentBuilderServiceFactory.CreateDocumentBuilderService()
            .Build(
                new DocumentBuilderContext(
                    file,
                    _config[Path_Output]!,
                    typeof(JsonDataProvider),
                    typeof(HtmlDocumentBuilder)));
}
