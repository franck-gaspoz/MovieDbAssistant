using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Extensions;

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
    string? _templateId;
    public const string ExtHtml = ".html";

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

        var path = RscPath(context);
        //var data = 
    }

    string RscPath(DocumentBuilderContext context) =>
        Path.Combine(
            context.RscPath,
            _config[Path_RscHtml]!)
                .NormalizePath();
}
