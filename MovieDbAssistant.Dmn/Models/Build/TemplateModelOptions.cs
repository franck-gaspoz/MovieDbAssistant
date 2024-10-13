using MovieDbAssistant.Dmn.Components.Builders.Document;

namespace MovieDbAssistant.Dmn.Models.Build;

/// <summary>
/// The template model options.
/// </summary>
public sealed class TemplateModelOptions
{
    /// <summary>
    /// Gets or sets the page list.
    /// </summary>
    /// <value>A <see cref="TemplateModelPageOption"/></value>
    public TemplateModelPageOption PageList { get; set; }

    /// <summary>
    /// Gets or sets the page detail.
    /// </summary>
    /// <value>A <see cref="TemplateModelPageOption"/></value>
    public TemplateModelPageOption PageDetail { get; set; }

    /// <summary>
    /// paths
    /// </summary>
    public PathsModel Paths { get; set; }

    /// <summary>
    /// repo link
    /// </summary>
    public string? RepoLink { get; set; }

    /// <summary>
    /// help link
    /// </summary>
    public string? HelpLink { get; set; }

    /// <summary>
    /// author link
    /// </summary>
    public string? AuthorLink { get; set; }

    /// <summary>
    /// alternate url page movie list movie pic not available
    /// </summary>
    public string? ListMoviePicNotAvailable { get; set; }

    /// <summary>
    /// alternate url page movie detail movie pic not available
    /// </summary>
    public string? DetailMoviePicNotAvailable { get; set; }

    /// <summary>
    /// alternate url page movie list movie pic not found
    /// </summary>
    public string? ListMoviePicNotFound { get; set; }

    /// <summary>
    /// alternate url page movie detail movie pic not found
    /// </summary>
    public string? DetailMoviePicNotFound { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TemplateModelOptions"/> class.
    /// </summary>
    /// <param name="pageList">page list</param>
    /// <param name="pageDetail">The page detail.</param>
    public TemplateModelOptions(
        TemplateModelPageOption pageList,
        TemplateModelPageOption pageDetail)
        => (PageDetail, PageList) = (pageDetail, pageList);


    /// <summary>
    /// Pages index path.
    /// </summary>
    /// <param name="docBuilderContext">doc builder context</param>
    /// <param name="extension">extension</param>
    /// <returns>A <see cref="string"/></returns>
    public string PageIndexPath(DocumentBuilderContext docBuilderContext,
        string extension)
        => docBuilderContext.TplFilePath(PageList.Filename!, extension);
}
