using Microsoft.Extensions.Logging;

using MovieDbAssistant.Dmn.Models.Queries;
using MovieDbAssistant.Dmn.Models.Scrap.Json;
using MovieDbAssistant.Lib.Components.Logger;

namespace MovieDbAssistant.Dmn.Components.DataProviders.Json;

/// <summary>
/// json from query file data provider
/// <para>call query data provider</para>
/// </summary>
public sealed class JsonQueryDataProvider : JsonDataProvider
{
    public JsonQueryDataProvider(ILogger<JsonQueryDataProvider> logger)
        : base(logger) { }

    /// <summary>
    /// get from query model
    /// </summary>
    /// <param name="source">query model</param>
    /// <returns>movies model or null</returns>
    public override MoviesModel? Get(object? source)
    {
        if (source == null) return null;
        if (source is not QueryModelSearchByTitle query) return null;  

        Logger.LogInformation(
            this,
            "handle search query: "
            + query);

        

        return null;
    }
}
