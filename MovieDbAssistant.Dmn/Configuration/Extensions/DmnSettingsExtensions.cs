using MovieDbAssistant.Lib.Extensions;

namespace MovieDbAssistant.Dmn.Configuration.Extensions;

/// <summary>
/// The dmn settings extensions.
/// </summary>
public static class DmnSettingsExtensions
{
    /// <summary>
    /// Gets the output path.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public static string OutputPath(this DmnSettings dmnSettings)
        => dmnSettings.Paths.Output.NormalizePath();

    /// <summary>
    /// Gets the input path.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public static string InputPath(this DmnSettings dmnSettings)
        => dmnSettings.Paths.Input.NormalizePath();

    /// <summary>
    /// full path of asset file from its name
    /// </summary>
    /// <param name="filename">asset filename</param>
    /// <returns>asset full path</returns>
    public static string AssetPath(
        this DmnSettings dmnSettings,
        string filename) =>
        Path.GetFullPath(
            Path.Combine(
                Directory.GetCurrentDirectory(),
                dmnSettings.Paths.Assets,
                filename));

    /// <summary>
    /// path of engin tpl parts
    /// </summary>
    /// <param name="dmnSettings">dmn settings</param>
    /// <returns>path</returns>
    public static string EngineTplPath(
        this DmnSettings dmnSettings)
        => Path.Combine(
            Directory.GetCurrentDirectory(),
            dmnSettings.Paths.Resources,
            dmnSettings.Paths.RscHtml,
            dmnSettings.Paths.RscHtmlAssets,
            dmnSettings.Paths.RscHtmlAssetsTpl
            );
}