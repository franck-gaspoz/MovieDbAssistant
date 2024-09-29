using Microsoft.Extensions.Configuration;

using MovieDbAssistant.Dmn.Models.Scrap.Json;

using static MovieDbAssistant.Dmn.Globals;

namespace MovieDbAssistant.Dmn.Models.Extensions;

/// <summary>
/// models extensions
/// </summary>
public static class ModelExtensions
{
    /// <summary>
    /// update the filename in the model with the concrete one during template build
    /// </summary>
    /// <param name="data">The data.</param>
    /// <param name="config">The config.</param>
    /// <returns>A <see cref="string"/></returns>
    public static string UpdateFilename(this MovieModel data, IConfiguration config)
    {
        var f = (data.Filename ?? data.Key) + config[Build_HtmlFileExt];
        data.Filename = f;
        return f;
    }
}
