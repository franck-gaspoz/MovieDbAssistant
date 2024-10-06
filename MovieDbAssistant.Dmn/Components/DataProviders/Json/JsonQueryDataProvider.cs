using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MovieDbAssistant.Dmn.Components.DataProviders.Json.SourceModelAdapters;
using MovieDbAssistant.Dmn.Components.Scrapper;
using MovieDbAssistant.Dmn.Configuration;
using MovieDbAssistant.Dmn.Models.Queries;
using MovieDbAssistant.Dmn.Models.Scrap.Json;
using MovieDbAssistant.Lib.Components.Logger;
using MovieDbAssistant.Lib.Components.Sys;

namespace MovieDbAssistant.Dmn.Components.DataProviders.Json;

/// <summary>
/// json from query file data provider
/// <para>call query data provider</para>
/// </summary>
public sealed class JsonQueryDataProvider : JsonDataProvider
{
    readonly IConfiguration _config;
    readonly SourceModelAdapterFactory _sourceModelAdapterFactory;
    readonly IServiceProvider _serviceProvider;
    readonly IOptions<DmnSettings> _settings;

    public JsonQueryDataProvider(
        ILogger<JsonQueryDataProvider> logger,
        IConfiguration config,
        IOptions<DmnSettings> settings,        
        SourceModelAdapterFactory sourceModelAdapterFactory,
        IServiceProvider serviceProvider)
        : base(logger)
    {
        _config = config;
        _settings = settings;
        _sourceModelAdapterFactory = sourceModelAdapterFactory;
        _serviceProvider = serviceProvider;
    }
    
    /// <summary>
    /// get from query model
    /// </summary>
    /// <param name="source">query model</param>
    /// <returns>movies model or null</returns>
    public override MoviesModel? Get(object? source)
    {
        if (source == null) return null;
        if (source is not QueryModel query) return null;

        var qid = query.Metadata!.InstanceId.Value + "";
        var outputFile = qid + ".json";
        var output = Path.Combine(
            Directory.GetCurrentDirectory(),
            _settings.Value.Paths.Temp,
            outputFile
            );

        if (_settings.Value.Scrap.SkipIfTempOutputFileAlreadyExists
            && File.Exists(output))
        {
            Logger.LogWarning(
                this,
                $"skip search query (file '{output}' already exists): #{qid}: {query}");
            return null;
        }

        Logger.LogInformation(
            this,
            $"handle search query #{qid}: {query}");

        query.Spiders
            .ToList()
            .ForEach(spiderId =>
            {
                var filters = _sourceModelAdapterFactory
                    .Create(spiderId)
                    .CreateFilter(query);

                var scraper = _serviceProvider
                    .GetRequiredService<MovieDbScrapper>();
                scraper.Scrap(
                    spiderId,
                    output,
                    filters,
                    query);
            });

        return null;
    }
}
