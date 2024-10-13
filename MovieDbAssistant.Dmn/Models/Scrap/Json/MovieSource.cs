namespace MovieDbAssistant.Dmn.Models.Scrap.Json;

/// <summary>
/// sources of a movie
/// </summary>
public sealed partial class MovieSources
{
    /// <summary>
    /// file url
    /// </summary>
    public string? Download { get; set; }

    /// <summary>
    /// direct play url
    /// </summary>
    public string? Play { get; set; }
}
