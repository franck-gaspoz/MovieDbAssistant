using System.Text.Json;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MovieDbAssistant.Dmn.Components.Builders.Document;
using MovieDbAssistant.Dmn.Configuration;
using MovieDbAssistant.Dmn.Models.Build;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Extensions;

using static MovieDbAssistant.Dmn.Globals;

namespace MovieDbAssistant.Dmn.Components.Builders.Templates;

/// <summary>
/// template builder context
/// </summary>
[Transient]
public sealed class TemplateBuilderContext
{
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
    /// Gets or sets the template version.
    /// </summary>
    /// <value>A <see cref="string? "/></value>
    public string? TemplateVersion { get; set; }

    private const string Separator_Id_Version = "-";
    readonly IConfiguration _config;
    readonly ILogger<TemplateBuilderContext> _logger;
    readonly IOptions<DmnSettings> _dmnSettings;

    /// <summary>
    /// template path
    /// </summary>
    public string TplPath =>
        Path.Combine(
            RscPath,
            _dmnSettings.Value.Paths.RscHtmlTemplates,
            TemplateId!
                + Separator_Id_Version
                + TemplateVersion!);

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

    TemplateModel? _tplModel;

    /// <summary>
    /// Template to model.
    /// </summary>
    /// <returns>A <see cref="TemplateModel"/></returns>
    public TemplateModel TemplateModel
        => _tplModel ?? (_tplModel =
            JsonSerializer.Deserialize<TemplateModel>(
                TplContent,
                JsonSerializerProperties.Value)
                    ?? throw new InvalidOperationException("template spec not found: "
                        + TplFile));

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
    /// Initializes a new instance of the <see cref="TemplateBuilderContext"/> class.
    /// </summary>
    /// <param name="config">The config.</param>
    /// <param name="logger">The logger.</param>
    public TemplateBuilderContext(
        IConfiguration config,
        ILogger<TemplateBuilderContext> logger,
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
    /// <param name="templateVersion">tpl version</param>
    /// <returns>A <see cref="TemplateBuilderContext"/></returns>
    public TemplateBuilderContext For(
        DocumentBuilderContext context,
        string templateId,
        string templateVersion)
    {
        DocContext = context;
        TemplateId = templateId;
        TemplateVersion = templateVersion;
        return this;
    }

}
