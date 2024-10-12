using MovieDbAssistant.Lib.Components.Extensions;

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
}
