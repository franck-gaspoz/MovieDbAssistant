namespace MovieDbAssistant.Dmn.Models.Queries;

/// <summary>
/// The query metadata.
/// </summary>
/// <param name="Source"> Gets or sets the source. </param>
/// <param name="Download"> Gets or sets the download. </param>
public sealed record QueryMetadata(
    string? Source, 
    string? Download);
