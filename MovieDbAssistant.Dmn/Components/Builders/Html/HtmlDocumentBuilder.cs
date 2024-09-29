using Microsoft.Extensions.Logging;

using MovieDbAssistant.Dmn.Components.Builder;
using MovieDbAssistant.Dmn.Models.Scrap.Json;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Logger;

namespace MovieDbAssistant.Dmn.Components.Builders.Html;

/// <summary>
/// html document builder.
/// </summary>
[Transient]
public sealed class HtmlDocumentBuilder : IDocumentBuilder
{
    readonly ILogger<HtmlDocumentBuilder> _logger;
    readonly HtmlMovieDocumentBuilder _htmlMovieDocumentBuilder;

    public HtmlDocumentBuilder(
        ILogger<HtmlDocumentBuilder> logger,
        HtmlMovieDocumentBuilder htmlMovieDocumentBuilder)
    {
        _logger = logger;
        _htmlMovieDocumentBuilder = htmlMovieDocumentBuilder;
    }

    /// <summary>
    /// build the movies documents
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="data">The data.</param>
    public void Build(DocumentBuilderContext context, MoviesModel data)
    {
        _logger.LogInformation(this, $"process json: {data.Movies.Count} movies");

        var ids = data.Movies.Select(x => x.Id).Distinct();
        var movies = data.Movies.Where(x => ids.Contains(x.Id));

        var folderName = Path.GetFileNameWithoutExtension(context.Source);
        var folder = Path.Combine(context.OutputPath, folderName);
        context.Target = folder;

        context.MakeOutputDir();

        foreach (var movie in movies)
            _htmlMovieDocumentBuilder.BuildMovie(context, movie);
    }
}
