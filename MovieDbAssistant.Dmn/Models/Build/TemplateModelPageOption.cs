﻿namespace MovieDbAssistant.Dmn.Models.Build;

/// <summary>
/// The template model page option.
/// </summary>
public sealed class TemplateModelPageOption
{
    /// <summary>
    /// Gets or sets the background.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string Background { get; set; }

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the filename.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string Filename { get; set; }

    public TemplateModelPageOption(string background, string title, string filename)
    {
        Background = background;
        Title = title;
        Filename = filename;
    }
}