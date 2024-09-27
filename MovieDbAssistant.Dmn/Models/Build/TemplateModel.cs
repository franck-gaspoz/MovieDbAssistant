namespace MovieDbAssistant.Dmn.Models.Build;

/// <summary>
/// template properties model
/// </summary>
public sealed class TemplateModel
{
    /// <summary>
    /// name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// template contents
    /// </summary>
    public TemplatesModel Templates { get; set; } = new("","");

    #region working properties

    /// <summary>
    /// path on disk of the template folder
    /// </summary>
    public string? Path { get; set; }

    #endregion

    /// <summary>
    /// load template contents
    /// </summary>
    /// <param name="templatePath">template folder</param>
    /// <returns>this object</returns>
    public TemplateModel LoadContent(string templatePath)
    {
        Templates.Load(templatePath);
        Path = templatePath;
        return this;
    }
}
