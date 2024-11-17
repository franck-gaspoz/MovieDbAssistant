namespace MovieDbAssistant.Dmn.Configuration;

/// <summary>
/// The media providers settings.
/// </summary>
public sealed class MediaProvidersSettings
{
    /// <summary>
    /// urls.
    /// </summary>
    /// <value>A list of media provider settings.</value>
    public required List<MediaProviderSettings> Urls { get; set; } = new();

    /// <summary>
    /// physicial media providers types
    /// </summary>
    /// <value>A list of media provider settings.</value>
    public required List<MediaProviderSettings> PhysycalTypes { get; set; } = new();
}