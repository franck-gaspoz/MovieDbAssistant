namespace MovieDbAssistant.Dmn.Models.Scrap.Json;

/// <summary>
/// The movies model.
/// </summary>
public sealed partial class MoviesModel
{
    /// <summary>
    /// gets a clone
    /// </summary>
    /// <returns>A <see cref="MoviesModel"/></returns>
    public MoviesModel Clone()
        => new()
        {
            Movies = Movies
        };
}
