using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using MovieDbAssistant.Dmn.Models.Scrap.Json;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Extensions;
using MovieDbAssistant.Lib.Components.Logger;
using MovieDbAssistant.Dmn.Components.Builders.Templates;

using static MovieDbAssistant.Dmn.Globals;
using static MovieDbAssistant.Dmn.Components.Builders.Html.HtmDocumentBuilderSettings;

namespace MovieDbAssistant.Dmn.Components.Builders.Html;

/// <summary>
/// html document movie builder
/// </summary>
[Transient]
public sealed class HtmlMovieDocumentBuilder
{
    readonly IConfiguration _config;
    readonly ILogger<HtmlDocumentBuilder> _logger;
    readonly TemplateBuilder _templateBuilder;

    public HtmlMovieDocumentBuilder(
        IConfiguration configuration,
        ILogger<HtmlDocumentBuilder> logger,
        TemplateBuilder templateBuilder)
    {
        _config = configuration;
        _logger = logger;
        _templateBuilder = templateBuilder;
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
        data.Key = key;

        _logger.LogInformation(this,
            _config[ProcMovie]
            + data.Title);

        _templateBuilder.LoadTemplate(
            context,
            context
                .BuilderOptions[Template_Id]
                .ToString()!)
            .Build(data);
    }

    void IgnoreDocument(MovieModel data)
        => _logger.LogWarning(this, $"skip movie: no title (id={data.Id})");
}
