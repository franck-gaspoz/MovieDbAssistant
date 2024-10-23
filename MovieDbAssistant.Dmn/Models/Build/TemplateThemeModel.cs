namespace MovieDbAssistant.Dmn.Models.Build;

/// <summary>
/// The template theme model.
/// </summary>
public sealed class TemplateThemeModel
{
    /// <summary>
    /// Gets or sets the UI.
    /// </summary>
    /// <value>A <see cref="TemplateThemeComponentModel"/></value>
    public TemplateThemeComponentModel Ui { get; set; }

    /// <summary>
    /// Gets or sets the fonts.
    /// </summary>
    /// <value>A <see cref="TemplateThemeComponentModel"/></value>
    public TemplateThemeComponentModel Fonts { get; set; }

    /// <summary>
    /// Gets or sets the icons.
    /// </summary>
    /// <value>A <see cref="TemplateThemeComponentModel"/></value>
    public TemplateThemeComponentModel Icons { get; set; }

    /// <summary>
    /// Gets or sets the buttons.
    /// </summary>
    /// <value>A <see cref="TemplateThemeComponentModel"/></value>
    public TemplateThemeComponentModel Buttons { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TemplateThemeModel"/> class.
    /// </summary>
    /// <param name="ui">The ui</param>
    /// <param name="fonts">The fonts.</param>
    /// <param name="icons">The icons.</param>
    /// <param name="buttons">The buttons.</param>
    public TemplateThemeModel(
        TemplateThemeComponentModel ui,
        TemplateThemeComponentModel fonts,
        TemplateThemeComponentModel icons,
        TemplateThemeComponentModel buttons)
    {
        Ui = ui;
        Fonts = fonts;
        Icons = icons;
        Buttons = buttons;
    }
}
