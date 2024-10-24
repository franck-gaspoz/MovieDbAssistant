using System.Text.Json;
using System.Text.Json.Nodes;

using Newtonsoft.Json;

namespace MovieDbAssistant.Lib.Extensions;

/// <summary>
/// The json extensions.
/// </summary>
public static class JsonExtensions
{
    static readonly Lazy<JsonSerializerOptions> _jsonSerializerOptions =
        new(() => new JsonSerializerOptions
            { WriteIndented = true });

    /// <summary>
    /// Converts to the dynamic.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <returns>An <see cref="object"/></returns>
    public static object? ToDynamic(this JsonElement? element)
    {
        if (element == null) return null;
        var json = JsonObject.Create(element!.Value)!
            .ToJsonString(_jsonSerializerOptions.Value);

        var obj = JsonConvert.DeserializeObject<dynamic>(json);
        return obj!;
    }
}
