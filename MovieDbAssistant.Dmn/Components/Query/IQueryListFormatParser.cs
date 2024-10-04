using MovieDbAssistant.Dmn.Models.Queries;

namespace MovieDbAssistant.Dmn.Components.Query;

/// <summary>
/// Query list format parser interface.
/// </summary>
public interface IQueryListFormatParser
{
    /// <summary>
    /// Parse and return a list of querymodels.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <returns>A list of querymodels.</returns>
    public List<QueryModel> Parse(string content);
}