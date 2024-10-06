namespace MovieDbAssistant.App.Configuration;

/// <summary>
/// The anim interval settings.
/// </summary>
public sealed class AnimIntervalSettings
{
    /// <summary>
    /// dot anim interval
    /// </summary>
    public required int Dot { get; set; }

    /// <summary>
    /// icon tray anim interval
    /// </summary>
    public required int WaitTrayIcon { get; set; }
}
