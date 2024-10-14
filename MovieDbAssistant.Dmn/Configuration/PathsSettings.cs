namespace MovieDbAssistant.Dmn.Configuration;

/// <summary>
/// The texts settings.
/// </summary>
public sealed class PathsSettings
{
    /// <summary>
    /// Gets or sets the path output pages.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public required string OutputPages { get; set; }

    /// <summary>
    /// Gets or sets the path temp.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public required string Temp { get; set; }

    /// <summary>
    /// Gets or sets the rsc.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public required string Resources { get; set; }

    /// <summary>
    /// Gets or sets the rsc html.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public required string RscHtml { get; set; }

    /// <summary>
    /// Gets or sets the rsc html templates.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public required string RscHtmlTemplates { get; set; }

    /// <summary>
    /// Gets or sets the rsc html assets.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public required string RscHtmlAssets { get; set; }

    /// <summary>
    /// Gets or sets the rsc html assets tpl
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public required string RscHtmlAssetsTpl { get; set; }

    /// <summary>
    /// Gets or sets the rsc html assets fonts.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public required string RscHtmlAssetsFonts { get; set; }

    /// <summary>
    /// Gets or sets the input.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public required string Input { get; set; }

    /// <summary>
    /// Gets or sets the output.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public required string Output { get; set; }

    /// <summary>
    /// The path assets.
    /// </summary>
    public required string Assets { get; set; }
}
