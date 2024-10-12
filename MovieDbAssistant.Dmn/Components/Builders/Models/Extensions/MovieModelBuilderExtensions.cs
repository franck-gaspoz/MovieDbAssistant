using System.Reflection;
using System.Runtime;

using MovieDbAssistant.Dmn.Configuration;
using MovieDbAssistant.Dmn.Models.Queries;
using MovieDbAssistant.Dmn.Models.Scrap.Json;

namespace MovieDbAssistant.Dmn.Components.Builders.Models.Extensions;

/// <summary>
/// The movies model create extensions.
/// </summary>
public static partial class MovieModelBuilderExtensions
{
    /// <summary>
    /// Creates default movie movie model.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <returns>A <see cref="MovieModel"/></returns>
    public static MovieModel CreateDefaultMovieModel(
        this QueryModel query)
    {
        var data = new MovieModel
        {
            Title = query.Title,
            Year = query.Year
        };
        return data;
    }

    /// <summary>
    /// Setup a movie model after a query.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="model">The model.</param>
    /// <param name="settings">The settings.</param>
    /// <param name="spiderId">The spider id.</param>
    /// <returns>A <see cref="MovieModel"/></returns>
    public static MovieModel SetupPostQuery(
        this QueryModel query,
        MovieModel model,
        DmnSettings settings,
        SpidersIds spiderId
        )
    {
        model.MetaData.ScraperTool = Path.GetFileName(
        settings.Scrap.ToolPath);
        model.MetaData.ScraperToolVersion = settings
            .App.MovieDbScraperToolVersion;
        model.MetaData.SpiderId = spiderId.ToString();
        model.Sources.Play = query.Metadata?.Source;
        model.Sources.Download = query.Metadata?.Download;
        model.MetaData.Query = query;
        return model;
    }

    /// <summary>
    /// Setup movies models after a query.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="models">The models.</param>
    /// <param name="settings">The settings.</param>
    /// <param name="spiderId">The spider id.</param>
    /// <returns>A <see cref="MovieModel"/></returns>
    public static MoviesModel SetupPostQuery(
        this QueryModel query,
        MoviesModel models,
        DmnSettings settings,
        SpidersIds spiderId
        )
    {
        foreach (var model in models.Movies)
            query.SetupPostQuery(
                model,
                settings,
                spiderId);
        return models;
    }
}
