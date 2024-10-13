using Microsoft.Extensions.DependencyInjection;

using MovieDbAssistant.Dmn.Models.Queries;
using MovieDbAssistant.Dmn.Models.Scrap.Json;

namespace MovieDbAssistant.Dmn.Components.Builders.Models.Extensions;

/// <summary>
/// The movie models extensions.
/// </summary>
public static class MoviesModelSearchScoreBuilderExtensions
{
    /// <summary>
    /// Build search score.
    /// </summary>
    /// <param name="moviesModel">The movies model.</param>
    /// <param name="serviceProvider">The service provider.</param>
    /// <param name="query">The query.</param>
    /// <returns>A <see cref="MoviesModel"/></returns>
    public static MoviesModel BuildSearchScore(
        this MoviesModel moviesModel,
        IServiceProvider serviceProvider,
        QueryModel query)
    {
        foreach (var m in moviesModel.Movies)
        {
            var affinity = serviceProvider.GetRequiredService<MovieModelSearchScoreAffinityBuilder>()
                .Setup(m, query)
                    .Build();

            var dataCompletion = serviceProvider.GetRequiredService<MovieModelSearchScoreDataCompletionBuilder>()
                .Setup(m)
                    .Build();

            m.MetaData.SearchScore!.Affinity = affinity;
            m.MetaData.SearchScore!.DataCompletion = dataCompletion;
        }
        return moviesModel;
    }
}
