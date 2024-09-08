using MediatR;

using Microsoft.Extensions.Configuration;

using MovieDbAssistant.App.Commands;
using MovieDbAssistant.Dmn.Components;

using static MovieDbAssistant.Dmn.Components.Settings;
using static MovieDbAssistant.Dmn.Globals;

namespace MovieDbAssistant.App.Services.Build;

/// <summary>
/// process input folder.
/// </summary>
sealed class BuiIdInputFolderService :
    BuildServiceBase<ProcessInputFolderCommand>
{
    /// <summary>
    /// true if buzy
    /// </summary>
    public static bool Buzy = false;

    /// <summary>
    /// Gets the input path.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public new string InputPath => Path.Combine(
        Directory.GetCurrentDirectory(),
        Config[Path_Input]!);

    public BuiIdInputFolderService(
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
            ProcInpFold,
            Item_Id_Build_Input)
    { }

    /// <inheritdoc/>
    protected override bool IsBuzy() => Buzy;

    /// <inheritdoc/>
    protected override void SetBuzy(bool buzy) => Buzy = buzy;

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
}
