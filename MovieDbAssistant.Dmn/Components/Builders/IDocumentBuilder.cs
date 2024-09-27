using MovieDbAssistant.Dmn.Components.Builders;
using MovieDbAssistant.Dmn.Models.Scrap.Json;

namespace MovieDbAssistant.Dmn.Components.Builder;

public interface IDocumentBuilder
{
    /// <summary>
    /// build the output document(s)
    /// </summary>
    /// <param name="context">doc builder context</param>
    /// <param name="data">movies data</param>
    void Build(DocumentBuilderContext context, MoviesModel data);
}