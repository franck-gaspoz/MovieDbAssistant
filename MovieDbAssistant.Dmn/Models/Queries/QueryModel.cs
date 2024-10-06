namespace MovieDbAssistant.Dmn.Models.Queries;

/// <summary>
/// query model (merge all specific models)
/// </summary>
public sealed record class QueryModel : QueryModelSearchByTitle
{
    /// <summary>
    /// spiders. default: SpidersIds.imdb
    /// </summary>
    public SpidersIds[] Spiders { get; set; }

    /// <summary>
    /// movie countries
    /// </summary>
    public string[]? Countries { get; set; }

    /// <summary>
    /// user rating
    /// </summary>
    public string? UserRating { get; set; }

    /// <summary>
    /// titles types
    /// </summary>
    public TitleTypes[]? TitleTypes { get; set; }

    /// <summary>
    /// genres
    /// </summary>
    public Genres[]? Genres { get; set; }

    public QueryModel(
        string title,
        string[]? languages = null,
        string? year = null,
        string[]? countries = null,
        string? userRating = null
        )
        : base(
            title,
            languages,
            year) 
    {
        Countries = countries;
        UserRating = userRating;
        Spiders = [SpidersIds.imdb];
    }
}
