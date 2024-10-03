using System.Collections;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text.Json;
using System.Linq;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using MovieDbAssistant.Dmn.Components.Builders.Html;
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
    const string Template_Var_Page_Title_Details = "pageTitleDetails";
    const string Template_Var_Title_List = "titleList";
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
        TemplateBuilderContext context)
    {
        _config = configuration;
        _logger = logger;
        Context = context;
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

        data.SetupModel(_config);
        ExportData(data);
        var page = _tpl!.Templates.TplList!;
        page = SetVars(page);
        page = IntegratesProps(page, htmlContext);

        Context.DocContext!.AddOutputFile(
            _tpl.Options.PageList.Filename!,
            _config[Build_HtmlFileExt]!,
            page);

        CopyRsc();

        return this;
    }

    /// <summary>
    /// build a page details
    /// </summary>
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
        (page,_) = SetVars(page, htmlContext, data);

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

        CopyRsc();

        return this;
    }

    public TemplateBuilder CopyRsc()
    {
        foreach (var item in _tpl!.Files)
            CopyRsc(item);
        return this;
    }

    void CopyRsc(string item)
    {
        var src = Path.Combine(Context.TplPath, item[1..]);
        var target = Context.DocContext!.OutputFolder!;

        if (!item.StartsWith('/'))
        {
            if (File.Exists(src))
                File.Copy(
                    src,
                    Path.Combine(target, item[1..]));
        }
        else
        {
            if (Directory.Exists(src))
            {
                src.CopyDirectory(
                    Path.Combine(
                        target,
                        Path.GetFileName(src)));
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
            _config[Build_Html_Filename_Data]!,
            src);
    }

    string IntegratesData(
        string tpl,
        MovieModel data)
    {
#if true || TEST_SOURCE
        data.Sources.Download = "http://www.asite.com/myvideo/"+data.Key+".mp4";
        data.Sources.Play = "http://www.asite.com/myvideo/"+data.Key+".html";
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
            //htmlContext,
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
                _config[Path_OutputPages]
            },
            {
                Template_Var_Build_Ext_Html,
                _config[Build_HtmlFileExt]
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
                _config[App_Title]
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
                _config[App_VersionDate]
            },
            {
                Template_Var_BuiltAt,
                DateTime.Now.ToString()
            },
            {
                Template_Var_Lang,
                _config[App_Lang]
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
        return (tpl,props);
    }

    (string, Dictionary<string, object?>) SetVars(
        string tpl,         
        HtmlDocumentBuilderContext htmlContext,
        MovieModel data)
    {
        var props = GetTemplateProps(true, data, htmlContext);
        tpl = SetVars(tpl, props);
        return (tpl,props);
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
                    .StartsWith( GetType()
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
                var k = kvp.Key;
                if (prefix != null)
                    k = prefix + '.' + k;

                // not a model or null model
                tpl = SetVar(
                    tpl,
                    KeyToVar(k),
                    VarToString(TransformValue(k,val)));
            }
        }
        return tpl;
    }

    static string KeyToVar(string key) =>
        key.ToFirstLower();
            //.Replace('.', '-');

    object? TransformValue(string? key,object? value)
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
            throw new InvalidOperationException("value transformer method not found: "+transform.Operation);
        value = tm!.Invoke(this,[value]);
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
