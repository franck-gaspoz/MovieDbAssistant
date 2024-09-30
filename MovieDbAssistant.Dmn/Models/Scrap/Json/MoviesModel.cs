namespace MovieDbAssistant.Dmn.Models.Scrap.Json;

#pragma warning disable CD1606 // The property must have a documentation header.

/// <summary>
/// The movies model.
/// </summary>
public sealed class MoviesModel
{
    public List<MovieModel> Movies { get; set; } = [];

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
}
