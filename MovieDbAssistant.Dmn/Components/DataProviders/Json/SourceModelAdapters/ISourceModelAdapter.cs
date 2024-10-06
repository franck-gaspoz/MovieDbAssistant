using MovieDbAssistant.Dmn.Models.Queries;
using MovieDbAssistant.Lib.Components.InstanceCounter;

namespace MovieDbAssistant.Dmn.Components.DataProviders.Json.SourceModelAdapters;

/// <summary>
/// source model adapter interface
/// </summary>
public interface ISourceModelAdapter
{
    /// <summary>
    /// instance id
    /// </summary>
    SharedCounter InstanceId { get; }

    /// <summary>
    /// get the query filter part
    /// </summary>
    /// <param name="model">query model</param>
    /// <returns>filter string</returns>
    string CreateFilter(QueryModelSearchByTitle model);
}