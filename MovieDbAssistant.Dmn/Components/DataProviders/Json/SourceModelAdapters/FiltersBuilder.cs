using System.Text.Encodings.Web;

using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;

namespace MovieDbAssistant.Dmn.Components.DataProviders.Json.SourceModelAdapters;

/// <summary>
/// The filters builder.
/// </summary>
[Transient]
public sealed class FiltersBuilder
{
    readonly Stack<string> _filters = new();

    /// <summary>
    /// add a filter to the filters group
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    public void Add(string key, string value)
    {
        var ftr = key + "=" +
            UrlEncoder.Default.Encode(value);
        _filters.Push(ftr);
    }

    /// <summary>
    /// Converts to url query.
    /// </summary>
    /// <returns>A <see cref="string"/></returns>
    public string ToUrlQuery()
        => string.Join('&', _filters.ToList());
}
