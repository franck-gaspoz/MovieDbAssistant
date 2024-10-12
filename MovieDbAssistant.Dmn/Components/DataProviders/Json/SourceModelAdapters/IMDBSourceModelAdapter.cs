using Microsoft.Extensions.Options;

using MovieDbAssistant.Dmn.Configuration;
using MovieDbAssistant.Dmn.Models.Queries;
using MovieDbAssistant.Lib.ComponentModels;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Extensions;
using MovieDbAssistant.Lib.Components.InstanceCounter;

namespace MovieDbAssistant.Dmn.Components.DataProviders.Json.SourceModelAdapters;

/// <summary>
/// The IMDB source model adapter.
/// </summary>
[Transient]
public sealed class ImdbSourceModelAdapter :
    IIdentifiable,
    ISourceModelAdapter
{
    const char Separator_StringArrayValues = ',';

    const string Query_Param_Languages = "languages";
    const string Query_Param_Count = "count";
    const string Query_Param_Countries = "country_of_origin";
    const string Query_Param_UserRating = "user-rating";
    const string Query_Param_Titles_Types = "title_type";
    const string Query_Param_Release_Date = "release_date";
    const string Query_Param_Genres = "genres";

    readonly FiltersBuilder _filtersBuilder;
    readonly IOptions<DmnSettings> _dmnSettings;

    /// <inheritdoc/>
    public SharedCounter InstanceId { get; }

    public ImdbSourceModelAdapter(
        FiltersBuilder filtersBuilder,
        IOptions<DmnSettings> dmnSettings)
    {
        InstanceId = new(this);
        _filtersBuilder = filtersBuilder;
        _dmnSettings = dmnSettings;
    }

    void Add(string key, string? value)
    {
        if (value == null) return;
        _filtersBuilder.Add(key, value);
    }

    void Add(string key, string[]? values, Func<string, string>? transform = null)
    {
        transform ??= x => x;
        if (values == null) return;
        if (values.Length == 0) return;
        _filtersBuilder.Add(key,
            string.Join(Separator_StringArrayValues,
                values.Select(x => transform(x))));
    }

    /// <inheritdoc/>
    public string CreateFilter(QueryModel model)
    {
        Add(Query_Param_Languages, model.Languages ??
            _dmnSettings.Value.Scrap.DefaultFilters.Languages);

        Add(Query_Param_Count, (model.Count ??
            _dmnSettings.Value.Scrap.DefaultFilters.Count)
                .ToString());

        Add(Query_Param_Countries, model.Countries
            ?? _dmnSettings.Value.Scrap.DefaultFilters.Countries);

        Add(Query_Param_UserRating, model.UserRating);

        Add(Query_Param_Titles_Types,
            model.TitleTypes?.Select(
                x => x.ToString()
                    .ToFirstLower())
                .ToArray());

        Add(Query_Param_Genres,
            model.Genres?.Select(
                x => x.ToString()
                    .ToLower()
                    .Replace('_', '-'))
                .ToArray());

        Add(Query_Param_Release_Date, model.Year == null ? null
            : model.Year + "-01-01," + model.Year + "-12-31");

        return _filtersBuilder.ToUrlQuery();
    }
}
