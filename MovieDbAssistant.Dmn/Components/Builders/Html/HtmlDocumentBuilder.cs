using Microsoft.Extensions.Logging;

using MovieDbAssistant.Dmn.Components.Builder;
using MovieDbAssistant.Dmn.Components.Builders.Templates;
using MovieDbAssistant.Dmn.Models.Scrap.Json;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Logger;

using static MovieDbAssistant.Dmn.Globals;
using static MovieDbAssistant.Dmn.Components.Builders.Html.HtmDocumentBuilderSettings;
using Microsoft.Extensions.Configuration;

namespace MovieDbAssistant.Dmn.Components.Builders.Html;

/// <summary>
/// html document builder.
/// </summary>
[Transient]
public sealed class HtmlDocumentBuilder : IDocumentBuilder
{
    readonly IConfiguration _config;
    readonly ILogger<HtmlDocumentBuilder> _logger;
    readonly HtmlMovieDocumentBuilder _htmlMovieDocumentBuilder;
    readonly TemplateBuilder _templateBuilder;

    public HtmlDocumentBuilder(
        IConfiguration config,
        ILogger<HtmlDocumentBuilder> logger,
        HtmlMovieDocumentBuilder htmlMovieDocumentBuilder,
        TemplateBuilder templateBuilder)
    {
        _config = config;
        _logger = logger;
        _htmlMovieDocumentBuilder = htmlMovieDocumentBuilder;
        _templateBuilder = templateBuilder;
    }

    /// <summary>
    /// build the movies documents
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="data">The data.</param>
    public void Build(DocumentBuilderContext context, MoviesModel data)
    {
        _logger.LogInformation(this, $"process json: {data.Movies.Count} movies");

        data
            .Filter()
            .Distinct()
            .Sort();

        var folderName = Path.GetFileNameWithoutExtension(context.Source);
        var folder = Path.Combine(context.OutputPath, folderName);
        context.Target = folder;

        _logger.LogInformation(this,
            _config[ProcMovieList]
            + Path.GetFileName(context.Source));

        // get template & prepare output

        context.MakeOutputDirs();

        _templateBuilder.LoadTemplate(
            context,
            context
                .BuilderOptions[Template_Id]
                .ToString()!
            )

        // build list pages

            .BuildPageList(data);

        // build detail pages

        foreach (var movie in data.Movies)
            _htmlMovieDocumentBuilder.BuildMovie(context, movie);
    }
}
