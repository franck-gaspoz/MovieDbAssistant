using Microsoft.Extensions.Options;

using MovieDbAssistant.Dmn.Configuration;
using MovieDbAssistant.Dmn.Models.Queries;
using MovieDbAssistant.Lib.ComponentModels;
using MovieDbAssistant.Lib.Components.InstanceCounter;

namespace MovieDbAssistant.Dmn.Components.DataProviders.Json.SourceModelAdapters;

/// <summary>
/// The source query model adapter abstract.
/// </summary>
public abstract class SourceQueryModelAdapterAbstract :
    IIdentifiable,
    ISourceQueryModelAdapter
{
    const char Separator_StringArrayValues = ',';

    /// <inheritdoc/>
    public SharedCounter InstanceId { get; }

    /// <inheritdoc/>
    public abstract string CreateFilter(QueryModel model);

    protected FiltersBuilder FiltersBuilder { get; private set; }

    protected IOptions<DmnSettings> DmnSettings { get; private set; }

    public SourceQueryModelAdapterAbstract(
        FiltersBuilder filtersBuilder,
        IOptions<DmnSettings> dmnSettings)
    {
        InstanceId = new(this);
        FiltersBuilder = filtersBuilder;
        DmnSettings = dmnSettings;
    }

    protected void Add(string key, string? value)
    {
        if (value == null) return;
        FiltersBuilder.Add(key, value);
    }

    protected void Add(string key, string[]? values, Func<string, string>? transform = null)
    {
        transform ??= x => x;
        if (values == null) return;
        if (values.Length == 0) return;
        FiltersBuilder.Add(key,
            string.Join(Separator_StringArrayValues,
                values.Select(x => transform(x))));
    }
}
