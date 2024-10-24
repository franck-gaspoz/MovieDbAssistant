using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

using MovieDbAssistant.Lib.Extensions;

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
        List<PageModel> pages,
        TemplatesModel templates,
        JsonElement? props,
        TemplateModelOptions options,
        List<string> files,
        List<string> resources,
        string? path,
        PathsModel paths,
        List<TransformModel> transforms)
    {
        Name = name;
        Version = version;
        VersionDate = versionDate;
        Id = id;
        Theme = theme;
        Pages = pages;
        Templates = templates;
        Props = props;
        Options = options;
        Files = files;
        Resources = resources;
        Path = path;
        Paths = paths;
        Transforms = transforms;
    }

    /// <summary>
    /// name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// version number
    /// </summary>
    [JsonPropertyName("version")]
    public string Version { get; set; }

    /// <summary>
    /// version date
    /// </summary>
    [JsonPropertyName("versionDate")]
    public string VersionDate { get; set; }

    /// <summary>
    /// Gets or sets the id.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the theme.
    /// </summary>
    /// <value>A <see cref="TemplateThemeModel"/></value>
    [JsonPropertyName("theme")]
    public TemplateThemeModel Theme { get; set; }

    /// <summary>
    /// pages
    /// </summary>
    [JsonPropertyName("pages")]
    public List<PageModel> Pages { get; set; }

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
    /// dynamic props
    /// </summary>
    [JsonIgnore]
    [IgnoreDataMember]
    public dynamic DProps
        => Props.ToDynamic();

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
    [JsonPropertyName("files")]
    public List<string> Files { get; set; } = [];

    /// <summary>
    /// gets or sets the resources
    /// </summary>
    [JsonPropertyName("resources")]
    public List<string> Resources { get; set; } = [];

    #region working properties

    /// <summary>
    /// path on disk of the template folder
    /// </summary>
    [JsonPropertyName("path")]
    public string? Path { get; set; }

    /// <summary>
    /// paths
    /// </summary>
    [JsonPropertyName("paths")]
    public PathsModel Paths { get; set; }

    /// <summary>
    /// transforms
    /// </summary>
    [JsonPropertyName("transforms")]
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
