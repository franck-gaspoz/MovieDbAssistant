using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;

namespace MovieDbAssistant.Dmn.Components.DataProviders;

#pragma warning disable CA1822 // Marquer les membres comme étant static

/// <summary>
/// data provider factory.
/// </summary>
[Scoped]
public sealed class DataProviderFactory
{
    /// <summary>
    /// Creates data provider.
    /// </summary>
    /// <param name="dataProviderType">The data provider type</param>
    /// <returns>An <see cref="IDataProvider"/></returns>
    public IDataProvider CreateDataProvider(Type dataProviderType)
        => (IDataProvider)Activator.CreateInstance(dataProviderType)!;
}
