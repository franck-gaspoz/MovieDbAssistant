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

    /// <summary>
    /// Copy the directory.
    /// </summary>
    /// <param name="sourceDir">The source dir.</param>
    /// <param name="destinationDir">The destination dir.</param>
    public static void CopyDirectory(this string sourceDir, string destinationDir, string subPath = "")
    {
        // Create the destination directory if it doesn't exist
        Directory.CreateDirectory(destinationDir);
        
        if (!Directory.Exists(destinationDir))
            Directory.CreateDirectory(destinationDir);

        // Copy all files
        foreach (string file in Directory.GetFiles(sourceDir))
        {
            string destFile = Path.Combine(destinationDir, Path.GetFileName(file));
            File.Copy(file, destFile, true);
        }

        // Copy all subdirectories
        foreach (string directory in Directory.GetDirectories(sourceDir))
        {
            var dir = Path.GetFileName(directory);
            string destDir = Path.Combine(destinationDir, dir);
            CopyDirectory(directory, destDir, dir);
        }
    }
}
