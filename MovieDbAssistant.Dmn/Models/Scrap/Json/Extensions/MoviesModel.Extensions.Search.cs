using System.Linq;

namespace MovieDbAssistant.Dmn.Models.Scrap.Json.Extensions;

/// <summary>
/// The movies model extensions: search functions
/// </summary>
public static partial class MoviesModelExtensions
{
    /// <summary>
    /// Having best search score affinity. Pick the first one if several having same affinity
    /// </summary>
    /// <param name="data">The data.</param>
    /// <returns>movie model or null</returns>
    public static MovieModel? HavingBestSearchScoreAffinity(this MoviesModel data)
    {
        if (data.Movies.Count == 0) return null;
        var set = data.Movies
            .Where(x => x.MetaData.SearchScore!=null &&
                x.MetaData.SearchScore.Affinity != null);
        
        if (!set.Any())
        {
            return data.Movies.Count > 1
                ? data.Movies.First()
                : null;
        }
        
        var max = set.Max(x => x.MetaData.SearchScore!.Affinity);
        return set.FirstOrDefault(x => x.MetaData.SearchScore!.Affinity == max);
    }

    /// <summary>
    /// Having best search score data completion.
    /// </summary>
    /// <param name="data">The data.</param>
    /// <returns>movie model or null</returns>
    public static MovieModel? HavingBestSearchScoreDataCompletion(this MoviesModel data)
    {
        if (data.Movies.Count == 0) return null;
        var set = data.Movies
            .Where(x => x.MetaData.SearchScore != null &&
                x.MetaData.SearchScore.Affinity != null);

        if (!set.Any())
        {
            return data.Movies.Count > 1
                ? data.Movies.First()
                : null;
        }

        var max = set.Max(x => x.MetaData.SearchScore!.Affinity);
        return set.FirstOrDefault(x => x.MetaData.SearchScore!.Affinity == max);
    }

    /// <summary>
    /// Having best search score.
    /// </summary>
    /// <param name="data">The data.</param>
    /// <returns>movie model or null</returns>
    public static MovieModel? HavingBestSearchScore(this MoviesModel data)
    {
        if (data.Movies.Count == 0) return null;
        var set = data.Movies
            .Where(x => x.MetaData.SearchScore != null &&
                x.MetaData.SearchScore.Value != null);

        if (!set.Any())
        {
            return data.Movies.Count > 1
                ? data.Movies.First()
                : null;
        }

        var max = set.Max(x => x.MetaData.SearchScore!.Value);
        return set.FirstOrDefault(x => x.MetaData.SearchScore!.Value == max);
    }
}
