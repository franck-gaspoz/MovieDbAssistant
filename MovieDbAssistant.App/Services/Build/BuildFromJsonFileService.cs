using MediatR;

using Microsoft.Extensions.Configuration;

using MovieDbAssistant.App.Commands;
using MovieDbAssistant.Dmn.Components.Builders;
using MovieDbAssistant.Dmn.Components.DataProviders;
using MovieDbAssistant.Dmn.Events;
using MovieDbAssistant.Lib.Components.Signal;

using static MovieDbAssistant.Dmn.Components.Settings;
using static MovieDbAssistant.Dmn.Globals;

namespace MovieDbAssistant.App.Services.Build;

/// <summary>
/// The build service.
/// </summary>
sealed class BuildFromJsonFileService : SignalHandlerBase<BuildFromJsonFileCommand>
{
    readonly IConfiguration _config;
    readonly IMediator _mediator;
    readonly Messages _messages;
    readonly DocumentBuilderServiceFactory _documentBuilderServiceFactory;

    public BuildFromJsonFileService(
         IConfiguration config,
         IMediator mediator,
         Messages messages,
         DocumentBuilderServiceFactory documentBuilderServiceFactory)
         => (_config, _mediator, _messages, _documentBuilderServiceFactory, Handler)
            = (config, mediator, messages, documentBuilderServiceFactory,
                (com, _) => Run(com));

    /// <summary>
    /// Build from json file.
    /// </summary>
    public void Run(BuildFromJsonFileCommand com)
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
            _mediator.Send(new BuildErroredEvent(
                this,
                Item_Id_Build_Json,
                ex));
        }
        /*finally
        {
            _mediator.Send(new BuildEndedEvent(Item_Id_Build_Json));
        }*/
    }
}
