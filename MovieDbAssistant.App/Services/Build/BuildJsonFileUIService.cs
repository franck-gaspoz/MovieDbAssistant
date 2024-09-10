using Microsoft.Extensions.Configuration;

using MovieDbAssistant.App.Commands;
using MovieDbAssistant.Dmn.Components;
using MovieDbAssistant.Dmn.Components.Builders;
using MovieDbAssistant.Dmn.Components.DataProviders;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Signal;

using static MovieDbAssistant.Dmn.Components.Settings;
using static MovieDbAssistant.Dmn.Globals;

namespace MovieDbAssistant.App.Services.Build;

/// <summary>
/// The build service.
/// </summary>
[Scoped]
sealed class BuildJsonFileUIService :
    BuildServiceBase<BuildFromJsonFileCommand>
{
    readonly DocumentBuilderServiceFactory _documentBuilderServiceFactory;

    public BuildJsonFileUIService(
         IConfiguration config,
         ISignalR signal,
         IServiceProvider serviceProvider,
         Settings settings,
         Messages messages,
         DocumentBuilderServiceFactory documentBuilderServiceFactory) :
        base(
            config,
            signal,
            serviceProvider,
            settings,
            messages,
            InputFolderProcessed,
            ProcInpFold,
            Item_Id_Build_Json)
        => _documentBuilderServiceFactory = documentBuilderServiceFactory;

    /// <summary>
    /// Build from json file.
    /// </summary>
    /// <inheritdoc/>
    protected override void Action() =>
            //try
            //{
            _documentBuilderServiceFactory.CreateDocumentBuilderService()
                .Build(
                    new DocumentBuilderContext(
                        Com!.Path,
                        Config[Path_Output]!,
                        typeof(HtmlDocumentBuilder),
                        typeof(JsonDataProvider)
                        ));/*}
        catch (Exception ex)
        {
            Signal.Send(
                com.Origin ?? this,
                new ActionErroredEvent(ex));
        }*//*finally
        {
            _signal.Send(new BuildEndedEvent(Item_Id_Build_Json));
        }*/
}
