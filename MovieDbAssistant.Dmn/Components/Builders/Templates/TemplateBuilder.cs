﻿using System.Collections.Concurrent;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using MovieDbAssistant.Dmn.Models.Build;
using MovieDbAssistant.Dmn.Models.Scrap.Json;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Extensions;
using MovieDbAssistant.Lib.Components.Logger;

using static MovieDbAssistant.Dmn.Components.Settings;

namespace MovieDbAssistant.Dmn.Components.Builders.Templates;

/// <summary>
/// template document
/// </summary>
[Transient]
public sealed class TemplateBuilder
{
    readonly IConfiguration _config;
    readonly ILogger<TemplateBuilder> _logger;

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
        if (_templates.TryGetValue(templateId, out var tpl))
        {
            _tpl = tpl;
            return this;
        }

        Context.For(
            docContext,
            templateId);

        tpl = _tpl = Context.TemplateModel();

        tpl.LoadContent(Context.TplPath);
        _templates.TryAdd(tpl.Id, tpl);

        _logger.LogInformation(this, $"template '{tpl.Name}' loaded");

        return this;
    }

    /// <summary>
    /// build the template file(s)
    /// </summary>
    /// <param name="data">The data.</param>
    /// <returns>A <see cref="TemplateBuilder"/></returns>
    public TemplateBuilder Build(MovieModel data)
    {
        var docContext = Context.DocContext;

        var listContent = ProcessTemplateList(
            _tpl!.Templates.TplList!,
            _tpl.Templates.TplItem!,
            data);

        Context.DocContext!.AddOutputFile(
            _tpl.Options.PageList.Filename,
            _config[Build_HtmlFileExt]!,
            listContent);



        return this;
    }

    string ProcessTemplateList(
        string pageListTemplate,
        string itemTemplate,
        MovieModel data)
    {
        return pageListTemplate;
    }

    string ProcessTemplate(
        string template,
        MovieModel data)
    {
        return template;
    }
}
