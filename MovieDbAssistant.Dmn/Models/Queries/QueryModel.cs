using System.Text.RegularExpressions;

namespace MovieDbAssistant.Dmn.Models.Queries;

/// <summary>
/// query model (merge all specific models)
/// </summary>
public sealed record class QueryModel
{
    /// <summary>
    /// spiders. default: SpidersIds.imdb
    /// </summary>
    public SpidersIds[] Spiders { get; set; }

    /// <summary>
    /// Gets or sets the metadata.
    /// </summary>
    /// <value>A <see cref="QueryMetadata? "/></value>
    public QueryMetadata? Metadata { get; set; }

    string? _title;
    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string Title
    {
        get => _title!;
        set
        {
            _title = value;
            string pattern = @"\b\d{4}\b";
            var matches = Regex.Matches(value, pattern);
            if (matches.Count != 0)
            {
                Year = matches[0].Value;
                if (_title.EndsWith(Year))
                    _title = _title[..^Year.Length];
            }
        }
    }

    /// <summary>
    /// count
    /// </summary>
    public int? Count { get; set; }

    /// <summary>
    /// Gets or sets the year.
    /// </summary>
    /// <value>A <see cref="string? "/></value>
    public string? Year { get; set; }

    /// <summary>
    /// Gets or sets the languages
    /// </summary>
    /// <value>A <see cref="string? "/></value>
    public string[]? Languages { get; set; }

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
    {
        Title = title;
        Languages = languages;
        Year = year;
        Countries = countries;
        Metadata = new();
        UserRating = userRating;
        Spiders = [SpidersIds.imdb];
    }
}
