using Microsoft.Extensions.Logging;

using MovieDbAssistant.Dmn.Components.Query;
using MovieDbAssistant.Dmn.Models.Scrap.Json;
using MovieDbAssistant.Lib.Components.Logger;

namespace MovieDbAssistant.Dmn.Components.DataProviders.Json;

/// <summary>
/// json from query file data provider
/// <para>call query data provider</para>
/// </summary>
public sealed class JsonQueryFileDataProvider : JsonFileDataProvider
{
    readonly JsonQueryDataProvider _queryDataProvider;
    readonly QueryBuilder _queryBuilder;

    public JsonQueryFileDataProvider(
        ILogger<JsonQueryFileDataProvider> logger,
        JsonQueryDataProvider queryDataProvider,
        QueryBuilder queryBuilder)
        : base(logger)
    {
        _queryDataProvider = queryDataProvider;
        _queryBuilder = queryBuilder;
    }

    /// <inheritdoc/>
    public override MoviesModel? Get(object? source)
    {
        if (source == null) return null;
        var src = (string)source;

        Logger.LogInformation(
            this,
            "parse query file: "
            + Path.GetFileName(src));

        var file = File.ReadAllText(src);

        var queries = _queryBuilder.Build(file);

        //return _queryDataProvider.Get()
        return null;
    }
}
