using MovieDbAssistant.Dmn.Models.Scrap.Json;

namespace MovieDbAssistant.Dmn.Components.DataProviders;

/// <summary>
/// json from file data provider == MovieDbScraper output
/// </summary>
public sealed class JsonFileDataProvider : JsonDataProvider
{
    /// <summary>
    /// get from path
    /// </summary>
    /// <param name="path">The path.</param>
    /// <returns>A <see cref="MoviesModel"/></returns>
    public override MoviesModel Get(string source)
        => base.Get(File.ReadAllText(source));    
}
