using System.Text.Json;

using MovieDbAssistant.Lib.Extensions;

namespace MovieDbAssistant.Dmn.Components.Json;

/// <summary>
/// The lower case naming policy.
/// </summary>
public sealed class FirstLowerCaseNamingPolicy : JsonNamingPolicy
{
    /// <summary>
    /// Convert the name.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns>A <see cref="string"/></returns>
    public override string ConvertName(string name)
    {
        if (string.IsNullOrEmpty(name) || !char.IsUpper(name[0]))
            return name;

        return name.ToFirstLower();
    }
}
