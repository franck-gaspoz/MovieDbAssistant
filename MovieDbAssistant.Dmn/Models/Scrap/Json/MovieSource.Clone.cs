namespace MovieDbAssistant.Dmn.Models.Scrap.Json;

/// <summary>
/// The movie source.
/// </summary>
public sealed partial class MovieSources
{
    /// <summary>
    /// gets a clone
    /// </summary>
    /// <returns>A <see cref="MovieSource"/></returns>
    public MovieSources Clone()
        => new()
        {
            Play = Play,
            Download = Download
        };
}
