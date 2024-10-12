using MovieDbAssistant.Dmn.Models.Scrap.Json;

namespace MovieDbAssistant.Dmn.Components.Builders.Document;

public interface IDocumentBuilder
{
    /// <summary>
    /// build the output document(s)
    /// </summary>
    /// <param name="context">doc builder context</param>
    /// <param name="data">movies data</param>
    void Build(DocumentBuilderContext context, MoviesModel data);
}