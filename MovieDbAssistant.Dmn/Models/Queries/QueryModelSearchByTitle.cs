using System.Text.RegularExpressions;

namespace MovieDbAssistant.Dmn.Models.Queries;

/// <summary>
/// The query model.
/// </summary>
public record class QueryModelSearchByTitle
{
    /// <summary>
    /// Gets or sets the year.
    /// </summary>
    /// <value>A <see cref="string? "/></value>
    public string? Year { get;set; }

    /// <summary>
    /// Gets or sets the languages
    /// </summary>
    /// <value>A <see cref="string? "/></value>
    public string[]? Languages { get;set; }

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
                    _title =_title[..^Year.Length];
            }
        }
    }

    /// <summary>
    /// count
    /// </summary>
    public int? Count { get; set; }

    /// <summary>
    /// Gets or sets the metadata.
    /// </summary>
    /// <value>A <see cref="QueryMetadata? "/></value>
    public QueryMetadata? Metadata { get; set; }

    public QueryModelSearchByTitle(
        string title,
        string[]? languages = null,
        string? year = null)
    {
        Languages = languages;
        Year = year;
        Title = title;
        Metadata = new();
    }
}