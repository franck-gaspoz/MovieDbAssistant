using System.Text;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using MovieDbAssistant.Dmn.Configuration;
using MovieDbAssistant.Dmn.Models.Queries;
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
    /// <param name="dmnSettings">The config.</param>
    /// <returns>A <see cref="string"/></returns>
    public static string UpdateFilename(
        this MovieModel data,
        IOptions<DmnSettings> dmnSettings)
    {
        var f = data.Filename ?? data.Key + dmnSettings.Value.Build.Html.Extension;
        data.Filename = f;
        return f;
    }

    /// <summary>
    /// Setups the model (setup extra properties)
    /// </summary>
    /// <param name="data">movie model.</param>
    /// <param name="dmnSettings">The config.</param>
    public static void SetupModel(
        this MovieModel data,
        IOptions<DmnSettings> dmnSettings)
    {
        var key = data.Title!.ToHexString();
        StringBuilder sb = new();
        foreach (var c in key)
            if (char.IsAsciiLetterOrDigit(c))
                sb.Append(c);
        key = new string(sb.ToString().ToCharArray());
        data.Key = key;
        data.UpdateFilename(dmnSettings);
    }

    /// <summary>
    /// setup the movie models (setup extra properties)
    /// </summary>
    /// <param name="data">movies model</param>
    /// <param name="dmnSettings">configuration</param>
    public static void SetupModel(
        this MoviesModel data,
        IOptions<DmnSettings> dmnSettings)
    {
        foreach (var movieModel in data.Movies)
            movieModel.SetupModel(dmnSettings);
    }

    /// <summary>
    /// gets the instance id of the query model
    /// </summary>
    /// <param name="model">query model</param>
    /// <returns>instance id</returns>
    public static int InstanceId(this QueryModel model)
        => model.Metadata!.InstanceId.Value;
}
