using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using MovieDbAssistant.App.Commands;
using MovieDbAssistant.Dmn.Components;
using MovieDbAssistant.Dmn.Events;
using MovieDbAssistant.Lib.Components.Actions;
using MovieDbAssistant.Lib.Components.Actions.Commands;
using MovieDbAssistant.Lib.Components.Actions.Events;
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
        ILogger<BuiIdInputFolderUIService> logger,
        IConfiguration config,
        ISignalR signal,
        IServiceProvider serviceProvider,
        ActionGroup actionGroup,
        Settings settings,
        Messages messages) :
        base(
            logger,
            config,
            signal,
            serviceProvider,
            settings,
            messages,
            InputFolderProcessed,
            ProcInpFold,
            Item_Id_Build_Input) => _actionGroup = actionGroup;

    /// <inheritdoc/>
    protected override void Action(ActionContext context)
    {
        try
        {
            _actionGroup.Clear();

            ProcessJsons(context);
            ProcessLists(context);

            _actionGroup.WaitAll();

            Tray.StopAnimInfo();

            Signal.Send(this, new ActionEndedEvent(context));
        }
        catch (Exception ex)
        {
            Tray.StopAnimInfo();
            Signal.Send(this, new ActionErroredEvent(context, ex));
        }
    }

    /// <inheritdoc/>
    public override void Handle(object sender, ActionFinalisedEvent @event)
    {
    }

    /// <inheritdoc/>
    public override void Handle(object sender, ActionEndedEvent @event)
    {
        base.Handle(sender, @event);
        if (@event.Context.Errors.Any())
        {
            Tray.ShowBalloonTip(InputFolderProcessedWithErrors,icon:ToolTipIcon.Warning);

            var jsonBuildErrors = @event.Context.Errors
                .Where(x => x.Event.Context.Command is BuildFromJsonFileCommand)
                .Select(x => x.Event);

            var jsonBuildLogs = jsonBuildErrors.Select(x =>
                "• "
                + (x.Context.Command is BuildFromJsonFileCommand com
                    ? Path.GetFileName(com.Path)
                    : string.Empty)
                + ": "
                + x.GetError());            

            Messages.Warn(
                Build_End_Input_With_Errors,
                '\n' + string.Join('\n', jsonBuildLogs));
        }
        else
        {
            Tray.ShowBalloonTip(InputFolderProcessed);

            Messages.Info(Build_End_Input_Without_Errors);
        }

        Signal.Send(this, new BuildCompletedEvent(
            Item_Id_Build_Input,
            @event.Context.Command));
    }

    /// <inheritdoc/>
    public override void Handle(object sender, ActionSuccessfullyEnded @event) => base.Handle(sender, @event);

    #region operations

    void ProcessLists(ActionContext context)
    {
        var lists = GetListsFiles();
        lists.ToList()
            .ForEach(file => AddAction(
                context,
                new BuildFromQueryFileCommand(
                    file,
                    NewActionContext(),
                    false)));
    }

    void ProcessJsons(ActionContext context)
    {
        var jsons = GetJsonFiles();
        jsons.ToList()
            .ForEach(file => AddAction(
                context,
                new BuildFromJsonFileCommand(
                    file,
                    NewActionContext(),
                    false)));
    }

    ActionContext NewActionContext()
        => ServiceProvider
            .GetRequiredService<ActionContext>()!;

    void AddAction(
        ActionContext context,
        Lib.Components.Actions.Commands.ActionCommandBase command)
    {
        var key = this.GetKey(ref _counter);
        _actionGroup.Add(key, command);
        // context growing
        command.ActionContext!
            .Setup(_actionGroup, command, [])
            .Merge(this, context);

        Signal.Send(_actionGroup, command);
    }

    #endregion

    IEnumerable<string> GetListsFiles()
        => EnabledFiles(Directory.GetFiles(InputPath, Config[SearchPattern_Txt]!));

    IEnumerable<string> GetJsonFiles()
        => EnabledFiles(Directory.GetFiles(InputPath, Config[SearchPattern_Json]!));
}
