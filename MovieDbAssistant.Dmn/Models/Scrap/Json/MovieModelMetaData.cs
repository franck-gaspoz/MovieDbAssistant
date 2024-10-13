using MovieDbAssistant.Dmn.Models.Queries;

namespace MovieDbAssistant.Dmn.Models.Scrap.Json;

/// <summary>
/// The movie model meta data.
/// </summary>
public sealed partial class MovieModelMetaData
{
    /// <summary>
    /// query model
    /// </summary>
    public QueryModel? Query { get; set; }

    /// <summary>
    /// infos about the scraper: tool name
    /// </summary>
    public string? ScraperTool { get; set; }

    /// <summary>
    /// infos about the scraper: tool version
    /// </summary>
    public string? ScraperToolVersion { get; set; }

    /// <summary>
    /// infos about the scrap : spider id
    /// </summary>
    public string? SpiderId { get; set; }

    /// <summary>
    /// score related to a search
    /// </summary>
    public SearchScore? SearchScore { get; set; } = new();
}
