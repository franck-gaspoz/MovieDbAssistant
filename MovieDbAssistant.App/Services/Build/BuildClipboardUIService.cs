using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MovieDbAssistant.App.Commands;
using MovieDbAssistant.App.Configuration;
using MovieDbAssistant.Dmn.Components.Builders;
using MovieDbAssistant.Dmn.Configuration;
using MovieDbAssistant.Lib.Components.Actions;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Signal;

using static MovieDbAssistant.Dmn.Globals;

namespace MovieDbAssistant.App.Services.Build;

/// <summary>
/// The build service.
/// </summary>
[Transient]
sealed class BuildClipboardUIService : BuildUIServiceBase<BuildClipboardCommand>
{
    readonly DocumentBuilderServiceFactory _documentBuilderServiceFactory;

    public BuildClipboardUIService(
         IConfiguration config,
         ISignalR signal,
         IServiceProvider serviceProvider,
         Messages messages,
         DocumentBuilderServiceFactory documentBuilderServiceFactory,
         ILogger<BuildClipboardUIService> logger,
         IOptions<DmnSettings> dmnSettings,
         IOptions<AppSettings> appSettings) :
        base(
            logger,
            config,
            signal,
            serviceProvider,
            messages,
            appSettings.Value.Texts.ClipboardProcessed,
            appSettings.Value.Texts.ProcClipboard,
            Item_Id_Build_Clipboard,
            dmnSettings,
            appSettings,
            runInBackground: false
            ) => _documentBuilderServiceFactory = documentBuilderServiceFactory;

    /// <summary>
    /// Build from clipboard.
    /// </summary>
    protected override void Action(ActionContext context)
    {
        var query = Clipboard.GetText();
    }
}
