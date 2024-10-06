using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MovieDbAssistant.App.Commands;
using MovieDbAssistant.App.Configuration;
using MovieDbAssistant.App.Features;
using MovieDbAssistant.Dmn.Configuration;
using MovieDbAssistant.Dmn.Configuration.Extensions;
using MovieDbAssistant.Dmn.Events;
using MovieDbAssistant.Lib.Components.Actions;
using MovieDbAssistant.Lib.Components.Actions.Commands;
using MovieDbAssistant.Lib.Components.Actions.Events;
using MovieDbAssistant.Lib.Components.Signal;

namespace MovieDbAssistant.App.Services.Build;

/// <summary>
/// build service base
/// </summary>
abstract class BuildUIServiceBase<TSignal> :
    ActionFeatureBase<TSignal>,
    ISignalHandler<TSignal>,
    ISignalMethodHandler<ActionFinalisedEvent>,
    ISignalMethodHandler<ActionAfterPromptEvent>
    where TSignal : ActionCommandBase
{
    /// <summary>
    /// Gets the input path.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string? InputPath { get; protected set; }

    protected string ActionDoneMessage { get; set; }

    /// <summary>
    /// item id build
    /// </summary>
    protected string ItemIdBuild;

    protected Action<ActionContext>? OnSuccessMessageAction;
    protected Action<ActionContext>? OnErrorMessageAction;
    protected IOptions<DmnSettings> DmnSettings;
    protected IOptions<AppSettings> AppSettings;

    public BuildUIServiceBase(
        ILogger<BuildUIServiceBase<TSignal>> logger,
        IConfiguration config,
        ISignalR signal,
        IServiceProvider serviceProvider,
        Messages messages,
        string actionDoneMessage,
        string actionOnGoingMessage,
        string itemIdBuild,
        IOptions<DmnSettings> dmnSettings,
        IOptions<AppSettings> appSettings,
        string? inputPath = null,
        bool runInBackground = true,
        Action<ActionContext>? onSuccessMessageAction = null,
        Action<ActionContext>? onErrorMessageAction = null) :
            base(
                logger,
                config,
                signal,
                serviceProvider,
                messages,
                appSettings,
                actionOnGoingMessage,
                runInBackground)
    {
        DmnSettings = dmnSettings;
        InputPath = inputPath;
        ItemIdBuild = itemIdBuild;
        AppSettings = appSettings;
        ActionDoneMessage = actionDoneMessage;
        OnSuccessMessageAction = onSuccessMessageAction;
        OnErrorMessageAction = onErrorMessageAction;
    }

    /// <summary>
    /// handle the TSignal action feature command
    /// </summary>
    /// <param name="sender">sender</param>
    /// <param name="signal">signal</param>
    public void Handle(object sender, TSignal signal) => Run(sender, signal);

    /// <inheritdoc/>
    public virtual void Handle(object sender, ActionSuccessfullyEnded @event)
    {
        if (!@event.Context.Command.HandleUI) return;

        Tray.ShowBalloonTip(ActionDoneMessage);

        PostHandle(@event);
    }

    protected virtual void PostHandle(ActionSuccessfullyEnded @event)
    {
        if (!@event.Context.Command.HandleUI) return;

        if (AppSettings.Value.Options.OpenOuputWindowOnBuild)
        {
            Signal.Send(this,
                new ExploreFolderCommand(DmnSettings.Value.OutputPath()));
        }
        OnSuccessMessageAction?.Invoke(@event.Context);
    }

    /// <inheritdoc/>
    public virtual void Handle(object sender, ActionFinalisedEvent @event)
        => Signal.Send(
            this,
            new BuildCompletedEvent(ItemIdBuild, Com!));

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public virtual void Handle(object sender, ActionAfterPromptEvent @event)
    {
        if (@event.Context.Command.HandleUI)
            OnErrorMessageAction?.Invoke(@event.Context);

        Signal.Send(
            this,
            new BuildErroredEvent(ItemIdBuild, Com!));
    }

    /// <summary>
    /// true if a file is disabled by convetion of its name
    /// </summary>
    /// <param name="x">file name</param>
    /// <returns>true if disabled, false otherwise</returns>
    protected bool FileIsDisabled(string x) =>
        Path.GetFileName(x)
            .StartsWith(DmnSettings.Value.Build.PrefixFileDisabled);

    /// <summary>
    /// filter a filename list to keep only enabled ones
    /// </summary>
    /// <param name="files">filenames</param>
    /// <returns>enabled files</returns>
    protected IEnumerable<string> EnabledFiles(IEnumerable<string> files)
        => files.Where(x => !FileIsDisabled(x));
}
