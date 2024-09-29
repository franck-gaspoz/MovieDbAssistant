namespace MovieDbAssistant.Dmn.Models.Build;

/// <summary>
/// template properties model
/// </summary>
public sealed class TemplateModel
{
    public TemplateModel(
        string name,
        string id,
        TemplatesModel templates,
        TemplateModelOptions options)
    {
        Name = name;
        Id = id;
        Templates = templates;
        Options = options;
    }

    /// <summary>
    /// name
    /// </summary>
    public string Name { get; set; }

    public string Id { get; set; }

    /// <summary>
    /// template contents
    /// </summary>
    public TemplatesModel Templates { get; set; }

    public TemplateModelOptions Options { get; set; }

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
