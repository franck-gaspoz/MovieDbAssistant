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
}
