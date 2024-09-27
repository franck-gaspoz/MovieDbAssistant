using System.Text.Json;

using Microsoft.Extensions.Logging;

using MovieDbAssistant.Dmn.Models.Scrap.Json;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;

namespace MovieDbAssistant.Dmn.Components.DataProviders;

/// <summary>
/// json data provider == MovieDbScraper output
/// </summary>
[Scoped]
public class JsonDataProvider : IDataProvider
{
    static readonly Lazy<JsonSerializerOptions> _jsonSerializerOptions
        = new(() =>
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });

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
            _jsonSerializerOptions.Value);

        return r!;
    }
}
