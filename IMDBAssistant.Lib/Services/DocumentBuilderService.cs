using IMDBAssistant.Lib.Components.DependencyInjection.Attributes;

namespace IMDBAssistant.Lib.Services;

/// <summary>
/// The document builder service.
/// </summary>
[Scoped()]
public sealed class DocumentBuilderService
{
    /// <summary>
    /// build a document
    /// </summary>
    /// <param name="file">The file.</param>
    /// <param name="dataProvider">The data provider.</param>
    /// <param name="documentBuilder">The document builder.</param>
    public void Build(
        string file,
        Type dataProvider,
        Type documentBuilder)
    {

    }
}
