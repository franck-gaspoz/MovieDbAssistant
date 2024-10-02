using Microsoft.Extensions.Configuration;

using MovieDbAssistant.Dmn.Models.Scrap.Json;
using MovieDbAssistant.Lib.Components.Extensions;

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
        var f = data.Filename ?? data.Key + config[Build_HtmlFileExt];
        data.Filename = f;
        return f;
    }

    /// <summary>
    /// Setups the model (setup extra properties)
    /// </summary>
    /// <param name="data">movie model.</param>
    /// <param name="config">The config.</param>
    public static void SetupModel(
        this MovieModel data,
        IConfiguration config)
    {
        var key = data.Title!.ToHexString();
        data.Key = key;
        data.UpdateFilename(config);
    }

    /// <summary>
    /// setup the movie models (setup extra properties)
    /// </summary>
    /// <param name="data">movies model</param>
    /// <param name="config">configuration</param>
    public static void SetupModel(
        this MoviesModel data,
        IConfiguration config)
    {
        foreach (var movieModel in data.Movies)
            movieModel.SetupModel(config);
    }
}
