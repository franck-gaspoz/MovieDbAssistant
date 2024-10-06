using Microsoft.Extensions.Configuration;

using MovieDbAssistant.Dmn.Models.Queries;
using MovieDbAssistant.Lib.ComponentModels;
using MovieDbAssistant.Lib.Components.InstanceCounter;

namespace MovieDbAssistant.Dmn.Components.DataProviders.Json.SourceModelAdapters;

/// <summary>
/// The IMDB source model adapter.
/// </summary>
public sealed class IMDBSourceModelAdapter : IIdentifiable
{
    readonly IConfiguration _config;

    /// <inheritdoc/>
    public SharedCounter InstanceId { get; }

    public IMDBSourceModelAdapter(IConfiguration config)
    {
        InstanceId = new(this);
        _config = config;
    }

    /// <summary>
    /// get the query filter part
    /// </summary>
    /// <param name="model">query model</param>
    /// <returns>filter string</returns>
    public string GetFilter(QueryModelSearchByTitle model)
    {
        return string.Empty;
    }
}
