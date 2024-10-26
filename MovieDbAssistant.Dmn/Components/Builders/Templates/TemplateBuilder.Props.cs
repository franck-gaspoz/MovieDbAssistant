using System.Reflection;

using MovieDbAssistant.Dmn.Components.Builders.Html;
using MovieDbAssistant.Dmn.Models.Scrap.Json;
using MovieDbAssistant.Dmn.Models.Extensions;
using MovieDbAssistant.Dmn.Models.Interface;
using MovieDbAssistant.Dmn.Models.Build;

namespace MovieDbAssistant.Dmn.Components.Builders.Templates;

/// <summary>
/// The template builder.
/// </summary>
public sealed partial class TemplateBuilder
{
    const string Template_Var_Tpl = "tpl";
    const string Template_Var_Page = "page";

    const string Template_Var_App = "app";

    const string Template_Var_BuiltAt = "builtAt";
    const string Template_Var_Lang = "lang";

    const string Template_Var_Background = "background";
    const string Template_Var_BackgroundIdle = "backgroundIdle";

    const string Template_Var_Page_Title_Details = "pageTitleDetails";
    const string Template_Var_Title_List = "titleList";
    const string Template_Var_SubTitle_List = "subTitleList";
    const string Template_Var_Page_Title_List = "pageTitleList";
    const string Template_Var_Template_Id = "templateId";
    const string Template_Var_Template_Version = "templateVersion";
    const string Template_Var_Template_VersionDate = "templateVersionDate";

    const string Template_Var_Output = "output";

    const string Template_Var_Navigation = "navigation";

    const string Template_Var_BasePath = "basePath";

    Dictionary<string, object?> GetTemplateProps(
        bool pageDetails,
        MovieModel? data = null,
        HtmlDocumentBuilderContext? htmlContext = null) => new()
        {
            {
                Template_Var_Tpl,
                _tpl
            },
            {
                Template_Var_Page,
                !pageDetails?
                    _tpl!.PageList()!
                    :_tpl!.PageDetail()
            },
            {
                Template_Var_Output,
                new OutputModel(
                    _dmnSettings.Value.Build.Html.Extension,
                    _dmnSettings.Value.Paths.OutputPages
                    )    
            },
            {
                Template_Var_Background ,
                !pageDetails?
                    _tpl!.PageList()!.Background
                    : (data==null || data.PicFullUrl == null)?
                        _tpl!.PageList()!.Background
                        : data.PicFullUrl
            },
            {
                Template_Var_BackgroundIdle,
                _tpl!.PageDetail()!.BackgroundIdle
            },
            {
                Template_Var_Navigation,
                new MovieListNavigationModel(
                    htmlContext?.HomeLink ?? string.Empty,
                    (htmlContext?.Index ?? -1)+1,
                    htmlContext?.NextLink,
                    htmlContext?.PreviousLink,
                    htmlContext?.Total ?? 0
                )
            },
            {
                Template_Var_Title_List,
                _tpl!.PageList()!.Title
            },
            {
                Template_Var_Page_Title_List,
                _tpl.PageList()!.PageTitle
            },
            {
                Template_Var_Page_Title_Details,
                _tpl.PageDetail()!.PageTitle
            },
            {
                Template_Var_Template_Id,
                _tpl.Id
            },
            {
                Template_Var_Template_Version,
                _tpl.Version
            },
            {
                Template_Var_Template_VersionDate,
                _tpl.VersionDate
            },
            {
                Template_Var_App,
                new AppModel(
                    _dmnSettings.Value.App.Title,
                    Assembly.GetExecutingAssembly()
                        .GetName()
                        .Name!
                        .Split('.')[0],
                    Assembly.GetExecutingAssembly()
                        .GetName()
                        .Version!
                        .ToString(),
                    _dmnSettings.Value.App.VersionDate
                    )                
            },
            {
                Template_Var_BuiltAt,
                DateTime.Now.ToString()
            },
            {
                Template_Var_Lang,
                _dmnSettings.Value.App.Lang
            },            
            {
                Template_Var_SubTitle_List,
                htmlContext?.SubTitle
            },
            {
                Template_Var_BasePath,
                pageDetails ? "../" : "./"
            }
        };
}
