using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using MovieDbAssistant.Dmn.Configuration;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Extensions;

namespace MovieDbAssistant.Dmn.Components;

/// <summary>
/// The settings.
/// </summary>
[Singleton]
public sealed class Settings
{
    #region consts

    /// <summary>
    /// The url help git hub.
    /// </summary>
    public const string Url_HelpGitHub = "Urls:HelpGitHub";

    /// <summary>
    /// Search pattern json.
    /// </summary>
    public const string SearchPattern_Json = "Build:SearchPatternJson";

    /// <summary>
    /// Search pattern txt.
    /// </summary>
    public const string SearchPattern_Txt = "Build:SearchPatternTxt";

    /// <summary>
    /// The prefix file disabled.
    /// </summary>
    public const string PrefixFileDisabled = "Build:PrefixFileDisabled";
    
    #endregion

    readonly IConfiguration _config;
    readonly IOptions<DmnSettings> _dmnSettings;

    /// <summary>
    /// Initializes a new instance of the <see cref="Settings"/> class.
    /// </summary>
    /// <param name="config">The config.</param>
    public Settings(
        IConfiguration config,
        IOptions<DmnSettings> dmnSettings)
    {
        _config = config;
        _dmnSettings = dmnSettings;
    }

    /// <summary>
    /// Gets the output path.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string OutputPath => _dmnSettings.Value.Paths.Output.NormalizePath();

    /// <summary>
    /// Gets the input path.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public string InputPath => _dmnSettings.Value.Paths.Input.NormalizePath();

    /// <summary>
    /// full path of asset file from its name
    /// </summary>
    /// <param name="filename">asset filename</param>
    /// <returns>asset full path</returns>
    public string AssetPath(string filename) =>
        Path.GetFullPath(
            Path.Combine(
                Directory.GetCurrentDirectory(),
                _dmnSettings.Value.Paths.Assets,
                filename));
}
