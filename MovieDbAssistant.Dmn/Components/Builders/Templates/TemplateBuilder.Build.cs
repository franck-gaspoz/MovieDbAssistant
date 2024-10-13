using MovieDbAssistant.Dmn.Components.Builders.Html;
using MovieDbAssistant.Dmn.Models.Extensions;
using MovieDbAssistant.Dmn.Models.Scrap.Json;
using MovieDbAssistant.Lib.Components.Extensions;

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
        var docContext = Context.DocContext;

        data.SetupModel(_dmnSettings);

        ExportData(data);
        var page = _tpl!.Templates.TplList!;
        (page, var props) = SetVars(page, htmlContext);
        page = IntegratesProps(page, htmlContext);

        Context.DocContext!.AddOutputFile(
            _tpl.Options.PageList.Filename!,
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
        var docContext = Context.DocContext!;

        var page = IntegratesData(
            _tpl!.Templates.TplDetails!,
            data);
        (page, _) = SetVars(page, htmlContext, data);

        page = IntegratesProps(page, htmlContext, data);
        page = SetVars(page,
            GetTemplateProps(true, data, htmlContext));

        page = SetVars(page, data.GetProperties());

        Context.DocContext!.AddOutputFile(
            Path
                .Combine(
                    docContext.PagesFolderName,
                    data.Filename!),
            "",
            page);

        return this;
    }
}
