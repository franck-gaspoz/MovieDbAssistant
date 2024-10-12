namespace MovieDbAssistant.Dmn.Models.Scrap.Json;

/// <summary>
/// The search score.
/// </summary>
public partial class SearchScore
{
    /// <summary>
    /// affinity to the query model. greater is better. 0...1
    /// </summary>
    public double? Affinity { get; set; }

    /// <summary>
    /// score of filled data. greater is better. 0...1
    /// </summary>
    public double? DataCompletion { get; set; }

    /// <summary>
    /// value: 0...2
    /// </summary>
    public double? Value =>
        (Affinity ?? 0)
        + (DataCompletion ?? 0);
}
