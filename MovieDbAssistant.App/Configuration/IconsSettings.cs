namespace MovieDbAssistant.App.Configuration;

/// <summary>
/// The icons settings.
/// </summary>
public sealed class IconsSettings
{

    /// <summary>
    /// app icon file.
    /// </summary>
    public required string Tray { get; set; }

    /// <summary>
    /// buzy step 1 icon file
    /// </summary>
    public required string Buzy1 { get; set; }

    /// <summary>
    /// buzy step 2 icon file
    /// </summary>
    public required string Buzy2 { get; set; }
}