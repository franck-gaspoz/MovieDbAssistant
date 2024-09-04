using IMDBAssistant.Lib.Components.DependencyInjection.Attributes;
using IMDBAssistant.Lib.Services;

using Microsoft.Extensions.DependencyInjection;

namespace IMDBAssistant.Lib.Components.Builders;

/// <summary>
/// document builder service factory.
/// </summary>
[Singleton()]
public sealed class DocumentBuilderServiceFactory
{
    readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="DocumentBuilderServiceFactory"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    public DocumentBuilderServiceFactory(IServiceProvider serviceProvider)
        => _serviceProvider = serviceProvider;

    /// <summary>
    /// Creates document builder.
    /// </summary>
    /// <returns>A <see cref="DocumentBuilderService"/></returns>
    public DocumentBuilderService CreateDocumentBuilderService()
    {
        var scope = _serviceProvider.CreateScope();
        return scope.ServiceProvider.GetRequiredService<DocumentBuilderService>();
    }
}
