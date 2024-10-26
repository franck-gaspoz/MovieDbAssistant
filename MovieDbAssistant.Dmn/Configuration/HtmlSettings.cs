namespace MovieDbAssistant.Dmn.Configuration;

/// <summary>
/// html settings
/// </summary>
public sealed class HtmlSettings
{
    /// <summary>
    /// Gets or sets the extension.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public required string Extension { get; set; }

    /// <summary>
    /// Gets or sets the template filename.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public required string TemplateFilename { get; set; }

    /// <summary>
    /// Gets or sets the data filename.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public required string DataFilename { get; set; }

    /// <summary>
    /// Gets or sets the template id.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public required string TemplateId { get; set; }

    /// <summary>
    /// Gets or sets the template version.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public required string TemplateVersion { get; set; }

}