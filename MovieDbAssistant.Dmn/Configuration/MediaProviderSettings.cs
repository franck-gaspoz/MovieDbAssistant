namespace MovieDbAssistant.Dmn.Configuration;

/// <summary>
/// The media provider settings.
/// </summary>
public sealed class MediaProviderSettings
{
    /// <summary>
    /// media provider id
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    /// paths masks
    /// </summary>
    public required string[] Paths { get; set; }
}
