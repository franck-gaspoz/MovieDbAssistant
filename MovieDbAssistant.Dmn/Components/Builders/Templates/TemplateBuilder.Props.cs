using System.Reflection;

using MovieDbAssistant.Dmn.Components.Builders.Html;
using MovieDbAssistant.Dmn.Models.Scrap.Json;

namespace MovieDbAssistant.Dmn.Components.Builders.Templates;

/// <summary>
/// The template builder.
/// </summary>
public sealed partial class TemplateBuilder
{
    const string Template_Var_Tpl = "tpl";

    const string Template_Var_Software = "software";
    const string Template_Var_Software_Id = "softwareId";
    const string Template_Var_Software_Version = "softwareVersion";
    const string Template_Var_Software_Version_Date = "softwareVersionDate";
    const string Template_Var_BuiltAt = "builtAt";
    const string Template_Var_Lang = "lang";
    const string Template_Var_Link_Repo = "linkRepo";
    const string Template_Var_Link_Help = "linkHelp";
    const string Template_Var_Link_Author = "linkAuthor";

    const string Template_Var_Background = "background";
    const string Template_Var_BackgroundIdle = "backgroundIdle";
    const string Template_Var_ListMoviePicNotAvailable = "listMoviePicNotAvailable";
    const string Template_Var_DetailMoviePicNotAvailable = "detailMoviePicNotAvailable";
    const string Template_Var_ListMoviePicNotFound = "listMoviePicNotFound";
    const string Template_Var_DetailMoviePicNotFound = "detailMoviePicNotFound";

    const string Template_Var_Page_Title_Details = "pageTitleDetails";
    const string Template_Var_Title_List = "titleList";
    const string Template_Var_SubTitle_List = "subTitleList";
    const string Template_Var_Page_Title_List = "pageTitleList";
    const string Template_Var_Template_Id = "templateId";
    const string Template_Var_Template_Version = "templateVersion";
    const string Template_Var_Template_VersionDate = "templateVersionDate";

    const string Template_Var_Prefix_Output = "output.";
    const string Template_Var_OutputPages = Template_Var_Prefix_Output + "pages";
    const string Template_Var_Build_Ext_Html = Template_Var_Prefix_Output + "ext";

    const string Template_Var_Prefix_Movies = "movies.";
    const string Template_Var_Index = Template_Var_Prefix_Movies + "index";
    const string Template_Var_Total = Template_Var_Prefix_Movies + "total";
    const string Template_Var_Link_Home = Template_Var_Prefix_Movies + "home";
    const string Template_Var_Link_Previous = Template_Var_Prefix_Movies + "previous";
    const string Template_Var_Link_Next = Template_Var_Prefix_Movies + "next";

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
                Template_Var_OutputPages,
                _dmnSettings.Value.Paths.OutputPages
            },
            {
                Template_Var_Build_Ext_Html,
                _dmnSettings.Value.Build.Html.Extension
            },
            {
                Template_Var_Background ,
                !pageDetails?
                    _tpl!.Options.PageList.Background
                    : (data==null || data.PicFullUrl == null)?
                        _tpl!.Options.PageList.Background
                        : data.PicFullUrl
            },
            {
                Template_Var_BackgroundIdle,
                _tpl!.Options.PageDetail.BackgroundIdle
            },
            {
                Template_Var_Index,
                htmlContext?.Index+1
            },
            {
                Template_Var_Total,
                htmlContext?.Total
            },
            {
                Template_Var_Link_Home,
                htmlContext?.HomeLink
            },
            {
                Template_Var_Link_Previous,
                htmlContext?.PreviousLink
            },
            {
                Template_Var_Link_Next,
                htmlContext?.NextLink
            },
            {
                Template_Var_Title_List,
                _tpl.Options.PageList.Title
            },
            {
                Template_Var_Page_Title_List,
                _tpl.Options.PageList.PageTitle
            },
            {
                Template_Var_Page_Title_Details,
                _tpl.Options.PageDetail.PageTitle
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
                Template_Var_Software_Id,
                Assembly.GetExecutingAssembly()
                    .GetName()
                    .Name!
                    .Split('.')[0]
            },
            {
                Template_Var_Software,
                _dmnSettings.Value.App.Title
            },
            {
                Template_Var_Software_Version,
                Assembly.GetExecutingAssembly()
                    .GetName()
                    .Version!
                    .ToString()
            },
            {
                Template_Var_Software_Version_Date,
                _dmnSettings.Value.App.VersionDate
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
                Template_Var_Link_Repo,
                _tpl!.Options.RepoLink
            },
            {
                Template_Var_Link_Help,
                _tpl!.Options.HelpLink
            },
            {
                Template_Var_Link_Author,
                _tpl!.Options.AuthorLink
            },
            {
                Template_Var_ListMoviePicNotAvailable,
                _tpl!.Options.ListMoviePicNotAvailable
            },
            {
                Template_Var_DetailMoviePicNotAvailable,
                _tpl!.Options.DetailMoviePicNotAvailable
            },
            {
                Template_Var_ListMoviePicNotFound,
                _tpl!.Options.ListMoviePicNotFound
            },
            {
                Template_Var_DetailMoviePicNotFound,
                _tpl!.Options.DetailMoviePicNotFound
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
