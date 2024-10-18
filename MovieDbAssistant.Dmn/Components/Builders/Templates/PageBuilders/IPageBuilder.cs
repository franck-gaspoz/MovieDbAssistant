using MovieDbAssistant.Dmn.Components.Builders.Html;

namespace MovieDbAssistant.Dmn.Components.Builders.Templates.PageBuilders;

/// <summary>
/// interface of a page builder
/// </summary>
public interface IPageBuilder
{
    /// <summary>
    /// build a page
    /// </summary>
    /// <param name="htmlContext">html doc builder context</param>
    /// <param name="data">any data expected by the builder. must match the builder data expected type</param>
    /// <returns>template builder</returns>
    public TemplateBuilder Build(
        HtmlDocumentBuilderContext htmlContext,
        object data);
}
