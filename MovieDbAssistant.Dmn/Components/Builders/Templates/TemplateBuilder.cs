using System.Collections.Concurrent;
using System.Text.Json;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using MovieDbAssistant.Dmn.Models.Build;
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
    TemplateModel? _tpl;

    static readonly ConcurrentDictionary<string, TemplateModel> _templates = [];

    public TemplateBuilder(
        IConfiguration configuration,
        ILogger<TemplateBuilder> logger)
    {
        _config = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Load the template or get from cache if already loaded
    /// </summary>
    /// <param name="context">biulder context</param>
    /// <param name="templateId">The template id.</param>
    public TemplateBuilder LoadTemplate(
        DocumentBuilderContext context,
        string templateId)
    {
        if (_templates.TryGetValue(templateId, out var tpl))
        {
            _tpl = tpl;
            return this;
        }

        var tplPath = Path.Combine(
            RscPath(context),
            templateId);

        var tplFile = Path.Combine(
            tplPath,
            _config[Build_Html_Template_Filename]!);
        var tplSpec = File.ReadAllText(tplFile);

        tpl = _tpl = JsonSerializer.Deserialize<TemplateModel>(
            tplSpec,
            JsonSerializerProperties.Value)
                ?? throw new InvalidOperationException("template spec not found: " + tplFile);
        
        tpl.LoadContent(tplPath);
        _templates.TryAdd(tpl.Id, tpl);

        _logger.LogInformation(this, $"template '{tpl.Name}' loaded");
        
        return this;

    }

    string RscPath(DocumentBuilderContext context) =>
        Path.Combine(
            context.RscPath,
            _config[Path_RscHtml]!)
                .NormalizePath();

    string AssetsPath(DocumentBuilderContext context) =>
        Path.Combine(
            context.RscPath,
            _config[Path_RscHtml]!,
            _config[Path_RscHtmlAssets]!)
                .NormalizePath();
}
