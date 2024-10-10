namespace MovieDbAssistant.Dmn.Configuration;

/// <summary>
/// filters settings
/// </summary>
public sealed class FiltersSettings
{
    /// <summary>
    /// Gets or sets the countries.
    /// </summary>
    /// <value>An array of strings</value>
    public required string[] Countries { get; set; }

    /// <summary>
    /// Gets or sets the languages.
    /// </summary>
    /// <value>An array of strings</value>
    public required string[]? Languages { get; set; }

    /// <summary>
    /// Gets or sets the count.
    /// </summary>
    /// <value>An <see cref="int"/></value>
    public int Count { get; set; }
}