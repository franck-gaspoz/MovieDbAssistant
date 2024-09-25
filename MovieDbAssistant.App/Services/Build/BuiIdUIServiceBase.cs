using Microsoft.Extensions.Configuration;

using MovieDbAssistant.App.Commands;
using MovieDbAssistant.App.Features;
using MovieDbAssistant.Dmn.Components;
using MovieDbAssistant.Dmn.Events;
using MovieDbAssistant.Lib.Components.Actions;
using MovieDbAssistant.Lib.Components.Actions.Commands;
using MovieDbAssistant.Lib.Components.Actions.Events;
using MovieDbAssistant.Lib.Components.Extensions;
using MovieDbAssistant.Lib.Components.Signal;

using static MovieDbAssistant.Dmn.Components.Settings;

namespace MovieDbAssistant.App.Services.Build;

/// <summary>
/// build service base
/// </summary>
abstract class BuildUIServiceBase<TSignal> :
    ActionFeatureBase<TSignal>,
    ISignalHandler<TSignal>,
    ISignalMethodHandler<ActionFinalisedEvent>,
    ISignalMethodHandler<ActionAfterPromptEvent>
    where TSignal : ActionFeatureCommandBase
{
    /// <summary>
    /// Gets the input path.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string? InputPath { get; protected set; }

    readonly string _actionDoneMessageKey;

    /// <summary>
    /// item id build
    /// </summary>
    protected string ItemIdBuild;

    protected Action<ActionContext>? OnSuccessMessageAction;
    protected Action<ActionContext>? OnErrorMessageAction;

    public BuildUIServiceBase(
        IConfiguration config,
        ISignalR signal,
        IServiceProvider serviceProvider,
        Settings settings,
        Messages messages,
        string actionDoneMessageKey,
        string actionOnGoingMessageKey,
        string itemIdBuild,
        string? inputPath = null,
        bool runInBackground = true,
        Action<ActionContext>? onSuccessMessageAction = null,
        Action<ActionContext>? onErrorMessageAction = null) :
            base(
                config,
                signal,
                serviceProvider,
                settings,
                messages,
                actionOnGoingMessageKey,
                runInBackground)
    {
        InputPath = inputPath;
        ItemIdBuild = itemIdBuild;
        _actionDoneMessageKey = actionDoneMessageKey;
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

        Tray.ShowBalloonTip(_actionDoneMessageKey);

        if (Config.GetBool(OpenOuputWindowOnBuild))
        {
            Signal.Send(this, new ExploreFolderCommand(Settings.OutputPath));
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
    protected bool FileIsDisabled(string x) => x.StartsWith(Config[PrefixFileDisabled]!);

    /// <summary>
    /// filter a filename list to keep only enabled ones
    /// </summary>
    /// <param name="files">filenames</param>
    /// <returns>enabled files</returns>
    protected IEnumerable<string> EnabledFiles(IEnumerable<string> files)
        => files.Where(x => !FileIsDisabled(x));
}
