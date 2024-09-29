namespace MovieDbAssistant.Dmn.Models.Build;

/// <summary>
/// template properties model
/// </summary>
public sealed class TemplateModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TemplateModel"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="id">The id.</param>
    /// <param name="templates">The templates.</param>
    /// <param name="options">The options.</param>
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

    /// <summary>
    /// Gets or sets the id.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string Id { get; set; }

    /// <summary>
    /// template contents
    /// </summary>
    public TemplatesModel Templates { get; set; }

    /// <summary>
    /// Gets or sets the options.
    /// </summary>
    /// <value>A <see cref="TemplateModelOptions"/></value>
    public TemplateModelOptions Options { get; set; }

    /// <summary>
    /// Gets or sets the files.
    /// </summary>
    /// <value>A list of strings.</value>
    public List<string> Files { get; set; } = [];

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
