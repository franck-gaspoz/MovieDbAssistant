namespace MovieDbAssistant.Dmn.Models.Scrap.Json;

/// <summary>
/// The query data model.
/// </summary>
public sealed class QueryDataModel
{
    /// <summary>
    /// queried title by opposite to scraped title in case of built for a query (filter Title)
    /// </summary>
    public string? Title { get; set; } = string.Empty;

    /// <summary>
    /// year from query
    /// </summary>
    public string? Year { get; set; } = string.Empty;

    /// <summary>
    /// clone the model
    /// </summary>
    /// <returns>A <see cref="QueryDataModel"/></returns>
    public QueryDataModel Clone()
        => new() { Title=Title, Year=Year };
}
