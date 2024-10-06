namespace MovieDbAssistant.App.Configuration;

/// <summary>
/// The anims settings.
/// </summary>
public sealed class AnimsSettings
{
    /// <summary>
    /// wait icons
    /// </summary>
    public required string[] WaitIcons { get; set; }

    /// <summary>
    /// anim intervals settings
    /// </summary>
    public required AnimIntervalSettings Interval { get; set; }
}
