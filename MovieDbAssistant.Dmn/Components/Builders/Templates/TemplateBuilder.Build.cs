using MovieDbAssistant.Dmn.Components.Builders.Html;
using MovieDbAssistant.Dmn.Components.Builders.Templates.PageBuilders;
using MovieDbAssistant.Dmn.Models.Build;
using MovieDbAssistant.Dmn.Models.Extensions;
using MovieDbAssistant.Dmn.Models.Scrap.Json;
using MovieDbAssistant.Lib.Extensions;

namespace MovieDbAssistant.Dmn.Components.Builders.Templates;

/// <summary>
/// The template builder.
/// </summary>
public sealed partial class TemplateBuilder
{
    /// <summary>
    /// build a page list
    /// </summary>
    /// <param name="htmlContext">html document builder context</param>
    /// <param name="data">The data.</param>
    /// <returns>A <see cref="TemplateBuilder"/></returns>
    public TemplateBuilder BuildPageList(
        HtmlDocumentBuilderContext htmlContext,
        MoviesModel data)
    {
        var build = new BuildModel(Layouts.List);
        var docContext = Context.DocContext;

        data.SetupModel(_dmnSettings);

        ExportData(data);

        var page = IncludeParts(_templatesSourceCache.PageList()?.Content!);

        (page, var props, var nprops) = SetVars(build, page, htmlContext);
        page = IntegratesProps(build, page, htmlContext, null);

        Context.DocContext!.AddOutputFile(
            _tpl!.PageList()!
                .Filename!,
            _dmnSettings.Value.Build.Html.Extension,
            page);

        return this;
    }

    /// <summary>
    /// build a page details
    /// </summary>
    ///<param name="context">doc builder context</param>
    /// <param name="htmlContext">html document builder context</param>
    /// <param name="data">The data.</param>
    /// <returns>A <see cref="TemplateBuilder"/></returns>
    public TemplateBuilder BuildPageDetail(
        HtmlDocumentBuilderContext htmlContext,
        MovieModel data)
    {
        var build = new BuildModel(Layouts.Detail);
        var docContext = Context.DocContext!;

        var page = IncludeParts(_templatesSourceCache.PageDetail()?.Content!);

        page = IntegratesData(page, data);
        (page, _, _) = SetVars(build, page, htmlContext, data);

        page = IntegratesProps(build, page, htmlContext, data);
        (page, _) = SetVars(page,
            GetTemplateProps(build, data, htmlContext));

        (page, _) = SetVars(page, data.GetProperties());

        Context.DocContext!.AddOutputFile(
            Path
                .Combine(
                    docContext.PagesFolderName,
                    data.Filename!),
            "",
            page);

        return this;
    }

    /// <summary>
    /// must Handle template.
    /// </summary>
    /// <param name="filePath">The file path.</param>
    /// <returns>A <see cref="bool"/></returns>
    public bool IsHandlableRscTemplateFile(string filePath, out string? ext)
    {
        ext = _tpl!.Paths
            .HandleExtensions
            .Where(x => filePath.EndsWith(x))
            .FirstOrDefault();
        return ext != null;
    }

    /// <summary>
    /// try to handle a resource template file if is handlable
    /// </summary>
    /// <param name="filePath">file pathh</param>
    /// <param name="targetFilePath">target file path</param>
    /// <returns>true if handled, else false</returns>
    public bool TryHandleRscTemplateFile(
        string filePath,
        string targetFilePath)
    {
        if (!IsHandlableRscTemplateFile(filePath, out var ext)) return false;

        BuildFile(filePath, targetFilePath);

        return true;
    }

    /// <summary>
    /// build a file
    /// </summary>
    /// <param name="filePath">src file path</param>
    /// <param name="targetFilePath">target file path</param>
    public TemplateBuilder BuildFile(
        string filePath,
        string targetFilePath)
    {
        var docContext = Context.DocContext!;
        var text = File.ReadAllText(filePath);
        var build = new BuildModel(Layouts.None);

        var page = IncludeParts(text);
        (page, _) = SetVars(
            page,
            GetTemplateProps(build, null, null));

        Context.DocContext!.AddOutputFile(
            RemoveTplPostfix(targetFilePath),
            page);

        return this;
    }
}
