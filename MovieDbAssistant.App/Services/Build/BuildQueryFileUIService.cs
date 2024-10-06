using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MovieDbAssistant.App.Commands;
using MovieDbAssistant.App.Configuration;
using MovieDbAssistant.Dmn.Components;
using MovieDbAssistant.Dmn.Components.Builders;
using MovieDbAssistant.Dmn.Components.Builders.Html;
using MovieDbAssistant.Dmn.Components.DataProviders.Json;
using MovieDbAssistant.Dmn.Configuration;
using MovieDbAssistant.Lib.Components.Actions;
using MovieDbAssistant.Lib.Components.Actions.Events;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Extensions;
using MovieDbAssistant.Lib.Components.Signal;

using static MovieDbAssistant.Dmn.Components.Builders.Html.HtmDocumentBuilderSettings;
using static MovieDbAssistant.Dmn.Components.Settings;
using static MovieDbAssistant.Dmn.Globals;

namespace MovieDbAssistant.App.Services.Build;

/// <summary>
/// The build query file service.
/// </summary>
[Transient]
sealed class BuildQueryFileUIService : 
    BuildUIServiceBase<BuildQueryFileCommand>
{
    readonly DocumentBuilderServiceFactory _documentBuilderServiceFactory;

    public BuildQueryFileUIService(
        ILogger<BuildQueryFileUIService> logger,
        IConfiguration config,
        ISignalR signal,
        IServiceProvider serviceProvider,
        Settings settings,
        Messages messages,
        DocumentBuilderServiceFactory documentBuilderServiceFactory,
        IOptions<DmnSettings> dmnSettings,
        IOptions<AppSettings> appSettings) :        
        base(
            logger,
            config,
            signal,
            serviceProvider,
            settings,
            messages,
            appSettings.Value.Texts.BuildQueryEndWithoutErrors,
            dmnSettings.Value.Texts.ProcFile,
            Item_Id_Build_Query,
            dmnSettings,
            appSettings) => 
                _documentBuilderServiceFactory = documentBuilderServiceFactory;

    /// <summary>
    /// Build from query file.
    /// </summary>
    protected override void Action(ActionContext context) 
        => _documentBuilderServiceFactory.CreateDocumentBuilderService()
        .AddListenerOnce(this,this,Signal)
        .Build(
            context,
            new DocumentBuilderContext(
                Config,
                Logger,
                Com!.Path,
                DmnSettings.Value.Paths.Output,
                DmnSettings.Value.Paths.Resources,
                DmnSettings,
                typeof(JsonQueryFileDataProvider),
                typeof(HtmlDocumentBuilder),
                new Dictionary<string, object>
                {
                    { Template_Id , DmnSettings.Value.Build.Html.TemplateId }
                }));

    /// <inheritdoc/>
    public override void Handle(object sender, ActionSuccessfullyEnded @event)
    {
        if (!@event.Context.Command.HandleUI) return;

        Tray.ShowBalloonTip(
            ActionDoneMessage
            + Path.GetFileName(
                (@event.Context.Command as BuildQueryFileCommand)
                    ?.Path));

        PostHandle(@event);
    }
}
