using MovieDbAssistant.Lib.ComponentModels;

namespace MovieDbAssistant.Lib.Extensions;

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
    /// <param name="getName">a get name func</param>
    /// <returns>A <see cref="string"/></returns>
    public static string Id(
        this IIdentifiable obj,
        Func<string>? getName = null)
    {
        var res = !string.IsNullOrWhiteSpace(obj.GetNamePrefix()) ?
                    obj.GetNamePrefix() + ": "
                    : "";
        res += obj.ShortId(getName);
        return res;
    }

    /// <summary>
    /// gets the textual object short id (prefix less)
    /// </summary>
    /// <param name="obj">The obj.</param>
    /// <param name="getName">a get name func</param>
    /// <returns>A <see cref="string"/></returns>
    public static string ShortId(
        this IIdentifiable obj,
        Func<string>? getName = null)
    {
        var res = "";
        res +=
            getName?.Invoke() ?? obj.GetName()
            + obj.InstanceIdPostfix();
        return res;
    }

    /// <summary>
    /// gets the textual object named instance id (prefix less)
    /// </summary>
    /// <param name="obj">The obj.</param>
    /// <returns>A <see cref="string"/></returns>
    public static string InstanceIdPostfix(
        this IIdentifiable obj)
    {
        var res = "";
        res +=
            " #"
            + obj.InstanceId.Value;
        return res;
    }
}
