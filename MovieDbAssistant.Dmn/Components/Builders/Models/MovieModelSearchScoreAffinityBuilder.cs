using MovieDbAssistant.Dmn.Models.Queries;
using MovieDbAssistant.Dmn.Models.Scrap.Json;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Extensions;

namespace MovieDbAssistant.Dmn.Components.Builders.Models;

/// <summary>
/// The movie model performance builder.
/// </summary>
[Transient]
public sealed class MovieModelSearchScoreAffinityBuilder : ScoreBuilder
{
    MovieModel? _movie;
    QueryModel? _query;

    /// <summary>
    /// Initializes a new instance of the <see cref="MovieModelSearchScoreAffinityBuilder"/> class.
    /// </summary>
    /// <param name="movie">The movie.</param>
    /// <param name="query">The query.</param>
    public MovieModelSearchScoreAffinityBuilder() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="MovieModelSearchScoreAffinityBuilder"/> class.
    /// </summary>
    /// <param name="movie">The movie.</param>
    /// <param name="query">The query.</param>
    public MovieModelSearchScoreAffinityBuilder(
        MovieModel movie, QueryModel query)
    {
        _movie = movie;
        _query = query;
    }

    /// <summary>
    /// setup the builder
    /// </summary>
    /// <param name="movie">The movie.</param>
    /// <param name="query">The query.</param>
    /// <returns>this <see cref="MovieModelSearchScoreAffinityBuilder"/></returns>
    public MovieModelSearchScoreAffinityBuilder Setup(
        MovieModel movie, QueryModel query)
    {
        _movie = movie;
        _query = query;
        return this;
    }

    /// <summary>
    /// build leven distance note: 0: worse, .., 1:dist=0 1.1: match
    /// <para>strict equality is superior by 0.1 to swap equality (dist=0)</para>
    /// </summary>
    /// <param name="src">src</param>
    /// <param name="target">target</param>
    /// <returns>note</returns>
    static double BuildDistance(string? src, string? target)
    {
        double distLeven = 0;
        if (src == null || target == null)
            distLeven = 0;
        else
        {
            var distLevenStrict = src.LevenshteinDistance(target);
            distLeven = 1d / (distLevenStrict + 1);
            if (distLevenStrict == 0)
                distLeven += 0.1;
        }
        return distLeven;
    }

    /// <summary>
    /// build numeric distance note: 0: worse, ..,  1: match
    /// </summary>
    /// <param name="src">src</param>
    /// <param name="target">target</param>
    /// <returns>note</returns>
    static double BuildDistance(double? src, double? target)
    {
        var srcd = src == null ? int.MaxValue : src!.Value;
        var targetd = target == null ? int.MinValue : target!.Value;
        var dist = (src == null || target == null) ? 0
            : srcd == targetd ? 1d
                : 1d / Math.Abs(srcd - targetd);
        return dist;
    }

    /// <summary>
    /// build the search score
    /// </summary>
    /// <param name="queryModel">The query model.</param>
    /// <returns>An <see cref="int"/></returns>
    public double Build()
    {
        if (/*_query!.Title?.ToLower()?.StartsWith("ant man") == true*/
            //&& 
            _movie!.Title/*?.ToLower()*/ == "Astérix & Obélix: L'Empire du Milieu"
            //&& _movie.Director == "Peyton Reed"
            )
            //Debugger.Break()
            ;

        const double titleWeight = 10d;
        const double yearWeight = 2d;

        AddNote(BuildDistance(
            _query!.Year.NotNullThen<string?, double>(x => Convert.ToDouble(x)),
            _movie!.Year.NotNullThen<string?, double>(x => Convert.ToDouble(x))
            ), yearWeight);

        AddNote(BuildDistance(
            _query.Title?.ToLower(),
            _movie.Title?.ToLower()
            ), titleWeight);

        Average();

        return Note;
    }
}
