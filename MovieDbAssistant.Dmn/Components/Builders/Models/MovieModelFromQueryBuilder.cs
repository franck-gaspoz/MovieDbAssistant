using MovieDbAssistant.Dmn.Models.Queries;
using MovieDbAssistant.Dmn.Models.Scrap.Json;

namespace MovieDbAssistant.Dmn.Components.Builders.Models;

/// <summary>
/// The movie model from query builder.
/// </summary>
public sealed class MovieModelFromQueryBuilder
{
    readonly QueryModel _queryModel;
    readonly MovieModel _data;

    public MovieModelFromQueryBuilder(
        QueryModel queryModel,
        MovieModel data)
    {
        _queryModel = queryModel;
        _data = data;
    }

    public MovieModel Build()
    {
        var o = new MovieModel();
        return o;
    }
}
