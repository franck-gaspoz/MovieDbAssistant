using Microsoft.Extensions.Configuration;

using MovieDbAssistant.App.Commands;
using MovieDbAssistant.Dmn.Components.Builders;
using MovieDbAssistant.Dmn.Components.DataProviders;
using MovieDbAssistant.Dmn.Events;
using MovieDbAssistant.Lib.ComponentModels;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.InstanceCounter;
using MovieDbAssistant.Lib.Components.Signal;

using static MovieDbAssistant.Dmn.Components.Settings;

namespace MovieDbAssistant.App.Services.Build;

// TODO: change to a BuildServiceBase
/// <summary>
/// The build service.
/// </summary>
[Scoped]
sealed class BuildFromJsonFileService : ISignalHandler<BuildFromJsonFileCommand>,
    IIdentifiable
{
    /// <summary>
    /// instance id
    /// </summary>
    public SharedCounter InstanceId { get; }

    readonly IConfiguration _config;
    readonly ISignalR _signal;
    readonly Messages _messages;
    readonly DocumentBuilderServiceFactory _documentBuilderServiceFactory;

    public BuildFromJsonFileService(
         IConfiguration config,
         ISignalR signal,
         Messages messages,
         DocumentBuilderServiceFactory documentBuilderServiceFactory)
    {
        InstanceId = new(this);
        (_config, _signal, _messages, _documentBuilderServiceFactory)
            = (config, signal, messages, documentBuilderServiceFactory);
    }

    /// <summary>
    /// Build from json file.
    /// </summary>
    public void Handle(object sender, BuildFromJsonFileCommand com)
    {
        try
        {
            _documentBuilderServiceFactory.CreateDocumentBuilderService()
                .Build(
                    new DocumentBuilderContext(
                        com.Path,
                        _config[Path_Output]!,
                        typeof(HtmlDocumentBuilder),
                        typeof(JsonDataProvider)
                        ));
        }
        catch (Exception ex)
        {
            _signal.Send(
                com.Origin ?? this,
                new ActionErroredEvent(ex));
        }
        /*finally
        {
            _signal.Send(new BuildEndedEvent(Item_Id_Build_Json));
        }*/
    }
}
