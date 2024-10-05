using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using MovieDbAssistant.Dmn.Models.Queries;
using MovieDbAssistant.Dmn.Models.Scrap.Json;
using MovieDbAssistant.Lib.Components.Logger;
using MovieDbAssistant.Lib.Components.Sys;

using static MovieDbAssistant.Dmn.Globals;

namespace MovieDbAssistant.Dmn.Components.DataProviders.Json;

/// <summary>
/// json from query file data provider
/// <para>call query data provider</para>
/// </summary>
public sealed class JsonQueryDataProvider : JsonDataProvider
{
    readonly IConfiguration _config;
    readonly ProcessWrapper _processWrapper;

    public JsonQueryDataProvider(
        ILogger<JsonQueryDataProvider> logger,
        IConfiguration config,
        ProcessWrapper processWrapper)
        : base(logger)
    {
        _config = config;
        _processWrapper = processWrapper;
    }

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

        var toolPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            _config[Scrap_Tool_Path]!);

        return null;
    }
}
