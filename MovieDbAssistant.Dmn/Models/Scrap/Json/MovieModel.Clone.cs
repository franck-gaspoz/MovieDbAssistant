﻿namespace MovieDbAssistant.Dmn.Models.Scrap.Json;

/// <summary>
/// The movie model.
/// </summary>
public sealed partial class MovieModel
{
    /// <summary>
    /// gets a clone
    /// </summary>
    /// <returns>A <see cref="MovieModel? "/></returns>
    public MovieModel? Clone()
    => new()
    {
        // root model

        Url = Url,
        MetaData = MetaData.Clone(),
        Sources = Sources.Clone(),
        Id = Id,
        Title = Title,
        Summary = Summary,
        Interests = new(Interests),
        Rating = Rating,
        RatingCount = RatingCount,
        Duration = Duration,
        ReleaseDate = ReleaseDate,
        Year = Year,
        Vote = Vote,
        Director = Director,
        Writers = new(Writers),
        Stars = new(Stars),
        Actors = Actors.Select(x => x.Clone()).ToList(),
        Anecdotes = Anecdotes,
        MinPicUrl = MinPicUrl,
        MinPicWidth = MinPicWidth,
        MinPicAlt = MinPicAlt,
        PicsUrls = PicsUrls != null ? new(PicsUrls) : null,
        PicFullUrl = PicFullUrl,
        PicsSizes = new(PicsSizes),

        // db

        // output

        Key = Key,
        Filename = Filename
    };
}
