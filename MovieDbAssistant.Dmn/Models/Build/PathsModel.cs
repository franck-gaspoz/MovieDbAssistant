using System.Text.Json.Serialization;

namespace MovieDbAssistant.Dmn.Models.Build;

/// <summary>
/// The paths model.
/// </summary>
/// <param name="Pages"> tpl pages path </param>
/// <param name="Parts"> tpl parts path </param>
public sealed class PathsModel
{
    /// <summary>
    /// pages
    /// </summary>
    [JsonPropertyName("pages")]
    public string Pages { get; set; }

    /// <summary>
    /// parts
    /// </summary>
    [JsonPropertyName("parts")]
    public string Parts { get; set; }

    /// <summary>
    /// handled tpl extensions
    /// </summary>
    [JsonPropertyName("handledExtensions")]
    public List<string> HandledExtensions { get; set; } = new();

    public PathsModel(string pages, string parts)
    {
        Pages = pages;
        Parts = parts;
    }
}
