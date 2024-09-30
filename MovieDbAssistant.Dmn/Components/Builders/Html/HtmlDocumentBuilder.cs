using System.Runtime.InteropServices;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using MovieDbAssistant.Dmn.Components.Builder;
using MovieDbAssistant.Dmn.Components.Builders.Templates;
using MovieDbAssistant.Dmn.Models.Scrap.Json;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Logger;

using static MovieDbAssistant.Dmn.Components.Builders.Html.HtmDocumentBuilderSettings;
using static MovieDbAssistant.Dmn.Globals;

namespace MovieDbAssistant.Dmn.Components.Builders.Html;

/// <summary>
/// html document builder.
/// </summary>
[Transient]
public sealed class HtmlDocumentBuilder : IDocumentBuilder
{
    const string Folder_Back = "../";
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

        var index = 0;
        var builders = new List<HtmlDocumentBuilderContext>();
        foreach (var movie in data.Movies)
            _htmlMovieDocumentBuilder.SetupModel(movie);

        foreach (var movie in data.Movies)
        {
            var builder = new HtmlDocumentBuilderContext(
                index,
                data.Movies.Count,
                Folder_Back + _templateBuilder.TemplateModel
                    .Options
                    .PageIndexPath(
                        context,
                        _config[Build_HtmlFileExt]!),
                index > 0 ?
                    Folder_Back + context.PageFilePath(
                        data.Movies[index - 1].Key!,
                        _config[Build_HtmlFileExt]!
                        )
                    : null,
                index < data.Movies.Count - 1 ?
                    Folder_Back + context.PageFilePath(
                        data.Movies[index + 1].Key!,
                        _config[Build_HtmlFileExt]!)
                    : null
                );
            builders.Add(builder);
            index++;
        }

        index = 0;
        foreach (var movie in data.Movies)
        {
            _htmlMovieDocumentBuilder.BuildMovie(
                context,
                builders[index],
                movie);
            index++;
        }
    }
}
