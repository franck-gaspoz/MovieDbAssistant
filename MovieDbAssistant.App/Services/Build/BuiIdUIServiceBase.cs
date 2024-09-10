using Microsoft.Extensions.Configuration;

using MovieDbAssistant.App.Commands;
using MovieDbAssistant.App.Features;
using MovieDbAssistant.Dmn.Components;
using MovieDbAssistant.Dmn.Events;
using MovieDbAssistant.Lib.Components.Actions.Commands;
using MovieDbAssistant.Lib.Components.Extensions;
using MovieDbAssistant.Lib.Components.Signal;

using static MovieDbAssistant.Dmn.Components.Settings;

namespace MovieDbAssistant.App.Services.Build;

/// <summary>
/// build service base
/// </summary>
abstract class BuildServiceBase<TSignal> :
    ActionFeatureBase<TSignal>,
    ISignalHandler<TSignal>
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

    public BuildServiceBase(
        IConfiguration config,
        ISignalR signal,
        IServiceProvider serviceProvider,
        Settings settings,
        Messages messages,
        string actionDoneMessageKey,
        string actionOnGoingMessageKey,
        string itemIdBuild,
        string? inputPath = null) :
            base(
                config,
                signal,
                serviceProvider,
                settings,
                messages,
                actionOnGoingMessageKey,
                true)
    {
        _actionDoneMessageKey = actionDoneMessageKey;
        InputPath = inputPath;
        ItemIdBuild = itemIdBuild;
    }

    public void Handle(object sender, TSignal signal) => Run(signal);

    /// <inheritdoc/>
    protected override void OnSucessEnd()
    {
        if (Config.GetBool(OpenOuputWindowOnBuild))
        {
            Tray.ShowBalloonTip(_actionDoneMessageKey);
            Signal.Send(this, new ExploreFolderCommand(Settings.OutputPath));
        }
    }

    /// <inheritdoc/>
    protected override void OnEnd() { }

    /// <inheritdoc/>
    protected override void OnFinally() => Signal.Send(
        this,
        new BuildCompletedEvent(ItemIdBuild,Com!));

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void OnErrorBeforePrompt() { }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void OnErrorAfterPrompt() => Signal.Send(
        this,
        new BuildErroredEvent(ItemIdBuild,Com!));

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
