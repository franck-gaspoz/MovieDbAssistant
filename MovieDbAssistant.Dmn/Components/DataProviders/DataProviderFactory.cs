using Microsoft.Extensions.DependencyInjection;

using MovieDbAssistant.Dmn.Components.Builder;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;

namespace MovieDbAssistant.Dmn.Components.DataProviders;

#pragma warning disable CA1822 // Marquer les membres comme étant static

/// <summary>
/// data provider factory.
/// </summary>
[Scoped]
public sealed class DataProviderFactory
{
    readonly IServiceProvider _serviceProvider;

    public DataProviderFactory(IServiceProvider serviceProvider)
        => _serviceProvider = serviceProvider;

    /// <summary>
    /// Creates data provider.
    /// </summary>
    /// <param name="dataProviderType">The data provider type</param>
    /// <returns>An <see cref="IDataProvider"/></returns>
    public IDataProvider CreateDataProvider(Type dataProviderType)
        => (IDataProvider)_serviceProvider
            .GetRequiredService(dataProviderType)!;
}
