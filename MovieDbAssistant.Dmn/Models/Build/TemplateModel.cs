namespace MovieDbAssistant.Dmn.Models.Build;

/// <summary>
/// template properties model
/// </summary>
public sealed class TemplateModel
{
    public string Name { get; set; } = string.Empty;

    public TemplatesModel Templates { get; set; } = new("","");
}
