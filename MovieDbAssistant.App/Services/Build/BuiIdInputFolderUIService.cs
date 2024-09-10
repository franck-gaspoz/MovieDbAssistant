using System.Diagnostics;

using Microsoft.Extensions.Configuration;

using MovieDbAssistant.App.Commands;
using MovieDbAssistant.Dmn.Components;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Signal;

using static MovieDbAssistant.Dmn.Components.Settings;
using static MovieDbAssistant.Dmn.Globals;

namespace MovieDbAssistant.App.Services.Build;

/// <summary>
/// process input folder.
/// </summary>
[Scoped]
sealed class BuiIdInputFolderUIService :
    BuildServiceBase<BuildFromInputFolderCommand>
{
    /// <summary>
    /// Gets the input path.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public new string InputPath => Path.Combine(
        Directory.GetCurrentDirectory(),
        Config[Path_Input]!);

    public BuiIdInputFolderUIService(
        IConfiguration config,
        ISignalR signal,
        IServiceProvider serviceProvider,
        Settings settings,
        Messages messages) :
        base(
            config,
            signal,
            serviceProvider,
            settings,
            messages,
            InputFolderProcessed,
            ProcInpFold,
            Item_Id_Build_Input)
    { }

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
            .ForEach(file => Signal.Send(
                this,
                new BuildFromQueryFileCommand(file, this, false)));
    }

    void ProcessJsons()
    {
        var jsons = GetJsonFiles();
        jsons.ToList()
            .ForEach(file => Signal.Send(
                this,
                new BuildFromJsonFileCommand(file, this, false)));
    }

    IEnumerable<string> GetListsFiles()
        => EnabledFiles(Directory.GetFiles(InputPath, Config[SearchPattern_Txt]!));

    IEnumerable<string> GetJsonFiles()
        => EnabledFiles(Directory.GetFiles(InputPath, Config[SearchPattern_Json]!));
}
