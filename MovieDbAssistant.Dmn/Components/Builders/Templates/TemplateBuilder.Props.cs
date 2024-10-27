using System.Reflection;

using MovieDbAssistant.Dmn.Components.Builders.Html;
using MovieDbAssistant.Dmn.Components.Builders.Templates.PageBuilders;
using MovieDbAssistant.Dmn.Models.Build;
using MovieDbAssistant.Dmn.Models.Extensions;
using MovieDbAssistant.Dmn.Models.Interface;
using MovieDbAssistant.Dmn.Models.Scrap.Json;

namespace MovieDbAssistant.Dmn.Components.Builders.Templates;

/// <summary>
/// The template builder.
/// </summary>
public sealed partial class TemplateBuilder
{
    const string Template_Var_Tpl = "tpl";
    
    const string Template_Var_Page = "page";

    const string Template_Var_App = "app";

    const string Template_Var_Build = "build";

    const string Template_Var_Output = "output";

    const string Template_Var_Navigation = "navigation";

    const string Template_Var_BasePath = "basePath";

    const string Template_Var_Vars = "vars";

    Dictionary<string, object?> GetTemplateProps(
        BuildModel build,
        MovieModel? data = null,
        HtmlDocumentBuilderContext? htmlContext = null)
    {
        build.FinishedAt = DateTime.UtcNow;
        var pageDetail = build.Layout == Layouts.Detail;

        // TODO: to be done by the builder layout = Detail
        if (pageDetail)
            _tpl!.PageDetail()!
                .Background =
                    (data == null || data.PicFullUrl == null) ?
                        _tpl!.PageList()!.Background
                        : data.PicFullUrl;

        // TODO: to be done by builder layout = List & Detail
        _tpl!.PageDetail()!
            .SubTitle = htmlContext?.SubTitle;
        _tpl!.PageList()!
            .SubTitle = htmlContext?.SubTitle;

        var props = new Dictionary<string, object?>()
        {
            {
                Template_Var_Tpl,
                _tpl
            },
            {
                Template_Var_Page,
                GetPropsPageModel(pageDetail)
            },
            {
                Template_Var_Output,
                GetPropsOutputModel()
            },
            {
                Template_Var_Build,
                build
            },
            {
                Template_Var_Navigation,
                GetPropsNavigationModel(htmlContext)
            },
            {
                Template_Var_App,
                GetPropsApplicationModel()
            },
            {
                Template_Var_BasePath,
                GetPropsBasePath(pageDetail)
            },
            {
                Template_Var_Vars,
                GetPropsVarsModel()
            }
        };
        return props;
    }

    VarsModel GetPropsVarsModel()
    {
        return new VarsModel();
    }

    PageModel? GetPropsPageModel(bool pageDetail) 
        => !pageDetail ?
            _tpl!.PageList()!
            : _tpl!.PageDetail();

    static string GetPropsBasePath(bool pageDetail) 
        => pageDetail ? "../" : "./";

    OutputModel GetPropsOutputModel() => new(
        _dmnSettings.Value.Build.Html.Extension,
        _dmnSettings.Value.Paths.OutputPages
        );

    static MovieListNavigationModel GetPropsNavigationModel(
        HtmlDocumentBuilderContext? htmlContext) => new(
            htmlContext?.HomeLink ?? string.Empty,
            (htmlContext?.Index ?? -1) + 1,
            htmlContext?.NextLink,
            htmlContext?.PreviousLink,
            htmlContext?.Total ?? 0
        );

    AppModel GetPropsApplicationModel() => new(
        _dmnSettings.Value.App.Title,
        Assembly.GetExecutingAssembly()
            .GetName()
            .Name!
            .Split('.')[0],
        Assembly.GetExecutingAssembly()
            .GetName()
            .Version!
            .ToString(),
        _dmnSettings.Value.App.VersionDate,
        _dmnSettings.Value.App.Lang
        );
}
