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
    public string Pages { get; set; }

    /// <summary>
    /// parts
    /// </summary>
    public string Parts { get; set; }

    /// <summary>
    /// handled tpl extensions
    /// </summary>
    public List<string> HandleExtensions { get; set; } = [];

    public PathsModel(string pages, string parts)
    {
        Pages = pages;
        Parts = parts;
    }
}
