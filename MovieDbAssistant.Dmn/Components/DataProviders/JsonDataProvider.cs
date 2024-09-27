using MovieDbAssistant.Dmn.Models.Scrap.Json;

using System.Text.Json;

namespace MovieDbAssistant.Dmn.Components.DataProviders;

/// <summary>
/// json data provider == MovieDbScraper output
/// </summary>
public class JsonDataProvider : IDataProvider
{
    readonly static Lazy<JsonSerializerOptions> _jsonSerializerOptions
        = new(() =>
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });

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
