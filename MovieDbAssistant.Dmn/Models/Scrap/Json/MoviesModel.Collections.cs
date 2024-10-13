namespace MovieDbAssistant.Dmn.Models.Scrap.Json;

/// <summary>
/// The movies model.
/// </summary>
public sealed partial class MoviesModel
{
    /// <summary>
    /// apply distinct on movie list
    /// </summary>
    public MoviesModel Distinct()
    {
        var movies = Movies.GroupBy(x => x.Id)
            .Select(x => x.First())
            .ToList();
        Movies.Clear();
        Movies.AddRange(movies);
        return this;
    }

    /// <summary>
    /// sort by title
    /// </summary>
    public void Sort() => Movies.Sort(new Comparison<MovieModel>(
        (x, y) => x.Title == null ?
            -1 : x.Title.CompareTo(y.Title)
        ));

    /// <summary>
    /// remove unacceptable models
    /// </summary>
    public MoviesModel Filter()
    {
        var movies = Movies
            .Where(x => !string.IsNullOrWhiteSpace(x.Title))
            .ToList();
        Movies.Clear();
        Movies.AddRange(movies);
        return this;
    }

    /// <summary>
    /// merge another model into this one
    /// </summary>
    /// <param name="moviesModel">The movies model.</param>
    /// <returns>A <see cref="MoviesModel? "/>this model</returns>
    public MoviesModel? Merge(MoviesModel? moviesModel)
    {
        if (moviesModel == null) return moviesModel;
        QueryCacheFiles.AddRange(moviesModel.QueryCacheFiles);
        QueryCacheFiles = QueryCacheFiles.Distinct().ToList();
        Movies.AddRange(moviesModel.Movies);
        return this;
    }
}
