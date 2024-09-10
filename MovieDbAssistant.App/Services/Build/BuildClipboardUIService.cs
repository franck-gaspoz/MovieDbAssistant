using Microsoft.Extensions.Configuration;

using MovieDbAssistant.App.Commands;
using MovieDbAssistant.Dmn.Components.Builders;
using MovieDbAssistant.Dmn.Events;
using MovieDbAssistant.Lib.ComponentModels;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.InstanceCounter;
using MovieDbAssistant.Lib.Components.Signal;

using static MovieDbAssistant.Dmn.Components.Settings;
using static MovieDbAssistant.Dmn.Globals;

namespace MovieDbAssistant.App.Services.Build;

/// <summary>
/// The build service.
/// </summary>
[Scoped]
sealed class BuildClipboardUIService : ISignalHandler<BuildFromClipboardCommand>,
    IIdentifiable
{
    /// <summary>
    /// instance id
    /// </summary>
    public SharedCounter InstanceId { get; }

    readonly IConfiguration _config;
    readonly IServiceProvider _serviceProvider;
    readonly ISignalR _signal;
    readonly Messages _messages;
    readonly DocumentBuilderServiceFactory _documentBuilderServiceFactory;

    public BuildClipboardUIService(
         IConfiguration config,
         IServiceProvider serviceProvider,
         ISignalR signal,
         Messages messages,
         DocumentBuilderServiceFactory documentBuilderServiceFactory)
    {
        InstanceId = new(this);
        (_config, _serviceProvider, _signal, _messages, _documentBuilderServiceFactory)
            = (config, serviceProvider, signal, messages, documentBuilderServiceFactory);
    }

    /// <summary>
    /// Build from clipboard.
    /// </summary>
    public void Handle(object sender, BuildFromClipboardCommand com)
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
            _signal.Send(this, 
                new BuildCompletedEvent(Item_Id_Build_Clipboard,com));
        }
    }
}
