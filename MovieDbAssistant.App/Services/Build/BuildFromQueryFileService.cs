using MediatR;

using Microsoft.Extensions.Configuration;

using MovieDbAssistant.App.Commands;
using MovieDbAssistant.App.Components;
using MovieDbAssistant.App.Events;
using MovieDbAssistant.Dmn.Components.Builders;

using static MovieDbAssistant.Dmn.Components.Settings;

namespace MovieDbAssistant.App.Services.Build;

/// <summary>
/// The build service.
/// </summary>
sealed class BuildFromQueryFileService : SignalHandlerBase<BuildFromQueryFileCommand>
{
    readonly IConfiguration _config;
    readonly IServiceProvider _serviceProvider;
    readonly IMediator _mediator;
    readonly Messages _messages;
    readonly DocumentBuilderServiceFactory _documentBuilderServiceFactory;

    public BuildFromQueryFileService(
         IConfiguration config,
         IServiceProvider serviceProvider,
         IMediator mediator,
         Messages messages,
         DocumentBuilderServiceFactory documentBuilderServiceFactory)
         => (_config, _serviceProvider, _mediator, _messages, _documentBuilderServiceFactory, Handler)
            = (config, serviceProvider, mediator, messages, documentBuilderServiceFactory,
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
            _mediator.Send(new BuildEndedEvent(Item_Id_Build_Query));
        }
    }
}
