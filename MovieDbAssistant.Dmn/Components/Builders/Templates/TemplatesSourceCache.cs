using MovieDbAssistant.Dmn.Components.Builders.Templates.PageBuilders;
using MovieDbAssistant.Dmn.Models.Build;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Extensions;

namespace MovieDbAssistant.Dmn.Components.Builders.Templates;

/// <summary>
/// template properties model
/// </summary>
[Singleton]
public sealed class TemplatesSourceCache
{
    static readonly Dictionary<string, (PageModel Page, string Content)> _cache = [];

    public static readonly object Lock = new();

    /// <summary>
    /// Add or replace into cache
    /// </summary>
    /// <param name="id">The id.</param>
    /// <param name="content">The content.</param>
    public void AddOrReplace(PageModel page, string content)
    {
        lock (Lock)
            _cache.AddOrReplace(
                page.Id,
                (page, content));
    }

    /// <summary>
    /// get from cache
    /// </summary>
    /// <param name="id">The id.</param>
    /// <returns>A <see cref="string? "/></returns>
    public string? Get(string id)
    {
        lock (Lock)
            return _cache.TryGetValue(
                id,
                out var item) ?
                    item.Content : null;
    }

    /// <summary>
    /// load page templates from a template
    /// </summary>
    /// <param name="templatePath"></param>
    /// <param name="pages"></param>
    public void Load(
        string templatePath,
        List<PageModel> pages)
    {
        lock (Lock)
        {
            foreach (var page in pages)
            {
                if (!_cache.ContainsKey(page.Id))
                {
                    var content = File.ReadAllText(
                        Path.Combine(
                            templatePath,
                            page.Tpl));
                    AddOrReplace(
                        page,
                        content);
                }
            }
        }
    }

    /// <summary>
    /// get pages having a layout
    /// </summary>
    /// <param name="tpl">The tpl.</param>
    /// <param name="layout">The layout.</param>
    /// <returns>A list of pagemodels.</returns>
    public IEnumerable<(PageModel Page, string Content)> GetPages(Layouts layout)
    {
        lock (Lock)
            return _cache.Values.Where(
                x => x.Page.Layout == layout.ToString());
    }

    /// <summary>
    /// Page the list.
    /// </summary>
    /// <param name="tpl">The tpl.</param>
    /// <returns>A <see cref="PageModel? "/></returns>
    public (PageModel Page, string Content)? PageList()
    {
        lock (Lock)
            return GetPages(Layouts.List)
                .FirstOrDefault();
    }

    /// <summary>
    /// Pages the detail.
    /// </summary>
    /// <param name="tpl">The tpl.</param>
    /// <returns>A <see cref="PageModel? "/></returns>
    public (PageModel Page, string Content)? PageDetail()
    {
        lock (Lock)
            return GetPages(Layouts.Detail)
                .FirstOrDefault();
    }

}
