using MediatR;

using Microsoft.Extensions.Configuration;

using MovieDbAssistant.App.Commands;
using MovieDbAssistant.Dmn.Components.Builders;
using MovieDbAssistant.Dmn.Events;
using MovieDbAssistant.Lib.Components.Signal;

using static MovieDbAssistant.Dmn.Components.Settings;
using static MovieDbAssistant.Dmn.Globals;

namespace MovieDbAssistant.App.Services.Build;

/// <summary>
/// The build service.
/// </summary>
sealed class BuildFromClipboardService : SignalHandlerBase<BuildFromClipboardCommand>
{
    readonly IConfiguration _config;
    readonly IServiceProvider _serviceProvider;
    readonly IMediator _mediator;
    readonly Messages _messages;
    readonly DocumentBuilderServiceFactory _documentBuilderServiceFactory;

    public BuildFromClipboardService(
         IConfiguration config,
         IServiceProvider serviceProvider,
         IMediator mediator,
         Messages messages,
         DocumentBuilderServiceFactory documentBuilderServiceFactory)
         => (_config, _serviceProvider, _mediator, _messages, _documentBuilderServiceFactory, Handler)
            = (config, serviceProvider, mediator, messages, documentBuilderServiceFactory,
                (_, _) => Run());

    /// <summary>
    /// Build from clipboard.
    /// </summary>
    public void Run()
    {
        try
        {
            var query = Clipboard.GetText();
        }
        catch (Exception ex)
        {
            _messages.Err(Message_Error_Unhandled, ex.Message);
        }
        finally
        {
            _mediator.Send(new BuildEndedEvent(this, Item_Id_Build_Clipboard));
        }
    }
}
