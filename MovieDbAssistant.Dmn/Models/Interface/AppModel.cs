namespace MovieDbAssistant.Dmn.Models.Interface;

/// <summary>
/// The app model.
/// </summary>
public sealed class AppModel
{
    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the id.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the version.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string Version { get; set; }

    /// <summary>
    /// Gets or sets the version date.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string VersionDate { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AppModel"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="id">The id.</param>
    /// <param name="version">The version.</param>
    /// <param name="versionDate">The version date.</param>
    public AppModel(string name, string id, string version, string versionDate)
    {
        Name = name;
        Id = id;
        Version = version;
        VersionDate = versionDate;
    }
}
