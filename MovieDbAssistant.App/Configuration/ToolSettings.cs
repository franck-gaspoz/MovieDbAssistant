namespace MovieDbAssistant.App.Configuration;

/// <summary>
/// The tool settings.
/// </summary>
public sealed class ToolSettings
{
    /// <summary>
    /// Gets or sets the command line.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public required string CommandLine { get; set; }

    /// <summary>
    /// Gets or sets the args.
    /// </summary>
    /// <value>A <see cref="string? "/></value>
    public required string? Args { get; set; }
}
