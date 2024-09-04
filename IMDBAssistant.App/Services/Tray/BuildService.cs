using IMDBAssistant.Dmn.Components.Builders;
using IMDBAssistant.Dmn.Components.DataProviders;
using IMDBAssistant.Lib.Components.DependencyInjection.Attributes;

using Microsoft.Extensions.Configuration;

using static IMDBAssistant.Dmn.Components.Settings;

namespace IMDBAssistant.App.Services.Tray;

/// <summary>
/// The build service.
/// </summary>
[Singleton]
public sealed class BuildService
{
    readonly IConfiguration _config;
    readonly IServiceProvider _serviceProvider;
    readonly DocumentBuilderServiceFactory _documentBuilderServiceFactory;

    public BuildService(
         IConfiguration config,
         IServiceProvider serviceProvider,
         DocumentBuilderServiceFactory documentBuilderServiceFactory)
         => (_config, _serviceProvider, _documentBuilderServiceFactory)
            = (config, serviceProvider, documentBuilderServiceFactory);

    /// <summary>
    /// Build from file.
    /// </summary>
    public void BuildFromQueryFile(string file)
    {

    }

    /// <summary>
    /// Build from json file.
    /// </summary>
    public void BuildFromJsonFile(string file)
        => _documentBuilderServiceFactory.CreateDocumentBuilderService()
            .Build(
                new DocumentBuilderContext(
                    file,
                    _config[Path_Output]!,
                    typeof(JsonDataProvider),
                    typeof(HtmlDocumentBuilder)));

    /// <summary>
    /// Build from clipboard.
    /// </summary>
    public void BuildFromClipboard()
    {
        var query = Clipboard.GetText();
    }
}
