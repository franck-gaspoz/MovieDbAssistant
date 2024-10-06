namespace MovieDbAssistant.App.Configuration;

/// <summary>
/// The tools settings.
/// </summary>
public sealed class ToolsSettings
{
    /// <summary>
    /// Gets or sets the shell.
    /// </summary>
    /// <value>A <see cref="ToolSettings"/></value>
    public required ToolSettings Shell { get; set; }

    /// <summary>
    /// Gets or sets the folder explorer.
    /// </summary>
    /// <value>A <see cref="ToolSettings"/></value>
    public required ToolSettings FolderExplorer { get; set; }

    /// <summary>
    /// Gets or sets the open browser.
    /// </summary>
    /// <value>A <see cref="ToolSettings"/></value>
    public required ToolSettings OpenBrowser { get; set; }
}
