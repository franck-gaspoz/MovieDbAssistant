using System.Text.Json;

using Microsoft.Extensions.Logging;

using MovieDbAssistant.Dmn.Models.Scrap.Json;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;

using static MovieDbAssistant.Dmn.Globals;

namespace MovieDbAssistant.Dmn.Components.DataProviders.Json;

/// <summary>
/// json data provider == MovieDbScraper output
/// </summary>
[Scoped]
public class JsonDataProvider : IDataProvider
{
    protected ILogger<JsonDataProvider> Logger { get; set; }

    public JsonDataProvider(ILogger<JsonDataProvider> logger)
        => Logger = logger;

    /// <summary>
    /// get from text
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns>A <see cref="MoviesModel"/></returns>
    public virtual MoviesModel Get(string source)
    {
        var r = JsonSerializer.Deserialize<MoviesModel>(
            source,
            JsonSerializerProperties.Value);

        return r!;
    }
}
