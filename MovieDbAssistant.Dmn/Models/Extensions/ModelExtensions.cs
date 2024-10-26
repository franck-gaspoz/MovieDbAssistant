using Microsoft.Extensions.Options;

using MovieDbAssistant.Dmn.Components.Builders.Templates;
using MovieDbAssistant.Dmn.Components.Builders.Templates.PageBuilders;
using MovieDbAssistant.Dmn.Configuration;
using MovieDbAssistant.Dmn.Models.Build;
using MovieDbAssistant.Dmn.Models.Queries;
using MovieDbAssistant.Dmn.Models.Scrap.Json;
using MovieDbAssistant.Lib.Extensions;

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
        var key = data.HashKey();
        data.Key = key;
        data.UpdateFilename(dmnSettings);
    }

    /// <summary>
    /// Hashes the key.
    /// </summary>
    /// <param name="data">The data.</param>
    /// <returns>A <see cref="string"/></returns>
    public static string HashKey(this MovieModel? data) =>
        (
            data?.Title!.ToHexLettersAndDigitsString()
            + data?.Id!.ToHexLettersAndDigitsString()
        )
        ?? string.Empty
        ;

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

    /// <summary>
    /// get pages having a layout
    /// </summary>
    /// <param name="tpl">The tpl.</param>
    /// <param name="layout">The layout.</param>
    /// <returns>A list of pagemodels.</returns>
    public static IEnumerable<PageModel> GetPages(this TemplateModel tpl, Layouts layout)
        => tpl.Pages
            .Where(x => x.Layout == layout.ToString());

    /// <summary>
    /// Page the list.
    /// </summary>
    /// <param name="tpl">The tpl.</param>
    /// <returns>A <see cref="PageModel? "/></returns>
    public static PageModel? PageList(this TemplateModel tpl)
        => tpl.GetPages(Layouts.List)
            .FirstOrDefault();

    /// <summary>
    /// Pages the detail.
    /// </summary>
    /// <param name="tpl">The tpl.</param>
    /// <returns>A <see cref="PageModel? "/></returns>
    public static PageModel? PageDetail(this TemplateModel tpl)
        => tpl.GetPages(Layouts.Detail)
            .FirstOrDefault();
}
