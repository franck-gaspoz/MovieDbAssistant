using Microsoft.Extensions.Logging;

using MovieDbAssistant.Dmn.Models.Scrap.Json;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Logger;

namespace MovieDbAssistant.Dmn.Components.DataProviders;

/// <summary>
/// json from file data provider == MovieDbScraper output
/// </summary>
[Scoped]
public sealed class JsonFileDataProvider : JsonDataProvider
{
    public JsonFileDataProvider(ILogger<JsonDataProvider> logger)
        : base(logger) { }

    /// <summary>
    /// get from path
    /// </summary>
    /// <param name="path">The path.</param>
    /// <returns>A <see cref="MoviesModel"/></returns>
    public override MoviesModel Get(string source)
    {
        Logger.LogInformation(
            this,
            "parse json: "
            + Path.GetFileName(source));

        return base.Get(File.ReadAllText(source));
    }
}
