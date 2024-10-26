using Microsoft.Extensions.Options;

using MovieDbAssistant.Dmn.Configuration;
using MovieDbAssistant.Dmn.Models.Queries;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Extensions;

namespace MovieDbAssistant.Dmn.Components.DataProviders.Json.SourceModelAdapters;

/// <summary>
/// The IMDB source query model adapter.
/// </summary>
[Transient]
public sealed class ImdbSourceQueryModelAdapter : SourceQueryModelAdapterAbstract
{
    const string Query_Param_Languages = "languages";
    const string Query_Param_Count = "count";
    const string Query_Param_Countries = "country_of_origin";
    const string Query_Param_UserRating = "user-rating";
    const string Query_Param_Titles_Types = "title_type";
    const string Query_Param_Release_Date = "release_date";
    const string Query_Param_Genres = "genres";

    public ImdbSourceQueryModelAdapter(
        FiltersBuilder filtersBuilder,
        IOptions<DmnSettings> dmnSettings) : base(
            filtersBuilder, dmnSettings)
    { }

    /// <inheritdoc/>
    public override string CreateFilter(QueryModel model)
    {
        Add(Query_Param_Languages, model.Languages ??
            DmnSettings.Value.Scrap.DefaultFilters.Languages);

        Add(Query_Param_Count, (model.Count ??
            DmnSettings.Value.Scrap.DefaultFilters.Count)
                .ToString());

        Add(Query_Param_Countries, model.Countries
            ?? DmnSettings.Value.Scrap.DefaultFilters.Countries);

        Add(Query_Param_UserRating, model.UserRating);

        Add(Query_Param_Titles_Types,
            (model.TitleTypes ?? DmnSettings.Value.Scrap
                .DefaultFilters.Types?.ToArray())?
                    .Select(
                        x => x.ToString()
                            .ToFirstLower())
                        .ToArray()
                        );

        Add(Query_Param_Genres,
            model.Genres?.Select(
                x => x.ToString()
                    .ToLower()
                    .Replace('_', '-'))
                .ToArray());

        Add(Query_Param_Release_Date, model.Year == null ? null
            : model.Year + "-01-01," + model.Year + "-12-31");

        return FiltersBuilder.ToUrlQuery();
    }
}
