using Microsoft.Extensions.DependencyInjection;

using MovieDbAssistant.Dmn.Components.DataProviders;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;

namespace MovieDbAssistant.Dmn.Components.Builders.Document;

#pragma warning disable CA1822 // Marquer les membres comme étant static

/// <summary>
/// document builder factory.
/// </summary>
[Scoped]
public sealed class DocumentBuilderFactory
{
    readonly IServiceProvider _serviceProvider;

    public DocumentBuilderFactory(IServiceProvider serviceProvider)
        => _serviceProvider = serviceProvider;

    /// <summary>
    /// Creates data provider.
    /// </summary>
    /// <param name="documentBuilderType">The doc builder type</param>
    /// <returns>An <see cref="IDataProvider"/></returns>
    public IDocumentBuilder CreateDocumentBuilder(Type documentBuilderType)
        => (IDocumentBuilder)_serviceProvider
            .GetRequiredService(documentBuilderType)!;
}
