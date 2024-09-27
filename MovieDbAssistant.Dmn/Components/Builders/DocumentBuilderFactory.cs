using MovieDbAssistant.Dmn.Components.Builder;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;

namespace MovieDbAssistant.Dmn.Components.DataProviders;

#pragma warning disable CA1822 // Marquer les membres comme étant static

/// <summary>
/// document builder factory.
/// </summary>
[Scoped]
public sealed class DocumentBuilderFactory
{
    /// <summary>
    /// Creates data provider.
    /// </summary>
    /// <param name="documentBuilderType">The doc builder type</param>
    /// <returns>An <see cref="IDataProvider"/></returns>
    public IDocumentBuilder CreateDocumentBuilder(Type documentBuilderType)
        => (IDocumentBuilder)Activator.CreateInstance(documentBuilderType)!;
}
