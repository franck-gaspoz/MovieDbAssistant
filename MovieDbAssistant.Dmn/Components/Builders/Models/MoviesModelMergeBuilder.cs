using Microsoft.Extensions.Logging;

using MovieDbAssistant.Dmn.Models.Scrap.Json;
using MovieDbAssistant.Lib.ComponentModels;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.InstanceCounter;

namespace MovieDbAssistant.Dmn.Components.Builders.Models;

/// <summary>
/// The movies model merge builder.
/// </summary>
[Transient]
public sealed class MoviesModelMergeBuilder : IIdentifiable
{
    readonly ILogger<MoviesModelMergeBuilder> _logger;

    public SharedCounter InstanceId { get; }

    public MoviesModelMergeBuilder(ILogger<MoviesModelMergeBuilder> logger)
    {
        _logger = logger;
        InstanceId = new(this);
    }

    /// <summary>
    /// collapse several results for a title into a single best filled movie model
    /// </summary>
    /// <param name="moviesModel">models for a movie</param>
    /// <returns>movie model or null</returns>
    public MovieModel? Collapse(MoviesModel moviesModel)
    {
        if (moviesModel.Movies.Count == 0)
            return null;

        return moviesModel.Movies.First();
    }
}
