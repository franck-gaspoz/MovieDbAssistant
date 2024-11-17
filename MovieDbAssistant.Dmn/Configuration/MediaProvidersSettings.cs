namespace MovieDbAssistant.Dmn.Configuration;

/// <summary>
/// The media providers settings.
/// </summary>
public sealed class MediaProvidersSettings
{
    /// <summary>
    /// Gets or sets the urls.
    /// </summary>
    /// <value>A list of media provider settings.</value>
    public required MediaProviderSettingsList Urls { get; set; } = new();

    /// <summary>
    /// physicial media providers types
    /// </summary>
    /// <value>A list of media provider settings.</value>
    public required MediaProviderSettingsList PhysycalTypes { get; set; } = new();
}