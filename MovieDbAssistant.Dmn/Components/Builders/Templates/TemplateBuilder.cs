using System.Collections.Concurrent;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MovieDbAssistant.Dmn.Components.Builders.Document;
using MovieDbAssistant.Dmn.Configuration;
using MovieDbAssistant.Dmn.Models.Build;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Logger;

namespace MovieDbAssistant.Dmn.Components.Builders.Templates;

/// <summary>
/// template document
/// </summary>
[Transient]
public sealed partial class TemplateBuilder
{
    readonly IConfiguration _config;
    readonly ILogger<TemplateBuilder> _logger;
    readonly IOptions<DmnSettings> _dmnSettings;

    const string Var_Data = "data";
    const string Var_Props = "props";

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

        tpl = _tpl = Context.TemplateModel;

        tpl.LoadContent(Path.Combine(
            Context.TplPath,
            _tpl!.Paths.Pages));

        _templates.TryAdd(tpl.Id, tpl);

        _logger.LogInformation(this, $"template '{tpl.Name}' loaded");

        return this;
    }

}
