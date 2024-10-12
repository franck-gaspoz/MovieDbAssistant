using System.Text.RegularExpressions;

using MovieDbAssistant.Lib.Components.Extensions;

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

    /// <summary>
    /// quasi unique key
    /// </summary>
    public string HashKey =>
        _title.ToHexLettersAndDigitsString()
        + Count?.ToString()
        + Year?.ToString()
        + Languages.IfNullElse(() => "", x => string.Join("",
            x.Select(x => x.ToHexLettersAndDigitsString())))
        + Countries.IfNullElse(() => "", x => string.Join("",
            x.Select(x => x.ToHexLettersAndDigitsString())))
        + UserRating.ToHexLettersAndDigitsString()
        + TitleTypes.IfNullElse(() => "", x => string.Join("",
            x.Select(x => x.ToString().ToHexLettersAndDigitsString())))
        + Genres.IfNullElse(() => "", x => string.Join("",
            x.Select(x => x.ToString().ToHexLettersAndDigitsString())))
        ;

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
            var pattern = @"\b\d{4}\b";
            var matches = Regex.Matches(value, pattern);
            if (matches.Count != 0)
            {
                _year = matches[0].Value;
                if (_title.EndsWith(Year))
                    _title = _title[..^Year.Length].Trim();
            }
        }
    }

    /// <summary>
    /// count
    /// </summary>
    public int? Count { get; set; }

    string? _year;
    /// <summary>
    /// Gets or sets the year.
    /// </summary>
    /// <value>A <see cref="string? "/></value>
    public string? Year
    {
        get => _year;
        set
        {
            if (value != null)
                _year = value;
        }
    }

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
