using System.Text.Json.Serialization;

namespace MovieDbAssistant.Dmn.Models.Build;

/// <summary>
/// page model
/// </summary>
public sealed class PageModel
{
    /// <summary>
    /// page id
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    /// layout
    /// </summary>
    [JsonPropertyName("layout")]
    public string Layout { get; set; }

    /// <summary>
    /// template file
    /// </summary>
    [JsonPropertyName("tpl")]
    public string Tpl { get; set; }

    /// <summary>
    /// Gets or sets the background (template overrided)
    /// </summary>
    /// <value>A <see cref="string"/></value>
    [JsonPropertyName("background")]
    public string? Background { get; set; }

    /// <summary>
    /// Gets or sets the background for idle phase
    /// </summary>
    /// <value>A <see cref="string"/></value>
    [JsonPropertyName("backgroundIdle")]
    public string? BackgroundIdle { get; set; }

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    [JsonPropertyName("pageTitle")]
    public string? PageTitle { get; set; }

    /// <summary>
    /// Gets or sets the filename.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    [JsonPropertyName("Filename")]
    public string? Filename { get; set; }

    public PageModel(
        string id,
        string layout,
        string tpl,
        string? background,
        string? backgroundIdle,
        string? title,
        string? pageTitle,
        string? filename)
    {
        Id = id;
        Layout = layout;
        Tpl = tpl;
        Background = background;
        BackgroundIdle = backgroundIdle;
        Title = title;
        PageTitle = pageTitle;
        Filename = filename;
    }
}