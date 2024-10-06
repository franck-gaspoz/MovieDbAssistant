namespace MovieDbAssistant.Dmn.Configuration;

/// <summary>
/// The texts settings.
/// </summary>
public sealed class PathsSettings
{
    /// <summary>
    /// Gets or sets the path output pages.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public required string OutputPages { get; set; }


    /// <summary>
    /// Gets or sets the path temp.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public required string Temp { get; set; }
}
