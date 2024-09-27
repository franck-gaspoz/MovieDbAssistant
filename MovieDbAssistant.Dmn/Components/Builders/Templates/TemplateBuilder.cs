using System.Text.Json;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using MovieDbAssistant.Dmn.Models.Build;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Extensions;

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
    string? _templateId;

    public TemplateBuilder(
        IConfiguration configuration,
        ILogger<TemplateBuilder> logger)
    {
        _config = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Load the template.
    /// </summary>
    /// <param name="context">biulder context</param>
    /// <param name="templateId">The template id.</param>
    public void LoadTemplate(
        DocumentBuilderContext context,
        string templateId)
    {
        _templateId = templateId;

        var path = Path.Combine(
            RscPath(context),
            templateId);

        var tplFile = Path.Combine(
            path,
            _config[Build_Html_Template_Filename]!);
        var tpl = File.ReadAllText(tplFile);

        var data = JsonSerializer.Deserialize<TemplateModel>(
            tpl,
            JsonSerializerProperties.Value);
    }

    string RscPath(DocumentBuilderContext context) =>
        Path.Combine(
            context.RscPath,
            _config[Path_RscHtml]!)
                .NormalizePath();
}
