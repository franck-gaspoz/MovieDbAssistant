namespace MovieDbAssistant.Dmn.Configuration;

/// <summary>
/// The texts settings.
/// </summary>
public sealed class TextsSettings
{
    /// <summary>
    /// Gets or sets the data provider failed.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public required string DataProviderFailed { get; set; }

    /// <summary>
    /// Gets or sets the proc movie.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public required string ProcMovie { get; set; }

    /// <summary>
    /// Gets or sets the proc movie list.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public required string ProcMovieList { get; set; }

    /// <summary>
    /// processing file
    /// </summary>
    public required string ProcFile { get; set; }
}
