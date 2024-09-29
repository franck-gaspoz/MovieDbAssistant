using System.Collections.Concurrent;
using System.Text.Json;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using MovieDbAssistant.Dmn.Models.Build;
using MovieDbAssistant.Dmn.Models.Scrap.Json;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Extensions;
using MovieDbAssistant.Lib.Components.Logger;

using static MovieDbAssistant.Dmn.Components.Settings;
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

    /// <summary>
    /// Gets or sets the context.
    /// </summary>
    /// <value>A <see cref="TemplateBuilderContext"/></value>
    public TemplateBuilderContext Context { get; set; }

    TemplateModel? _tpl;

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
    /// <param name="data">The data.</param>
    /// <returns>A <see cref="TemplateBuilder"/></returns>
    public TemplateBuilder BuildPageList(MoviesModel data)
    {
        var docContext = Context.DocContext;

        ExportData(data);
        var page = _tpl!.Templates.TplList!;
        SetVars(page, data.GetProperties());

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
    /// <param name="data">The data.</param>
    /// <returns>A <see cref="TemplateBuilder"/></returns>
    public TemplateBuilder BuildPageDetail(MovieModel data)
    {
        var docContext = Context.DocContext!;

        var page = IntegratesData(
            _tpl!.Templates.TplDetails!,
            data);
        SetVars(page, data.GetProperties());

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
        var src = JsonSerializer.Serialize(
            data,
            JsonSerializerProperties.Value)!;

        tpl = SetVar(tpl, Var_Data, src);
        return tpl;
    }

    string SetVars(string tpl,Dictionary<string,object?> vars)
    {
        foreach ( var kvp in vars )
            tpl = SetVar(
                tpl,
                KeyToVar(kvp.Key),
                VarToString(kvp.Value));
        return tpl;
    }

    static string KeyToVar(string key) => key.ToLower();

    static string VarToString(object? value)
    {
        return value?.ToString() ?? string.Empty;
    }

    string SetVar(string text, string name, string value)
    {
        text = text.Replace(Var(name), value);
        return text;
    }

    string Var(string name) => "{{"+name+"}}";
}
