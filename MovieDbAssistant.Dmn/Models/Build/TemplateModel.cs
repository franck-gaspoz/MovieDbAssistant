﻿using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

using MovieDbAssistant.Dmn.Components.Builders.Document;
using MovieDbAssistant.Dmn.Models.Extensions;
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
        ThemeModel theme,
        List<PageModel> pages,
        JsonElement? props,
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
        Props = props;
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
    /// <value>A <see cref="ThemeModel"/></value>
    [JsonPropertyName("theme")]
    public ThemeModel Theme { get; set; }

    /// <summary>
    /// pages
    /// </summary>
    [JsonPropertyName("pages")]
    public List<PageModel> Pages { get; set; }

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
    /// Pages index path.
    /// </summary>
    /// <param name="docBuilderContext">doc builder context</param>
    /// <param name="extension">extension</param>
    /// <returns>A <see cref="string"/></returns>
    public string PageIndexPath(DocumentBuilderContext docBuilderContext,
        string extension)
            => docBuilderContext.TplFilePath(
                this.PageList()!.Filename!, extension);
}
