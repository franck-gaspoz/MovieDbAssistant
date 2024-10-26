using MovieDbAssistant.Dmn.Components.Builders.Html;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;

namespace MovieDbAssistant.Dmn.Components.Builders.Templates.PageBuilders;

/// <summary>
/// builder for page detail
/// </summary>
[Transient]
public sealed class PageBuilderDetail : IPageBuilder
{
    /// <inheritdoc/>
    public TemplateBuilder Build(
        HtmlDocumentBuilderContext htmlContext,
        object data)
    {
        throw new NotImplementedException();
    }
}
