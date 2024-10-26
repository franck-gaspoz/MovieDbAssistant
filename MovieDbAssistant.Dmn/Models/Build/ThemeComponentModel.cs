using System.Text.Json.Serialization;

namespace MovieDbAssistant.Dmn.Models.Build;

/// <summary>
/// The template theme model.
/// </summary>
public sealed class ThemeComponentModel
{
    /// <summary>
    /// Gets or sets the id.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the ver.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    [JsonPropertyName("ver")] 
    public string Ver { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ThemeComponentModel"/> class.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <param name="ver">The ver.</param>
    public ThemeComponentModel(string id, string ver)
    {
        Id = id;
        Ver = ver;
    }
}
