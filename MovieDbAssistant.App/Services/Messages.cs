using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using MovieDbAssistant.Dmn.Configuration;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;

using static MovieDbAssistant.Dmn.Components.Settings;

namespace MovieDbAssistant.App.Services;

[Singleton]
sealed class Messages
{
    readonly IConfiguration _config;

    readonly DmnSettings _dmnSettings;

    public Messages(IConfiguration config,
        IOptions<DmnSettings> dmnSettings)
        => (_config, _dmnSettings) = (config, dmnSettings.Value);

    /// <summary>
    /// warning alert box
    /// </summary>
    /// <param name="key">message key</param>
    /// <param name="text">additional text</param>
    public void Warn(string key, string? text = null)
        => MessageBox.Show(
            _config[key]! + text ?? "",
            Caption(Message_Warning),
            MessageBoxButtons.OK,
            MessageBoxIcon.Warning);

    /// <summary>
    /// info alert box
    /// </summary>
    /// <param name="key">message key</param>
    /// <param name="text">additional text</param>
    public void Info(string key, string? text = null)
        => MessageBox.Show(
            _config[key]! + text ?? "",
            Caption(Message_Info),
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);

    /// <summary>
    /// error alert box
    /// </summary>
    /// <param name="errorTypeKey">error type</param>
    /// <param name="errorMessage">error message</param>
    public void Err(string errorTypeKey, string errorMessage)
        => MessageBox.Show(
            _config[errorTypeKey]! + errorMessage,
            Caption(Message_Error),
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);

    string Caption(string postFixKey)
        => _dmnSettings.App.Title! + ": " + _config[postFixKey]!;
}
