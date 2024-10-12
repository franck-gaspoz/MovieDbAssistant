using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MovieDbAssistant.Dmn.Components.Builders.Document;
using MovieDbAssistant.Dmn.Components.Builders.Templates;
using MovieDbAssistant.Dmn.Configuration;
using MovieDbAssistant.Dmn.Models.Extensions;
using MovieDbAssistant.Dmn.Models.Scrap.Json;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Logger;

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
    readonly IOptions<DmnSettings> _dmnSettings;

    public HtmlMovieDocumentBuilder(
        IConfiguration configuration,
        ILogger<HtmlDocumentBuilder> logger,
        TemplateBuilder templateBuilder,
        IOptions<DmnSettings> dmnSettings)
    {
        _config = configuration;
        _logger = logger;
        _templateBuilder = templateBuilder;
        _dmnSettings = dmnSettings;
    }

    /// <summary>
    /// Setups the model.
    /// </summary>
    /// <param name="data">The data.</param>
    public void SetupModel(MovieModel data)
        => data.SetupModel(_dmnSettings);

    /// <summary>
    /// Build the movie.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="htmlContext">html document builder context</param>
    /// <param name="data">The data.</param>
    public void BuildMovie(
        DocumentBuilderContext context,
        HtmlDocumentBuilderContext htmlContext,
        MovieModel data)
    {
        if (string.IsNullOrWhiteSpace(data.Title))
        {
            IgnoreDocument(data);
            return;
        }
        SetupModel(data);

        _logger.LogInformation(this,
            _dmnSettings.Value.Texts.ProcMovie
            + data.Title);

        _templateBuilder.LoadTemplate(
            context,
            context
                .BuilderOptions[Template_Id]
                .ToString()!)
            .BuildPageDetail(
                htmlContext,
                data);
    }

    void IgnoreDocument(MovieModel data)
        => _logger.LogWarning(this, $"skip movie: no title (id={data.Id})");
}
