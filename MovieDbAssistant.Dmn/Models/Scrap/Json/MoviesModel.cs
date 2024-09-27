namespace MovieDbAssistant.Dmn.Models.Scrap.Json;

#pragma warning disable CD1606 // The property must have a documentation header.

/// <summary>
/// The movies model.
/// </summary>
public sealed class MoviesModel
{
    public List<MovieModel> Movies { get; set; } = [];
}
