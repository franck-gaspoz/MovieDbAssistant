﻿using MovieDbAssistant.Dmn.Components.Builders.Document;

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
    /// Initializes a new instance of the <see cref="TemplateModelOptions"/> class.
    /// </summary>
    /// <param name="pageList">page list</param>
    /// <param name="pageDetail">The page detail.</param>
    /// <param name="paths">paths</param>
    public TemplateModelOptions(
        TemplateModelPageOption pageList,
        TemplateModelPageOption pageDetail,
        PathsModel paths)
        => (PageDetail, PageList, Paths)
            = (pageDetail, pageList, paths);

    /// <summary>
    /// Pages index path.
    /// </summary>
    /// <param name="docBuilderContext">doc builder context</param>
    /// <param name="extension">extension</param>
    /// <returns>A <see cref="string"/></returns>
    public string PageIndexPath(DocumentBuilderContext docBuilderContext,
        string extension)
        => docBuilderContext.TplFilePath(
            PageList.Filename!, extension);
}
