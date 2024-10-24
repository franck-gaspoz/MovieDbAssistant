using System.Text.Json;
using System.Text.Json.Serialization;

namespace MovieDbAssistant.Dmn.Models.Build;

/// <summary>
/// template properties model
/// </summary>
public sealed class TemplateModel
{
    public TemplateModel(
        string name,
        string version,
        string versionDate,
        string id,
        TemplateThemeModel theme,
        TemplatesModel templates,
        TemplateModelOptions options,
        List<string> files,
        string? path)
    {
        Theme = theme;
        Name = name;
        Version = version;
        VersionDate = versionDate;
        Id = id;
        Templates = templates;
        Options = options;
        Files = files;
        Path = path;
    }

    /// <summary>
    /// name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// version number
    /// </summary>
    public string Version { get; set; }

    /// <summary>
    /// version date
    /// </summary>
    public string VersionDate { get; set; }

    /// <summary>
    /// Gets or sets the id.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the theme.
    /// </summary>
    /// <value>A <see cref="TemplateThemeModel"/></value>
    public TemplateThemeModel Theme { get; set; }

    /// <summary>
    /// template contents
    /// </summary>
    public TemplatesModel Templates { get; set; }

    /// <summary>
    /// Gets or sets the template dynamic json properties. (fits js needs)
    /// </summary>
    /// <value>A <see cref="JsonElement? "/></value>
    [JsonPropertyName("props")]
    public JsonElement? Props { get; set; }

    /// <summary>
    /// Gets or sets the options.
    /// </summary>
    /// <value>A <see cref="TemplateModelOptions"/></value>
    [JsonPropertyName("options")]
    public TemplateModelOptions Options { get; set; }

    /// <summary>
    /// Gets or sets the files.
    /// </summary>
    /// <value>A list of strings.</value>
    public List<string> Files { get; set; } = [];

    /// <summary>
    /// gets or sets the resources
    /// </summary>
    public List<string> Resources { get; set; } = [];

    #region working properties

    /// <summary>
    /// path on disk of the template folder
    /// </summary>
    public string? Path { get; set; }

    /// <summary>
    /// transforms
    /// </summary>
    public List<TransformModel> Transforms { get; set; } = [];

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
