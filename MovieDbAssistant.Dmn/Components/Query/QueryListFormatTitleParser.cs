using MovieDbAssistant.Dmn.Models.Queries;

namespace MovieDbAssistant.Dmn.Components.Query;

/// <summary>
/// The query list format title parser.
/// </summary>
public sealed class QueryListFormatTitleParser : IQueryListFormatParser
{
    /// <inheritdoc/>
    public List<QueryModel> Parse(string content)
    {
        var res = new List<QueryModel>();
        return res;
    }
}
