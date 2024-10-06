using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MovieDbAssistant.Dmn.Configuration;
using MovieDbAssistant.Dmn.Models.Queries;
using MovieDbAssistant.Dmn.Models.Scrap.Json;
using MovieDbAssistant.Lib.Components.Extensions;
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
    readonly DmnSettings _settings;

    public JsonQueryDataProvider(
        ILogger<JsonQueryDataProvider> logger,
        IConfiguration config,
        IOptions<DmnSettings> settings,
        ProcessWrapper processWrapper)
        : base(logger)
    {
        _config = config;
        _settings = settings.Value;       
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

        var qid = query.Metadata!.InstanceId + "";
        var outputFile = qid + ".json";
        var output = Path.Combine(
            Directory.GetCurrentDirectory(),
            _settings.Paths.Temp
            );

        if ( _settings.Scrap.SkipIfTempOutputFileAlreadyExists
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

        var toolPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            _settings.Scrap.ToolPath);

        const string Q = "\"";

        /// outputFile title [filters]=
        var args = new List<string>
        {
            Q + output + Q,
            Q + query.Title + Q,
            ""
        };
        //if (query.)

        return null;
    }
}
