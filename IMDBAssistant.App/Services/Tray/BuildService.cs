using IMDBAssistant.Lib.Components.Builders;
using IMDBAssistant.Lib.Components.DependencyInjection.Attributes;

using Microsoft.Extensions.Configuration;

namespace IMDBAssistant.App.Services.Tray;

/// <summary>
/// The build service.
/// </summary>
[Singleton()]
public sealed class BuildService
{
    readonly Settings _settings;
    readonly IConfiguration _config;
    readonly IServiceProvider _serviceProvider;
    readonly DocumentBuilderServiceFactory _documentBuilderServiceFactory;

    public BuildService(
         IConfiguration config,
         Settings settings,
         IServiceProvider serviceProvider,
         DocumentBuilderServiceFactory documentBuilderServiceFactory)
         => (_settings, _config, _serviceProvider, _documentBuilderServiceFactory)
            = (settings, config, serviceProvider, documentBuilderServiceFactory);

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
            .Build(file, typeof(object), typeof(object));

    /// <summary>
    /// Build from clipboard.
    /// </summary>
    public void BuildFromClipboard()
    {
        var query = Clipboard.GetText();
    }
}
