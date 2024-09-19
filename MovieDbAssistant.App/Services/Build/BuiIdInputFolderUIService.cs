using Microsoft.Extensions.Configuration;

using MovieDbAssistant.App.Commands;
using MovieDbAssistant.Dmn.Components;
using MovieDbAssistant.Lib.Components.Actions;
using MovieDbAssistant.Lib.Components.Actions.Commands;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Extensions;
using MovieDbAssistant.Lib.Components.Signal;

using static MovieDbAssistant.Dmn.Components.Settings;
using static MovieDbAssistant.Dmn.Globals;

namespace MovieDbAssistant.App.Services.Build;

/// <summary>
/// process input folder.
/// </summary>
[Scoped]
sealed class BuiIdInputFolderUIService :
    BuildUIServiceBase<BuildFromInputFolderCommand>
{
    #region fields & properties

    readonly ActionGroup _actionGroup;
    static int _counter = 0;

    /// <summary>
    /// Gets the input path.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public new string InputPath => Path.Combine(
        Directory.GetCurrentDirectory(),
        Config[Path_Input]!);

    #endregion

    public BuiIdInputFolderUIService(
        IConfiguration config,
        ISignalR signal,
        IServiceProvider serviceProvider,
        ActionGroup actionGroup,
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
    {
        _actionGroup = actionGroup;
    }

    /// <inheritdoc/>
    protected override void Action(ActionContext context)
    {
        _actionGroup.Clear();

        ProcessJsons();
        ProcessLists();

        _actionGroup.WaitAll();

        Thread.Sleep(7000);
    }

    void ProcessLists()
    {
        var lists = GetListsFiles();
        lists.ToList()
            .ForEach(file => AddAction(
                new BuildFromQueryFileCommand(file, null, false)));
    }

    void ProcessJsons()
    {
        var jsons = GetJsonFiles();
        jsons.ToList()
            .ForEach(file => AddAction(
                new BuildFromJsonFileCommand(file, null, false)));
    }

    void AddAction(ActionFeatureCommandBase command)
    {
        var key = this.GetKey(ref _counter);
        _actionGroup.Add(key, command);
        Signal.Send(_actionGroup, command);
    }

    IEnumerable<string> GetListsFiles()
        => EnabledFiles(Directory.GetFiles(InputPath, Config[SearchPattern_Txt]!));

    IEnumerable<string> GetJsonFiles()
        => EnabledFiles(Directory.GetFiles(InputPath, Config[SearchPattern_Json]!));
}
