namespace MovieDbAssistant.Lib.Components.Extensions;

/// <summary>
/// string extensions
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Converts to hex string.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>A <see cref="string"/></returns>
    public static string ToHexString(this string value)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(value);
        var base64String = Convert.ToBase64String(bytes);
        return base64String;
    }

    /// <summary>
    /// normalize a path -&gt; set to absolute
    /// </summary>
    /// <param name="path">path</param>
    /// <returns>path</returns>
    public static string NormalizePath(this string path)
    {
        if (!Path.IsPathFullyQualified(path))
            path = Path.Combine(
                Directory.GetCurrentDirectory(),
                path);
        return path;
    }
}
