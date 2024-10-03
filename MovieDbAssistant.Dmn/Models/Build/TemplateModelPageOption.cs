namespace MovieDbAssistant.Dmn.Models.Build;

/// <summary>
/// The template model page option.
/// </summary>
public sealed class TemplateModelPageOption
{
    /// <summary>
    /// Gets or sets the background (template overrided)
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string? Background { get; set; }

    /// <summary>
    /// Gets or sets the background for idle phase
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string? BackgroundIdle { get; set; }

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string? PageTitle { get; set; }

    /// <summary>
    /// Gets or sets the filename.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string? Filename { get; set; }

    public TemplateModelPageOption(
        string background,
        string? backgroundIdle,
        string? title,
        string? filename)
    {
        Background = background;
        BackgroundIdle = backgroundIdle;
        Title = title;
        Filename = filename;
    }
}