namespace MovieDbAssistant.Lib.Extensions;

/// <summary>
/// The file extensions.
/// </summary>
public static class FileExtensions
{
    /// <summary>
    /// Checks if is newer file
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>A <see cref="bool"/></returns>
    public static bool IsNewerFile(this string left,string right)
        => !File.Exists(right)
            || File.Exists(left)
                && new FileInfo(left).LastWriteTimeUtc >
                    new FileInfo(right).LastWriteTimeUtc;
}
