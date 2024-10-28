using Microsoft.Extensions.Options;

using MovieDbAssistant.Dmn.Configuration;
using MovieDbAssistant.Dmn.Models.Queries;
using MovieDbAssistant.Dmn.Models.Scrap.Json;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;

namespace MovieDbAssistant.Dmn.Components.Builders.Models;

/// <summary>
/// The movie model from query builder.
/// </summary>
[Transient]
public sealed class MovieModelFromQueryBuilder
{
    QueryModel? _query;
    MovieModel? _data;
    readonly IOptions<DmnSettings> _settings;

    /// <summary>
    /// Initializes a new instance of the <see cref="MovieModelFromQueryBuilder"/> class.
    /// </summary>
    public MovieModelFromQueryBuilder(
        IOptions<DmnSettings> settings
    ) => _settings = settings;

    /// <summary>
    /// setup the builder
    /// </summary>
    /// <param name="queryModel">The query model.</param>
    /// <param name="data">The data.</param>
    /// <returns>A <see cref="MovieModelFromQueryBuilder"/></returns>
    public MovieModelFromQueryBuilder Setup(
        QueryModel queryModel,
        MovieModel data)
    {
        _query = queryModel;
        _data = data;
        return this;
    }

    /// <summary>
    /// build
    /// </summary>
    /// <returns>A <see cref="MovieModel"/></returns>
    public MovieModel Build()
    {
        var o = _data!.Clone();

        o.OriginalTitle = _data!.Title;
        if (_settings.Value.Scrap.Prefers.QueryTitle)
            o.Title = _query?.Title;

        o.OriginalYear = _data.Year!;
        if (_settings.Value.Scrap.Prefers.QueryYear
            && _query?.Year != null)
            o.Year = _query?.Year;

        if (_settings.Value.Scrap.Prefers.QueryYearIfDataTitleIsNull
            && _data.Year == null
            && _query?.Year != null)
            o.Year = _query?.Year;

        return o;
    }
}
