namespace MovieDbAssistant.Dmn.Configuration;

/// <summary>
/// The prefers settings.
/// </summary>
public sealed class PrefersSettings
{
    /// <summary>
    /// if true, replace scraped titles with queried titles. track the original title in MovieModel.OriginalTitle
    /// </summary>
    public bool QueryTitle { get; set; }

    /// <summary>
    /// it true, replaces scraped year with queried year if available. track the original title in MovieModel.OriginalYear
    /// </summary>
    public bool QueryYear { get; set; }

    /// <summary>
    /// prefers year from query if available when year from title is null
    /// </summary>
    public bool QueryYearIfDataTitleIsNull { get; set; }
}
