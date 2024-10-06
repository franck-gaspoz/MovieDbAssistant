using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MovieDbAssistant.App.Commands;
using MovieDbAssistant.App.Configuration;
using MovieDbAssistant.Dmn.Components;
using MovieDbAssistant.Dmn.Configuration;
using MovieDbAssistant.Dmn.Events;
using MovieDbAssistant.Lib.Components.Actions;
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
    BuildUIServiceBase<BuildInputFolderCommand>
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
        DmnSettings.Value.Paths.Input);

    #endregion

    public BuiIdInputFolderUIService(
        ILogger<BuiIdInputFolderUIService> logger,
        IConfiguration config,
        ISignalR signal,
        IServiceProvider serviceProvider,
        ActionGroup actionGroup,
        Settings settings,
        Messages messages,
        IOptions<DmnSettings> dmnSettings,
        IOptions<AppSettings> appSettings) :
        base(
            logger,
            config,
            signal,
            serviceProvider,
            settings,
            messages,
            appSettings.Value.Texts.InputFolderProcessed,
            appSettings.Value.Texts.ProcInpFold,
            Item_Id_Build_Input,
            dmnSettings,
            appSettings) => _actionGroup = actionGroup;

    /// <inheritdoc/>
    protected override void Action(ActionContext context)
    {
        try
        {
            _actionGroup.Clear();

            ProcessJsons(context);
            ProcessQueries(context);

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
            Tray.ShowBalloonTip(
                AppSettings.Value.Texts.InputFolderProcessedWithErrors, 
                icon: ToolTipIcon.Warning);

            var jsonBuildErrors = @event.Context.Errors
                .Where(x => x!=null && x.Event.Context.Command is ICommandWithPath)
                .Select(x => x.Event);

            var jsonBuildLogs = jsonBuildErrors.Select(x =>
                "• "
                + (x.Context.Command is ICommandWithPath com
                    ? Path.GetFileName(com.Path)
                    : string.Empty)
                + ": "
                + x.GetError());

            Messages.Warn(
                AppSettings.Value.Texts.BuildInputEndWithErrors
                + '\n' + string.Join('\n', jsonBuildLogs));
        }
        else
        {
            Messages.Info(AppSettings.Value.Texts.BuildInputEndWithoutErrors);
        }

        Signal.Send(this, new BuildCompletedEvent(
            Item_Id_Build_Input,
            @event.Context.Command));
    }

    /// <inheritdoc/>
    public override void Handle(object sender, ActionSuccessfullyEnded @event) => base.Handle(sender, @event);

    #region operations

    void ProcessQueries(ActionContext context)
    {
        var lists = GetQueriesFiles();
        lists.ToList()
            .ForEach(file => AddAction(
                context,
                new BuildQueryFileCommand(
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
                new BuildJsonFileCommand(
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

    IEnumerable<string> GetQueriesFiles()
        => EnabledFiles(Directory.GetFiles(InputPath, Config[SearchPattern_Txt]!));

    IEnumerable<string> GetJsonFiles()
        => EnabledFiles(Directory.GetFiles(InputPath, Config[SearchPattern_Json]!));
}
