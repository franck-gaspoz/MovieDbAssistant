using Microsoft.Extensions.Configuration;

using MovieDbAssistant.Dmn.Models.Queries;
using MovieDbAssistant.Lib.ComponentModels;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.InstanceCounter;

namespace MovieDbAssistant.Dmn.Components.DataProviders.Json.SourceModelAdapters;

/// <summary>
/// The IMDB source model adapter.
/// </summary>
[Transient]
public sealed class ImdbSourceModelAdapter :
    IIdentifiable,
    ISourceModelAdapter
{
    readonly IConfiguration _config;

    /// <inheritdoc/>
    public SharedCounter InstanceId { get; }

    public ImdbSourceModelAdapter(IConfiguration config)
    {
        InstanceId = new(this);
        _config = config;
    }

    /// <inheritdoc/>
    public string CreateFilter(QueryModelSearchByTitle model) => string.Empty;
}
