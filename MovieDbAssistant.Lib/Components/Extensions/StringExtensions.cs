using System.Text;

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
    public static string ToHexString(this string? value)
    {
        if (value == null) return string.Empty;
        var bytes = System.Text.Encoding.UTF8.GetBytes(value);
        var base64String = Convert.ToBase64String(bytes);
        return base64String;
    }

    /// <summary>
    /// Converts to hex having only letters and digits
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>A <see cref="string"/></returns>
    public static string ToHexLettersAndDigitsString(this string? value)
    {
        if (value == null) return string.Empty;
        var bytes = System.Text.Encoding.UTF8.GetBytes(value);
        var base64String = Convert.ToBase64String(bytes);
        StringBuilder sb = new();
        foreach (var c in base64String)
            if (char.IsAsciiLetterOrDigit(c))
                sb.Append(c);
        return sb.ToString();
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

    /// <summary>
    /// first letter in lower case
    /// </summary>
    /// <param name="s">string</param>
    /// <returns>first letter in lower case</returns>
    public static string ToFirstLower(this string s)
        => s[0].ToString().ToLower() + s[1..];

    /// <summary>
    /// first letter in upper case
    /// </summary>
    /// <param name="s">string</param>
    /// <returns>first letter in upper case</returns>
    public static string ToFirstUpper(this string s)
        => s[0].ToString().ToUpper() + s[1..];

    /// <summary>
    /// double quote a string
    /// </summary>
    /// <param name="s">string</param>
    /// <returns>double quoted string</returns>
    public static string DblQuote(this string s)
        => '"' + s + '"';
}
