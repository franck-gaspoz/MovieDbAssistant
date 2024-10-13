namespace MovieDbAssistant.Dmn.Models.Scrap.Json;

/// <summary>
/// The search score.
/// </summary>
public sealed partial class SearchScore
{
    /// <summary>
    /// gets a clone
    /// </summary>
    /// <returns>A <see cref="SearchScore"/></returns>
    public SearchScore Clone()
        => new()
        {
            Affinity = Affinity,
            DataCompletion = DataCompletion
        };
}
