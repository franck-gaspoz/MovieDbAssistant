using System.Text.Json;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MovieDbAssistant.Dmn.Components.Builders.Document;
using MovieDbAssistant.Dmn.Configuration;
using MovieDbAssistant.Dmn.Models.Build;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Extensions;

using static MovieDbAssistant.Dmn.Globals;

namespace MovieDbAssistant.Dmn.Components.Builders.Templates;

/// <summary>
/// template builder context
/// </summary>
[Transient]
public sealed class TemplateBuilderContext
{
    readonly IConfiguration _config;
    readonly ILogger<TemplateModel> _logger;
    readonly IOptions<DmnSettings> _dmnSettings;

    /// <summary>
    /// template path
    /// </summary>
    public string TplPath =>
        Path.Combine(
            RscPath,
            _dmnSettings.Value.Paths.RscHtmlTemplates,
            TemplateId!);

    /// <summary>
    /// Gets the rsc path.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string RscPath =>
        Path.Combine(
            DocContext!.RscPath,
            _dmnSettings.Value.Paths.RscHtml)
                .NormalizePath();

    /// <summary>
    /// Gets the tpl file.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string TplFile =>
        Path.Combine(
            TplPath,
            _dmnSettings.Value.Build.Html.TemplateFilename);

    string? _tplContent;
    /// <summary>
    /// Gets the tpl content.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string TplContent => _tplContent ?? (_tplContent =
        File.ReadAllText(TplFile)!);

    /// <summary>
    /// assets path
    /// </summary>
    /// <param name="context">doc builder context</param>
    /// <returns>assets path</returns>
    public string AssetsPath(DocumentBuilderContext context) =>
        Path.Combine(
            context.RscPath,
            _dmnSettings.Value.Paths.RscHtml,
            _dmnSettings.Value.Paths.RscHtmlAssets)
                .NormalizePath();

    /// <summary>
    /// doc builder context
    /// </summary>
    public DocumentBuilderContext? DocContext { get; set; }

    /// <summary>
    /// Gets or sets the template id.
    /// </summary>
    /// <value>A <see cref="string? "/></value>
    public string? TemplateId { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TemplateBuilderContext"/> class.
    /// </summary>
    /// <param name="config">The config.</param>
    /// <param name="logger">The logger.</param>
    public TemplateBuilderContext(
        IConfiguration config,
        ILogger<TemplateModel> logger,
        IOptions<DmnSettings> dmnSettings)
    {
        _config = config;
        _logger = logger;
        _dmnSettings = dmnSettings;
    }

    /// <summary>
    /// attach the context to the document builder context and set the template id
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="templateId">The template id.</param>
    /// <returns>A <see cref="TemplateBuilderContext"/></returns>
    public TemplateBuilderContext For(
        DocumentBuilderContext context,
        string templateId)
    {
        DocContext = context;
        TemplateId = templateId;
        return this;
    }

    /// <summary>
    /// Templates the model.
    /// </summary>
    /// <returns>A <see cref="TemplateModel"/></returns>
    public TemplateModel TemplateModel()
        => JsonSerializer.Deserialize<TemplateModel>(
            TplContent,
            JsonSerializerProperties.Value)
                ?? throw new InvalidOperationException("template spec not found: "
                    + TplFile);
}
