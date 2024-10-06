namespace MovieDbAssistant.Dmn.Configuration;

/// <summary>
/// The app settings.
/// </summary>
public sealed class AppSettings
{
    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public required string Title { get; set; }

    /// <summary>
    /// Gets or sets the lang.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public required string Lang { get; set; }

    /// <summary>
    /// Gets or sets the version date.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public required string VersionDate { get; set; }
}
