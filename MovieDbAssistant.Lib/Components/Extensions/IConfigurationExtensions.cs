using Microsoft.Extensions.Configuration;

namespace MovieDbAssistant.Lib.Components.Extensions;

/// <summary>
/// The IConfiguration extensions.
/// </summary>
public static class IConfigurationExtensions
{
    /// <summary>
    /// Get bool.
    /// </summary>
    /// <param name="config">The config.</param>
    /// <param name="key">The key.</param>
    /// <returns>A <see cref="bool"/></returns>
    public static bool GetBool(
        this IConfiguration config,
        string key)
        => config.GetSection(key)!.Get<bool>();

    /// <summary>
    /// Get int
    /// </summary>
    /// <param name="config">The config.</param>
    /// <param name="key">The key.</param>
    /// <returns>A <see cref="int"/></returns>
    public static int GetInt(
        this IConfiguration config,
        string key)
        => config.GetSection(key)!.Get<int>();

    /// <summary>
    /// Get the value of section as type T
    /// </summary>
    /// <typeparam name="T"/>
    /// <param name="config">The config.</param>
    /// <param name="key">The key.</param>
    /// <returns>A <typeparamref name="T"/></returns>
    public static T GetAs<T>(IConfiguration config, string key)
        => config.GetSection(key)
            .Get<T>()!;

    /// <summary>
    /// Get the array of string from section with given key
    /// </summary>
    /// <param name="config">The config.</param>
    /// <param name="key">The key.</param>
    /// <returns>An array of strings</returns>
    public static string[] GetArray(this IConfiguration config, string key)
        => GetAs<string[]>(config, key);
}
