using static System.Runtime.InteropServices.JavaScript.JSType;

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
    public void Distinct()
    {
        var ids = Movies.Select(x => x.Id).Distinct().ToList();
        var movies = Movies.Where(x => ids.Contains(x.Id)).ToList();
        Movies.Clear();
        Movies.AddRange(movies);
    }
}
