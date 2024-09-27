using Microsoft.Extensions.Logging;

using MovieDbAssistant.Dmn.Components.Builder;
using MovieDbAssistant.Dmn.Models.Scrap.Json;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Logger;

namespace MovieDbAssistant.Dmn.Components.Builders;

/// <summary>
/// html document builder.
/// </summary>
[Scoped]
public sealed class HtmlDocumentBuilder : IDocumentBuilder
{
    readonly ILogger<HtmlDocumentBuilder> _logger;

    public HtmlDocumentBuilder(ILogger<HtmlDocumentBuilder> logger)
        => _logger = logger;

    public void Build(DocumentBuilderContext context, MoviesModel data) 
        => _logger.LogInformation(this, $"process json: {data.Movies.Count} movies");
}
