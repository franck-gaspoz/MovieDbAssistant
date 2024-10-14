using MovieDbAssistant.Dmn.Models.Scrap.Json;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Extensions;

namespace MovieDbAssistant.Dmn.Components.Builders.Models;

/// <summary>
/// The movie model search score affinity builder.
/// </summary>
[Transient]
public sealed class MovieModelSearchScoreDataCompletionBuilder
     : ScoreBuilder
{
    MovieModel? _movie;

    /// <summary>
    /// Initializes a new instance of the <see cref="MovieModelSearchScoreAffinityBuilder"/> class.
    /// </summary>
    public MovieModelSearchScoreDataCompletionBuilder() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="MovieModelSearchScoreAffinityBuilder"/> class.
    /// </summary>
    /// <param name="movie">The movie.</param>
    public MovieModelSearchScoreDataCompletionBuilder(MovieModel movie) => _movie = movie;

    /// <summary>
    /// setup the builder
    /// </summary>
    /// <param name="movie">The movie.</param>
    /// <returns>this <see cref="MovieModelSearchScoreAffinityBuilder"/></returns>
    public MovieModelSearchScoreDataCompletionBuilder Setup(MovieModel movie)
    {
        _movie = movie;
        return this;
    }

    /// <summary>
    /// add a note 1 * weight if o is not null
    /// </summary>
    /// <param name="o">checked object</param>
    /// <param name="weight">weight</param>
    public void AddNote(object? o, double weight)
    {
        if (o != null)
            Note += 1d * weight;
        TotWeight += weight;
    }

    /// <summary>
    /// build the search score
    /// </summary>
    /// <param name="queryModel">The query model.</param>
    /// <returns>An <see cref="int"/></returns>
    public double Build()
    {
        const double highWeight = 3d;
        const double medWeight = 2d;
        const double lowWeight = 1d;

        AddNote(_movie!.Title, highWeight);
        AddNote(_movie!.Year, highWeight);
        AddNote(_movie!.Summary, highWeight);

        AddNote(_movie!.Rating, highWeight);
        AddNote(_movie!.RatingCount, highWeight);

        AddNote(_movie!.Duration, medWeight);
        AddNote(_movie!.ReleaseDate, medWeight);
        AddNote(_movie!.MinPicUrl, medWeight);
        AddNote(_movie!.MinPicWidth, medWeight);
        AddNote(_movie!.MinPicWidth, medWeight);

        AddNote(_movie!.Vote, lowWeight);
        AddNote(_movie!.Director, lowWeight);
        AddNote(_movie!.Writers.NullIfEmpty(), lowWeight);
        AddNote(_movie!.Interests.NullIfEmpty(), lowWeight);
        AddNote(_movie!.Stars.NullIfEmpty(), lowWeight);
        AddNote(_movie!.Actors.NullIfEmpty(), lowWeight);
        AddNote(_movie!.PicsUrls.NullIfEmpty(), lowWeight);
        AddNote(_movie!.Anecdotes, lowWeight);
        AddNote(_movie!.MinPicAlt, lowWeight);
        AddNote(_movie!.MedPicUrl, lowWeight);
        AddNote(_movie!.PicFullUrl, lowWeight);
        AddNote(_movie!.PicsSizes.NullIfEmpty(), lowWeight);

        Average();

        return Note;
    }
}
