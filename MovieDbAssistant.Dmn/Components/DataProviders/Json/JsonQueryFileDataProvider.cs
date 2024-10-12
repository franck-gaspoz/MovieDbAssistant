using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using MovieDbAssistant.Dmn.Components.Builders.Models;
using MovieDbAssistant.Dmn.Components.Builders.Models.Extensions;
using MovieDbAssistant.Dmn.Components.Query;
using MovieDbAssistant.Dmn.Models.Scrap.Json;
using MovieDbAssistant.Dmn.Models.Scrap.Json.Extensions;
using MovieDbAssistant.Lib.Components.Logger;

namespace MovieDbAssistant.Dmn.Components.DataProviders.Json;

/// <summary>
/// json from query file data provider
/// <para>call query data provider</para>
/// </summary>
public sealed class JsonQueryFileDataProvider : JsonFileDataProvider
{
    readonly QueryBuilder _queryBuilder;
    readonly MoviesModelMergeBuilder _moviesModelMergeBuilder;
    readonly IServiceProvider _serviceProvider;

    public JsonQueryFileDataProvider(
        ILogger<JsonQueryFileDataProvider> logger,
        QueryBuilder queryBuilder,
        MoviesModelMergeBuilder moviesModelMergeBuilder,
        IServiceProvider serviceProvider)
        : base(logger)
    {
        _queryBuilder = queryBuilder;
        _moviesModelMergeBuilder = moviesModelMergeBuilder;
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc/>
    public override MoviesModel? Get(object? source)
    {
        if (source == null) return null;
        var src = (string)source;

        Logger.LogInformation(
            this,
            "parse query file: "
            + Path.GetFileName(src));

        var file = File.ReadAllText(src);

        var queries = _queryBuilder.Build(src, file);

        if (queries == null) return null;

        var movies = new List<MovieModel>();
        queries.ForEach(query =>
        {
            var provider = _serviceProvider
                .GetRequiredService<JsonQueryDataProvider>();
            var moviesModel = provider.Get(query);

            if (moviesModel != null
                && moviesModel.Movies.Count>0)                
            {
                // TODO
                //var t = _moviesModelMergeBuilder.Collapse(moviesModel);
                //movies.Add(t);

                // compute search score

                moviesModel.BuildSearchScore(
                    _serviceProvider,
                    query);

                var best = moviesModel.HavingBestSearchScore();

                //all results
                movies.AddRange(moviesModel.Movies);
            }
            else
            {
                var defaultModel = query.CreateDefaultMovieModel();
                movies.Add(defaultModel);
            }
        });

        // encapsulate type and add meta data (query,..)
        var data = new MoviesModel()
        {
            Movies = movies
        };

        return data;
    }
}
