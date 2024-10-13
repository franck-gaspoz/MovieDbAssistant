namespace MovieDbAssistant.App.Configuration;

/// <summary>
/// The balloon settings.
/// </summary>
public sealed class BalloonTipsSettings
{
    /// <summary>
    /// The balloon tip start.
    /// </summary>
    public required string Start { get; set; }

    /// <summary>
    /// The balloon tip end.
    /// </summary>
    public required string End { get; set; }

    /// <summary>
    /// The balloon tip delay.
    /// </summary>
    public required int Delay { get; set; }

}
