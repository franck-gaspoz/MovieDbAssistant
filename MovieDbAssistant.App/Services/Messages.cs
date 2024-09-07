using Microsoft.Extensions.Configuration;

using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;

using static MovieDbAssistant.Dmn.Components.Settings;

namespace MovieDbAssistant.App.Services;

[Singleton]
sealed class Messages
{
    readonly IConfiguration _config;

    public Messages(IConfiguration config)
        => _config = config;

    /// <summary>
    /// warning alert box
    /// </summary>
    /// <param name="key">message key</param>
    public void Warn(string key)
        => MessageBox.Show(
            _config[key]!,
            Caption(Message_Error),
            MessageBoxButtons.OK,
            MessageBoxIcon.Warning);

    /// <summary>
    /// error alert box
    /// </summary>
    /// <param name="errorTypeKey">error type</param>
    /// <param name="errorMessage">error message</param>
    public void Err(string errorTypeKey,string errorMessage)
        => MessageBox.Show(
            _config[errorTypeKey]! + errorMessage,
            Caption(Message_Error),
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);

    string Caption(string postFixKey)
        => _config[AppTitle]! + ": " + _config[postFixKey]!;
}
