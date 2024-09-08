using MediatR;

using Microsoft.Extensions.Configuration;

using MovieDbAssistant.App.Commands;
using MovieDbAssistant.App.Features;
using MovieDbAssistant.Dmn.Components;
using MovieDbAssistant.Dmn.Events;
using MovieDbAssistant.Lib.Components.Extensions;

using static MovieDbAssistant.Dmn.Components.Settings;
using static MovieDbAssistant.Dmn.Globals;

namespace MovieDbAssistant.App.Services.Build;

/// <summary>
/// process input folder.
/// </summary>
sealed class BuiIdInputFolder :
    ActionFeatureBase,
    IRequestHandler<ProcessInputFolderCommand>
{
    /// <summary>
    /// true if buzy
    /// </summary>
    public static bool Buzy = false;

    /// <summary>
    /// Gets the input path.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string InputPath => Path.Combine(
        Directory.GetCurrentDirectory(),
        Config[Path_Input]!);

    public BuiIdInputFolder(
        IConfiguration config,
        IMediator mediator,
        IServiceProvider serviceProvider,
        Settings settings,
        Messages messages) :
        base(
            config,
            mediator,
            serviceProvider,
            settings,
            messages,
            InputFolderProcessed,
            ProcInpFold)
    { }

    public async Task Handle(
        ProcessInputFolderCommand request,
        CancellationToken cancellationToken)
    {
        CancellationToken = cancellationToken;
        Run();
        await Task.CompletedTask;
    }

    /// <inheritdoc/>
    protected override bool IsBuzy() => Buzy;

    /// <inheritdoc/>
    protected override void SetBuzy(bool buzy) => Buzy = buzy;

    /// <inheritdoc/>
    protected override void OnSucessEnd()
    {
        if (Config.GetBool(OpenOuputWindowOnBuild))
        {
            Tray.ShowBalloonTip(InputFolderProcessed);
            Mediator.Send(new ExploreFolderCommand(Settings.OutputPath));
        }
    }

    /// <inheritdoc/>
    protected override void OnEnd() { }

    /// <inheritdoc/>
    protected override void OnFinally() => Mediator.Send(
        new BuildEndedEvent(this, Item_Id_Build_Input));

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void OnErrorBeforePrompt() { }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void OnErrorAfterPrompt() { }

    /// <inheritdoc/>
    protected override void Action()
    {
        ProcessJsons();
        ProcessLists();

        Thread.Sleep(7000);
    }

    void ProcessLists()
    {
        var lists = GetListsFiles();
        lists.ToList()
            .ForEach(file => Mediator.Send(
                new BuildFromQueryFileCommand(file)));
    }

    void ProcessJsons()
    {
        var jsons = GetJsonFiles();
        jsons.ToList()
            .ForEach(file => Mediator.Send(
                new BuildFromJsonFileCommand(file)));
    }

    IEnumerable<string> GetListsFiles()
        => EnabledFiles(Directory.GetFiles(InputPath, Config[SearchPattern_Txt]!));

    IEnumerable<string> GetJsonFiles()
        => EnabledFiles(Directory.GetFiles(InputPath, Config[SearchPattern_Json]!));

    bool FileIsDisabled(string x) => x.StartsWith(Config[PrefixFileDisabled]!);

    IEnumerable<string> EnabledFiles(IEnumerable<string> files)
        => files.Where(x => !FileIsDisabled(x));
}
