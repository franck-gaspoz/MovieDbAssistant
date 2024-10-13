namespace MovieDbAssistant.Dmn.Models.Scrap.Json;

/// <summary>
/// The movie model meta data.
/// </summary>
public sealed partial class MovieModelMetaData
{
    /// <summary>
    /// gets a clone
    /// </summary>
    /// <returns>A <see cref="MovieModelMetaData"/></returns>
    public MovieModelMetaData Clone()
        => new()
        {
            Query = Query,
            ScraperTool = ScraperTool,
            ScraperToolVersion = ScraperToolVersion,
            SpiderId = SpiderId,
            SearchScore = SearchScore?.Clone()
        };
}
