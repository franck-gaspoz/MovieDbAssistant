using System.Text.Encodings.Web;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MovieDbAssistant.Dmn.Configuration;
using MovieDbAssistant.Dmn.Models.Queries;
using MovieDbAssistant.Dmn.Models.Scrap.Json;
using MovieDbAssistant.Lib.ComponentModels;
using MovieDbAssistant.Lib.Components.Actions;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Errors;
using MovieDbAssistant.Lib.Components.Extensions;
using MovieDbAssistant.Lib.Components.InstanceCounter;
using MovieDbAssistant.Lib.Components.Logger;
using MovieDbAssistant.Lib.Components.Signal;
using MovieDbAssistant.Lib.Components.Sys;

namespace MovieDbAssistant.Dmn.Components.Scrapper;

/// <summary>
/// The movie db scrapper.
/// </summary>
[Transient]
public sealed class MovieDbScrapper : IIdentifiable
{
    readonly ILogger<MovieDbScrapper> _logger;
    readonly ISignalR _signal;
    readonly IConfiguration _config;
    readonly IOptions<DmnSettings> _dmnSettings;
    readonly ProcessWrapper _processWrapper;
    readonly BackgroundWorkerWrapper _worker;
    bool _completed;
    Exception? _exception;

    /// <summary>
    /// Gets the instance id.
    /// </summary>
    /// <value>A <see cref="SharedCounter"/></value>
    public SharedCounter InstanceId { get; }

    public MovieDbScrapper(
        ILogger<MovieDbScrapper> logger,
        ISignalR signal,
        IConfiguration config,
        IOptions<DmnSettings> dmnSettings,
        ProcessWrapper processWrapper
        )
    {
        InstanceId = new(this);
        _logger = logger;
        _signal = signal;
        _config = config;
        _dmnSettings = dmnSettings;
        _processWrapper = processWrapper;
        _worker = new(logger, _signal, this);
        _worker.Setup(
            o => _completed = true);
        _worker.Setup((o, ex) =>
        {
            _exception = ex;
            _completed = true;
        });
    }

    /// <summary>
    /// scrap using the query. block until the end
    /// <para>call MovieDbScrapper</para>
    /// </summary>
    /// <param name="spiderId">spider id</param>
    /// <param name="output">output file path</param>
    /// <param name="filters">eventually filters</param>
    /// <param name="query">The query model.</param>
    /// <returns>A <see cref="MoviesModel"/></returns>
    public MoviesModel? Scrap(
        SpidersIds spiderId,
        string output,
        string? filters,
        QueryModel query)
    {
        _completed = false;
        _worker.RunAction(
            this
            , new ActionContext(
                _signal,
                new StackErrors())
            , (context, owner, eventArgs) =>
            {
                // call a scrap process

                var toolPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    _dmnSettings.Value.Scrap.ToolPath);

                // args

                /// spiderId outputFile title [filters]=
                var args = new List<string>
                {
                    spiderId.ToString().ToLower(),
                    output.DblQuote(),
                    query.Title.DblQuote()
                };
                if (filters != null)
                    args.Add(filters.DblQuote());

                _logger.LogInformation(this,
                    $"scrap: spider={args[0]} title={args[2]} filters={(args.Count > 3 ? args[3] : "")} output={args[1]}");

                _completed = true;
            }
            , _config
            );
        while (!_completed)
        {
            Thread.Yield();
        }
        return null;
    }


}
