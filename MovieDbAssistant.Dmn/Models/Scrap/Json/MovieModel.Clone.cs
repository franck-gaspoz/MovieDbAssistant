using MovieDbAssistant.Lib.Components.Extensions;

namespace MovieDbAssistant.Dmn.Models.Scrap.Json;

/// <summary>
/// The movie model.
/// </summary>
public sealed partial class MovieModel
{
    /// <summary>
    /// gets a clone
    /// </summary>
    /// <returns>A <see cref="MovieModel? "/></returns>
    public MovieModel Clone()
    => new()
    {
        // root model

        Url = Url,
        MetaData = MetaData.Clone(),
        Sources = Sources.Clone(),
        Id = Id,
        Title = Title,
        Summary = Summary,
        Interests = Interests.Clone()!,
        Rating = Rating,
        RatingCount = RatingCount,
        Duration = Duration,
        ReleaseDate = ReleaseDate,
        Year = Year,
        Vote = Vote,
        Director = Director,
        Writers = Writers.Clone()!,
        Stars = Stars.Clone()!,
        Actors = Actors.Select(x => x.Clone()).ToList(),
        Anecdotes = Anecdotes,
        MinPicUrl = MinPicUrl,
        MinPicWidth = MinPicWidth,
        MinPicAlt = MinPicAlt,
        PicsUrls = PicsUrls != null ? new(PicsUrls) : null,
        PicFullUrl = PicFullUrl,
        PicsSizes = PicsSizes.Clone()!,
        OriginalTitle = OriginalTitle,
        QueryTitle = QueryTitle,

        // db

        // output

        Key = Key,
        Filename = Filename
    };
}
