namespace MovieDbAssistant.Dmn.Models.Build;

/// <summary>
/// page model
/// </summary>
public sealed class PageModel
{
    /// <summary>
    /// page id
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// layout
    /// </summary>
    public string Layout { get; set; }

    /// <summary>
    /// template file
    /// </summary>
    public string Tpl { get; set; }

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
    /// sub title
    /// </summary>
    public string? SubTitle { get; set; }

    /// <summary>
    /// Gets or sets the filename.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string? Filename { get; set; }

    public PageModel(
        string id,
        string layout,
        string tpl,
        string? background,
        string? backgroundIdle,
        string? title,
        string? subTitle,
        string? filename)
    {
        Id = id;
        Layout = layout;
        Tpl = tpl;
        Background = background;
        BackgroundIdle = backgroundIdle;
        Title = title;
        SubTitle = subTitle;
        Filename = filename;
    }
}