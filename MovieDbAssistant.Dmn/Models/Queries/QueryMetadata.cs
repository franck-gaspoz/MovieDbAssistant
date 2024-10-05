namespace MovieDbAssistant.Dmn.Models.Queries;

/// <summary>
/// The query metadata.
/// </summary>
/// <param name="Source"> Gets or sets the source. </param>
/// <param name="Download"> Gets or sets the download. </param>
/// <param name="QueryFile">source query file</param>
public sealed class QueryMetadata
{
    /// <summary>
    /// Gets or sets the source.
    /// </summary>
    /// <value>A <see cref="string? "/></value>
    public string? Source { get; set;}

    /// <summary>
    /// Gets or sets the download.
    /// </summary>
    /// <value>A <see cref="string? "/></value>
    public string? Download { get; set; }

    /// <summary>
    /// Gets or sets the query file.
    /// </summary>
    /// <value>A <see cref="string? "/></value>
    public string? QueryFile { get; set; }

    /// <summary>
    /// Gets or sets the query file line.
    /// </summary>
    /// <value>An <see cref="int? "/></value>
    public int? QueryFileLine {  get; set; }
}
