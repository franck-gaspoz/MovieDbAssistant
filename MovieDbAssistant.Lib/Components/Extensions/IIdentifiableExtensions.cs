using MovieDbAssistant.Lib.ComponentModels;

namespace MovieDbAssistant.Lib.Components.Extensions;

/// <summary>
/// The I identifiable extensions.
/// </summary>
public static class IIdentifiableExtensions
{
    /// <summary>
    /// gets an message identified by the textual object id
    /// </summary>
    /// <param name="obj">The obj.</param>
    /// <param name="text">The text.</param>
    /// <returns>A <see cref="string"/></returns>
    public static string IdWith(
        this IIdentifiable obj,
        string text)
        => obj.Id() + ": " + text;

    /// <summary>
    /// gets the textual object id
    /// </summary>
    /// <param name="obj">The obj.</param>
    /// <returns>A <see cref="string"/></returns>
    public static string Id(this IIdentifiable obj)
        => obj.GetType().Name + " #" + obj.InstanceId.Value;

}
