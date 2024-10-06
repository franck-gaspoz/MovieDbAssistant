using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using MovieDbAssistant.Dmn.Models.Queries;
using MovieDbAssistant.Dmn.Models.Scrap.Json;
using MovieDbAssistant.Lib.ComponentModels;
using MovieDbAssistant.Lib.Components.Actions;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Errors;
using MovieDbAssistant.Lib.Components.InstanceCounter;
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
        IConfiguration config
        )
    {
        InstanceId = new(this);
        _logger = logger;
        _signal = signal;
        _config = config;
        _worker = new(logger, _signal, this);
        _worker.Setup( 
            o => _completed = true);
        _worker.Setup( (o,ex) =>
        {
            _exception = ex;
            _completed = true;
        });
    }

    /// <summary>
    /// scrap using the query
    /// <para>call MovieDbScrapper</para>
    /// </summary>
    /// <param name="queryModel">The query model.</param>
    /// <returns>A <see cref="MoviesModel"/></returns>
    public MoviesModel? Scrap(QueryModelSearchByTitle queryModel)
    {
        _completed = false;
        _worker.RunAction(
            this
            , new ActionContext(
                _signal,
                new StackErrors())
            , (context, owner, args) =>
            {
                // call a scrap process
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
