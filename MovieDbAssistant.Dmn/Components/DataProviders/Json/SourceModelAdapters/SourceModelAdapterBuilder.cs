using MovieDbAssistant.Lib.ComponentModels;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.InstanceCounter;

namespace MovieDbAssistant.Dmn.Components.DataProviders.Json.SourceModelAdapters;

/// <summary>
/// The source model adapter builder.
/// </summary>
[Transient]
public sealed class SourceModelAdapterBuilder : IIdentifiable
{
    /// <inheritdoc/>
    public SharedCounter InstanceId { get; }

    public SourceModelAdapterBuilder()
    {
        InstanceId = new(this);
    }

}
