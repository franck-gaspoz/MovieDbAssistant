using System.Text.Json.Serialization;

namespace MovieDbAssistant.Dmn.Models.Build;

/// <summary>
/// The template theme model.
/// </summary>
public sealed class ThemeModel
{
    /// <summary>
    /// Gets or sets the kernel.
    /// </summary>
    /// <value>A <see cref="ThemeComponentModel"/></value>
    [JsonPropertyName("kernel")]
    public ThemeComponentModel Kernel { get; set; }

    /// <summary>
    /// Gets or sets the UI.
    /// </summary>
    /// <value>A <see cref="ThemeComponentModel"/></value>
    [JsonPropertyName("ui")]
    public ThemeComponentModel Ui { get; set; }

    /// <summary>
    /// Gets or sets the fonts.
    /// </summary>
    /// <value>A <see cref="ThemeComponentModel"/></value>
    [JsonPropertyName("fonts")] 
    public ThemeComponentModel Fonts { get; set; }

    /// <summary>
    /// Gets or sets the icons.
    /// </summary>
    /// <value>A <see cref="ThemeComponentModel"/></value>
    [JsonPropertyName("icons")] 
    public ThemeComponentModel Icons { get; set; }

    /// <summary>
    /// Gets or sets the buttons.
    /// </summary>
    /// <value>A <see cref="ThemeComponentModel"/></value>
    [JsonPropertyName("buttons")] 
    public ThemeComponentModel Buttons { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ThemeModel"/> class.
    /// </summary>
    /// <param name="ui">The ui</param>
    /// <param name="fonts">The fonts.</param>
    /// <param name="icons">The icons.</param>
    /// <param name="buttons">The buttons.</param>
    public ThemeModel(
        ThemeComponentModel ui,
        ThemeComponentModel fonts,
        ThemeComponentModel icons,
        ThemeComponentModel buttons)
    {
        Ui = ui;
        Fonts = fonts;
        Icons = icons;
        Buttons = buttons;
    }
}
