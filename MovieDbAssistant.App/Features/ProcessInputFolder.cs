﻿using MediatR;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MovieDbAssistant.App.Commands;
using MovieDbAssistant.App.Components;
using MovieDbAssistant.App.Events;
using MovieDbAssistant.App.Services;
using MovieDbAssistant.App.Services.Tray;
using MovieDbAssistant.Dmn.Components;
using MovieDbAssistant.Lib.Components;
using MovieDbAssistant.Lib.Components.Extensions;

using static MovieDbAssistant.Dmn.Components.Settings;

namespace MovieDbAssistant.App.Features;

/// <summary>
/// process input folder.
/// </summary>
sealed class ProcessInputFolder : SignalHandlerBase<ProcessInputFolderCommand>
{
    /// <summary>
    /// true if buzy
    /// </summary>
    public static bool Buzy = false;

    readonly IConfiguration _config;
    readonly IMediator _mediator;
    readonly IServiceProvider _serviceProvider;
    readonly Settings _settings;
    readonly Messages _messages;
    readonly BackgroundWorkerWrapper? _backgroundWorker;

    TrayMenuService _tray => _serviceProvider.GetRequiredService<TrayMenuService>();

    /// <summary>
    /// Gets the input path.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string InputPath => Path.Combine(
        Directory.GetCurrentDirectory(),
        _config[Path_Input]!);

    public ProcessInputFolder(
        IConfiguration config,
        IMediator mediator,
        IServiceProvider serviceProvider,
        Settings settings,
        Messages messages)
    {
        _serviceProvider = serviceProvider;
        _settings = settings;
        _messages = messages;
        _mediator = mediator;
        _config = config;
        _backgroundWorker = new(config);
        Handler = (_, _) => Run();
    }

    /// <summary>
    /// run the feature in a background worker
    /// </summary>
    void Run()
    {
        if (Buzy)
        {
            _messages.Warn(Builder_Busy);
            return;
        }
        Buzy = true;
        _backgroundWorker!.RunAction((o, e) => DoWork());
    }

    void DoWork()
    {
        void End(bool error = false)
        {
            _tray.StopAnimInfo();
            if (!error && _config.GetBool(OpenOuputWindowOnBuild))
            {
                _tray.ShowBalloonTip(InputFolderProcessed);
                _mediator.Send(new ExploreFolderCommand(_settings.OutputPath));
            }
            Buzy = false;
        }

        try
        {
            _tray.AnimWorkInfo(_config[ProcInpFold]!);

            ProcessJsons();
            ProcessLists();

            Thread.Sleep(7000);

            End();
        }
        catch (Exception ex)
        {
            End(true);
            _messages.Err(Message_Error_Unhandled, ex.Message);
        }
        finally
        {
            _mediator.Send(new BuildEndedEvent(Item_Id_Build_Input));
        }
    }

    void ProcessLists()
    {
        var lists = GetListsFiles();
        lists.ToList()
            .ForEach(file => _mediator.Send(
                new BuildFromQueryFileCommand(file)));
    }

    void ProcessJsons()
    {
        var jsons = GetJsonFiles();
        jsons.ToList()
            .ForEach(file => _mediator.Send(
                new BuildFromJsonFileCommand(file)));
    }

    IEnumerable<string> GetListsFiles()
        => EnabledFiles(Directory.GetFiles(InputPath, _config[SearchPattern_Txt]!));

    IEnumerable<string> GetJsonFiles()
        => EnabledFiles(Directory.GetFiles(InputPath, _config[SearchPattern_Json]!));

    bool FileIsDisabled(string x) => x.StartsWith(_config[PrefixFileDisabled]!);

    IEnumerable<string> EnabledFiles(IEnumerable<string> files)
        => files.Where(x => !FileIsDisabled(x));
}
