﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using MovieDbAssistant.App.Commands;
using MovieDbAssistant.Dmn.Components;
using MovieDbAssistant.Dmn.Components.Builders;
using MovieDbAssistant.Dmn.Components.Builders.Html;
using MovieDbAssistant.Dmn.Components.DataProviders.Json;
using MovieDbAssistant.Lib.Components.Actions;
using MovieDbAssistant.Lib.Components.Actions.Events;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Signal;

using static MovieDbAssistant.Dmn.Components.Builders.Html.HtmDocumentBuilderSettings;
using static MovieDbAssistant.Dmn.Components.Settings;
using static MovieDbAssistant.Dmn.Globals;

namespace MovieDbAssistant.App.Services.Build;

/// <summary>
/// The build service.
/// </summary>
[Scoped]
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
        DocumentBuilderServiceFactory documentBuilderServiceFactory) :        
        base(
            logger,
            config,
            signal,
            serviceProvider,
            settings,
            messages,
            Build_End_Query_Without_Errors,
            ProcFile,
            Item_Id_Build_Query) => _documentBuilderServiceFactory = documentBuilderServiceFactory;

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
                Config[Path_Output]!,
                Config[Path_Rsc]!,
                typeof(JsonQueryFileDataProvider),
                typeof(HtmlDocumentBuilder),
                new Dictionary<string, object>
                {
                    { Template_Id , Config[Build_Html_Template_Id]! }
                }));

    /// <inheritdoc/>
    public override void Handle(object sender, ActionSuccessfullyEnded @event)
    {
        if (!@event.Context.Command.HandleUI) return;

        Tray.ShowBalloonTip(
            null,
            Config[ActionDoneMessageKey]
            + Path.GetFileName(
                (@event.Context.Command as BuildQueryFileCommand)
                    ?.Path));

        PostHandle(@event);
    }
}
