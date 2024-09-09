using MovieDbAssistant.Lib.ComponentModels;

namespace MovieDbAssistant.Lib.Components.Extensions;

/// <summary>
/// The I identifiable extensions.
/// </summary>
public static class IIdentifiableExtensions
{
    /// <summary>
    /// gets an id string for the identifiable
    /// </summary>
    /// <typeparam name="T"/>
    /// <param name="obj">The obj.</param>
    /// <param name="id">The id.</param>
    /// <returns>A <see cref="string"/></returns>
    public static string Id(this IIdentifiable obj)
        => obj.GetType().Name + " #" + obj.InstanceId.Value;
}
