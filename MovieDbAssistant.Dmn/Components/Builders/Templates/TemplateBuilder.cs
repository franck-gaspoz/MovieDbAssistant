#define TEST_SOURCE

using System.Collections;

using System.Collections.Concurrent;
using System.Reflection;
using System.Text.Json;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MovieDbAssistant.Dmn.Components.Builders.Html;
using MovieDbAssistant.Dmn.Configuration;
using MovieDbAssistant.Dmn.Models.Build;
using MovieDbAssistant.Dmn.Models.Extensions;
using MovieDbAssistant.Dmn.Models.Scrap.Json;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Extensions;
using MovieDbAssistant.Lib.Components.Logger;

using static MovieDbAssistant.Dmn.Globals;

namespace MovieDbAssistant.Dmn.Components.Builders.Templates;

/// <summary>
/// template document
/// </summary>
[Transient]
public sealed class TemplateBuilder
{
    readonly IConfiguration _config;
    readonly ILogger<TemplateBuilder> _logger;
    readonly IOptions<DmnSettings> _dmnSettings;

    const string Var_Data = "data";
    const string Var_Props = "props";

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

    /// <summary>
    /// Gets or sets the context.
    /// </summary>
    /// <value>A <see cref="TemplateBuilderContext"/></value>
    public TemplateBuilderContext Context { get; set; }

    TemplateModel? _tpl;

    /// <summary>
    /// Gets the template model.
    /// </summary>
    /// <value>A <see cref="TemplateModel"/></value>
    public TemplateModel TemplateModel => _tpl!;

    static readonly ConcurrentDictionary<string, TemplateModel> _templates = [];

    public TemplateBuilder(
        IConfiguration configuration,
        ILogger<TemplateBuilder> logger,
        TemplateBuilderContext context,
        IOptions<DmnSettings> dmnSettings)
    {
        _config = configuration;
        _logger = logger;
        Context = context;
        _dmnSettings = dmnSettings;
    }

    /// <summary>
    /// Load the template or get from cache if already loaded
    /// </summary>
    /// <param name="docContext">biulder context</param>
    /// <param name="templateId">The template id.</param>
    public TemplateBuilder LoadTemplate(
        DocumentBuilderContext docContext,
        string templateId)
    {
        Context.For(
            docContext,
            templateId);

        if (_templates.TryGetValue(templateId, out var tpl))
        {
            _tpl = tpl;
            return this;
        }

        tpl = _tpl = Context.TemplateModel();

        tpl.LoadContent(Context.TplPath);
        _templates.TryAdd(tpl.Id, tpl);

        _logger.LogInformation(this, $"template '{tpl.Name}' loaded");

        return this;
    }

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
#if true || TEST_SOURCE
        foreach (var movie in data.Movies)
        {
            movie.Sources.Download = _tpl!.Options.HelpLink+"#"+movie.Key;
            movie.Sources.Play = _tpl!.Options.HelpLink + "#" + movie.Key;
        }
#endif
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

    /// <summary>
    /// Copy tpl rsc.
    /// </summary>
    /// <returns>A <see cref="TemplateBuilder"/></returns>
    public TemplateBuilder CopyTplRsc()
    {
        foreach (var item in _tpl!.Files)
            CopyTemplateRsc(item);
        return this;
    }

    /// <summary>
    /// Copy the rsc.
    /// </summary>
    /// <returns>A <see cref="TemplateBuilder"/></returns>
    public TemplateBuilder CopyRsc()
    {
        foreach (var item in _tpl!.Resources)
            CopyRsc(item);
        return this;
    }

    void CopyRsc(string item)
    {
        var target = Context.DocContext!.OutputFolder!;
        var t = item.Split(':');

        var src = Context.AssetsPath(Context.DocContext!);
        src = Path.Combine(src, t[0][1..]);
        target = Path.Combine(
            target, 
            t[1][1..]);       

        if (!Directory.Exists(target))
            Directory.CreateDirectory(target);

        target = Path.Combine(target,
            Path.GetFileName(src));

        if (File.Exists(src))
        {
            File.Copy(
                src,
                target,
                true);

            _logger.LogInformation(this,"file copied: " + src + " to " + target);
        }
    }

    void CopyTemplateRsc(string item)
    {
        var src = Path.Combine(Context.TplPath, item[1..]);
        var target = Context.DocContext!.OutputFolder!;

        if (!item.StartsWith('/'))
        {
            target = Path.Combine(target, item[1..]);
            var tgtFolder = Path.GetDirectoryName(target)!;
            if (!Directory.Exists(tgtFolder))
                Directory.CreateDirectory(tgtFolder);

            if (File.Exists(src))
                File.Copy(
                    src,
                    target);

            _logger.LogInformation(this, "file copied: " + src + " to " + target);
        }
        else
        {
            if (Directory.Exists(src))
            {
                target = Path.Combine(
                        target,
                        Path.GetFileName(src));
                src.CopyDirectory( target);

                _logger.LogInformation(this, "folder copied: " + src + " to " + target);
            }
        }
    }

    void ExportData(MoviesModel data)
    {
        var src = JsonSerializer.Serialize(
            data,
            JsonSerializerProperties.Value)!;

        src = $"const data = {src};";

        Context.DocContext!.AddOutputFile(
            _dmnSettings.Value.Build.Html.DataFilename,
            src);
    }

    string IntegratesData(
        string tpl,
        MovieModel data)
    {
#if TEST_SOURCE
        data.Sources.Download = _tpl!.Options.HelpLink+"#"+data.Key;
        data.Sources.Play = _tpl!.Options.HelpLink + "#" + data.Key;
#endif

        var src = JsonSerializer.Serialize(
            data,
            JsonSerializerProperties.Value)!;

        tpl = SetVar(tpl, Var_Data, src);
        return tpl;
    }

    string IntegratesProps(
        string tpl,
        HtmlDocumentBuilderContext htmlContext,
        MovieModel? data = null)
    {
        var src = JsonSerializer.Serialize(
            GetTemplateProps(true, data, htmlContext),
            JsonSerializerProperties.Value)!;

        tpl = SetVar(tpl, Var_Props, src);
        return tpl;
    }

    Dictionary<string, object?> GetTemplateProps(
        bool pageDetails,
        MovieModel? data = null,
        HtmlDocumentBuilderContext? htmlContext = null) => new()
        {
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
                Template_Var_SubTitle_List,
                htmlContext?.SubTitle
            }
        };

    string SetVars(string tpl)
    {
        tpl = SetVars(tpl, GetTemplateProps(false, null, null));
        return tpl;
    }

    (string, Dictionary<string, object?>) SetVars(string tpl, HtmlDocumentBuilderContext htmlContext)
    {
        var props = GetTemplateProps(false, null, htmlContext);
        tpl = SetVars(tpl, props);
        return (tpl, props);
    }

    (string, Dictionary<string, object?>) SetVars(
        string tpl,
        HtmlDocumentBuilderContext htmlContext,
        MovieModel data)
    {
        var props = GetTemplateProps(true, data, htmlContext);
        tpl = SetVars(tpl, props);
        return (tpl, props);
    }

    string SetVars(
        string tpl,
        Dictionary<string, object?> vars,
        string? prefix = null)
    {
        foreach (var kvp in vars)
        {
            var val = kvp.Value;
            var varnp = KeyToVar(kvp.Key);

            if (val != null
                && val!.GetType().Namespace!
                    .StartsWith(GetType()
                        .Namespace!
                        .Split('.')[0]))
            {
                // model not null
                SetVars(
                    tpl,
                    val.GetProperties(),
                    prefix != null ?
                        prefix + '.' + varnp
                        : varnp
                        );
            }
            else
            {
                // not a model or null model
                var k = kvp.Key;
                if (prefix != null)
                    k = prefix + '.' + k;

                tpl = SetVar(
                    tpl,
                    KeyToVar(k),
                    VarToString(TransformValue(k, val)));
            }
        }
        return tpl;
    }

    static string KeyToVar(string key) =>
        key.ToFirstLower();
    //.Replace('.', '-');

    object? TransformValue(string? key, object? value)
    {
        if (key == null) return null;
        if (value == null) return null;
        var transforms = _tpl!.Transforms;
        var transform = transforms
            .FirstOrDefault(x => x.Target == key)
            ?? transforms
                .FirstOrDefault(x => x.GetType().Name == key);
        if (transform == null) return value;
        var tm = GetType()
            .GetMethod(transform.Operation);
        if (tm == null)
            throw new InvalidOperationException("value transformer method not found: " + transform.Operation);
        value = tm!.Invoke(this, [value]);
        return value;
    }

    /// <summary>
    /// Transform actor simple.
    /// </summary>
    /// <param name="o">The object</param>
    /// <returns>An <see cref="object? "/></returns>
    public object? Transform_ActorSimple(object? o)
    {
        if (o == null) return null;
        if (o is ActorModel actor)
        {
            o = actor.Actor;
        }
        return o;
    }

    /// <summary>
    /// Transform the array.
    /// </summary>
    /// <param name="o">The object</param>
    /// <returns>An <see cref="object? "/></returns>
    public object? Transform_Array(object? o)
    {
        if (o == null) return null;
        if (o is IEnumerable list)
        {
            var t = list.Cast<object?>()
                .Select(x => TransformValue(
                    o?.GetType()?.GetGenericArguments()[0]?.Name,
                    x));
            o = string.Join(
                _tpl!.HSep,
                t);
        }
        return o;
    }

    static string VarToString(object? value) => value?.ToString() ?? string.Empty;

    string SetVar(string text, string name, string value)
    {
        text = text.Replace(Var(name), value);
        return text;
    }

    string Var(string name) => "{{" + name + "}}";
}
