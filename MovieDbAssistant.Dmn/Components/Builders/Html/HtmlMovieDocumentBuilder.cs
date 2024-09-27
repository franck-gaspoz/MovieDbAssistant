using System.Data;

using Microsoft.Extensions.Logging;

using MovieDbAssistant.Dmn.Models.Scrap.Json;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Extensions;
using MovieDbAssistant.Lib.Components.Logger;

namespace MovieDbAssistant.Dmn.Components.Builders.Html;

/// <summary>
/// html document movie builder
/// </summary>
[Transient]
public sealed class HtmlMovieDocumentBuilder
{
    readonly ILogger<HtmlDocumentBuilder> _logger;

    public HtmlMovieDocumentBuilder(
        ILogger<HtmlDocumentBuilder> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Build the movie.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="data">The data.</param>
    public void BuildMovie(
        DocumentBuilderContext context,
        MovieModel data)
    {
        if (string.IsNullOrWhiteSpace(data.Title))
        {
            IgnoreDocument(data);
            return;
        }
        var key = data.Title!.ToHexString();
    }

    void IgnoreDocument(MovieModel data)
        => _logger.LogWarning(this, $"skip movie: no title (id={data.Id})");
}
