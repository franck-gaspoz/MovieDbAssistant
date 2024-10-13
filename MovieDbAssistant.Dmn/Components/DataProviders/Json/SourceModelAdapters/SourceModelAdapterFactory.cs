using Microsoft.Extensions.DependencyInjection;

using MovieDbAssistant.Dmn.Models.Queries;
using MovieDbAssistant.Lib.ComponentModels;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Extensions;
using MovieDbAssistant.Lib.Components.InstanceCounter;

namespace MovieDbAssistant.Dmn.Components.DataProviders.Json.SourceModelAdapters;

/// <summary>
/// The source model adapter builder.
/// </summary>
[Transient]
public sealed class SourceModelAdapterFactory : IIdentifiable
{
    const char NamespaceSeparator = '.';

    readonly IServiceProvider _serviceProvider;

    /// <inheritdoc/>
    public SharedCounter InstanceId { get; }

    public SourceModelAdapterFactory(IServiceProvider serviceProvider)
    {
        InstanceId = new(this);
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// creates a source model adapter for the targetted scraper by id
    /// </summary>
    /// <param name="spiderId">spider id</param>
    /// <returns>source model adapter</returns>
    public ISourceQueryModelAdapter Create(SpidersIds spiderId)
    {
        var typeName = typeof(ISourceQueryModelAdapter).Namespace
            + NamespaceSeparator
            + spiderId.ToString()
                .ToFirstUpper()
            + nameof(ISourceQueryModelAdapter)[1..];

        var type = GetType().Assembly
            .GetType(typeName);

        return (ISourceQueryModelAdapter)_serviceProvider
            .GetRequiredService(type!);
    }
}
