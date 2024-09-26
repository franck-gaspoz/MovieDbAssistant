using System.Diagnostics;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using MovieDbAssistant.App.Services;
using MovieDbAssistant.App.Services.Tray;
using MovieDbAssistant.Dmn.Components;
using MovieDbAssistant.Lib.Components.Actions;
using MovieDbAssistant.Lib.Components.Actions.Commands;
using MovieDbAssistant.Lib.Components.Signal;

using static MovieDbAssistant.Dmn.Components.Settings;

namespace MovieDbAssistant.App.Features;

/// <summary>
/// action feature base
/// </summary>
#if DEBUG || TRACE
[DebuggerDisplay("{DbgId()}")]
#endif
abstract class ActionFeatureBase<TCommand> : ActionBase<TCommand>
    where TCommand : ActionCommandBase
{
    #region fields & properties

    protected readonly Settings Settings;
    protected readonly Messages Messages;

    protected TrayMenuService Tray => ServiceProvider
        .GetRequiredService<TrayMenuService>();

    #endregion

    #region build & init

    public ActionFeatureBase(
        ILogger<ActionFeatureBase<TCommand>> logger,
        IConfiguration config,
        ISignalR signal,
        IServiceProvider serviceProvider,
        Settings settings,
        Messages messages,
        string actionOnGoingMessageKey,
        bool runInBackground,
        string textKeyMessageErrorUnhandled = Message_Error_Unhandled,
        string textKeyFeatureIsBuzy = Feature_Busy) : base(
            logger,
            config,
            signal,
            serviceProvider,
            actionOnGoingMessageKey,
            runInBackground,
            textKeyMessageErrorUnhandled,
            textKeyFeatureIsBuzy)
    {
        Settings = settings;
        Messages = messages;

        StoppingAnimInfo += (o, e) => Tray.StopAnimInfo();

        StartingAnimWorkInfo += (o, e) => Tray
            .AnimWorkInfo(
                Logger,
                e.Context!,
                e.Sender,
                Config[ActionOnGoingMessageKey]!);

        MessagesErrorOpening_ErrorUnHandled += (o, e) =>
            Messages.Err(Message_Error_Unhandled, '\n' + e.Text);

        MessageWarningOpening_IsBuzy += (o, e) =>
            Messages.Warn(TextKeyFeatureIsBuzy);
    }

    #endregion
}
