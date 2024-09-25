using Microsoft.Extensions.Configuration;

using MovieDbAssistant.App.Commands;
using MovieDbAssistant.Dmn.Components;
using MovieDbAssistant.Dmn.Components.Builders;
using MovieDbAssistant.Lib.Components.Actions;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Signal;

using static MovieDbAssistant.Dmn.Components.Settings;
using static MovieDbAssistant.Dmn.Globals;

namespace MovieDbAssistant.App.Services.Build;

/// <summary>
/// The build service.
/// </summary>
[Scoped]
sealed class BuildClipboardUIService : BuildUIServiceBase<BuildFromClipboardCommand>
{
    readonly DocumentBuilderServiceFactory _documentBuilderServiceFactory;

    public BuildClipboardUIService(
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
            ClipboardProcessed,
            ProcClipboard,
            Item_Id_Build_Clipboard,
            runInBackground: false
            ) => _documentBuilderServiceFactory = documentBuilderServiceFactory;/*OnSuccessMessageAction = context =>
        {
            Messages.Info(
                Build_End_Json_Without_Errors
                + '\n'
                + ((BuildFromJsonFileCommand)context.Command).Path
                );
        };*/

    /// <summary>
    /// Build from clipboard.
    /// </summary>
    protected override void Action(ActionContext context)
    {
        var query = Clipboard.GetText();
    }
}
