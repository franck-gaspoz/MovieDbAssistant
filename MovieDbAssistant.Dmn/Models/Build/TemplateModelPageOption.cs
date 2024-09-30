using MovieDbAssistant.Dmn.Components.Builders;

namespace MovieDbAssistant.Dmn.Models.Build;

/// <summary>
/// The template model page option.
/// </summary>
public sealed class TemplateModelPageOption
{
    /// <summary>
    /// Gets or sets the fallback background.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string FallbackBackground { get; set; }

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the filename.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string? Filename { get; set; }

    public TemplateModelPageOption(string fallbackBackground, string? title, string? filename)
    {
        FallbackBackground = fallbackBackground;
        Title = title;
        Filename = filename;
    }
}