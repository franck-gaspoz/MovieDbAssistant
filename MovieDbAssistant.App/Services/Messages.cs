using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using MovieDbAssistant.App.Configuration;
using MovieDbAssistant.Dmn.Configuration;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;

using static MovieDbAssistant.Dmn.Components.Settings;

namespace MovieDbAssistant.App.Services;

[Singleton]
sealed class Messages
{
    readonly IConfiguration _config;

    readonly IOptions<DmnSettings> _dmnSettings;
    readonly IOptions<AppSettings> _appSettings;

    public Messages(IConfiguration config,
        IOptions<DmnSettings> dmnSettings,
        IOptions<AppSettings> appSettings)
        => (_config, _dmnSettings, _appSettings) 
            = (config, dmnSettings, appSettings);

    /// <summary>
    /// warning alert box
    /// </summary>
    /// <param name="text">additional text</param>
    public void Warn(string text)
        => MessageBox.Show(
            text,
            Caption(_appSettings.Value.Texts.Warning),
            MessageBoxButtons.OK,
            MessageBoxIcon.Warning);

    /// <summary>
    /// info alert box
    /// </summary>
    /// <param name="text">additional text</param>
    public void Info(string text)
        => MessageBox.Show(
            text,
            Caption(_appSettings.Value.Texts.Info),
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);

    /// <summary>
    /// error alert box
    /// </summary>
    /// <param name="errorTypeKey">error type</param>
    /// <param name="errorMessage">error message</param>
    public void Err(string errorType, string errorMessage)
        => MessageBox.Show(
            errorType + errorMessage,
            Caption(_appSettings.Value.Texts.Error),
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);

    string Caption(string postFixKey)
        => _dmnSettings.Value.App.Title! + ": " + _config[postFixKey]!;
}
