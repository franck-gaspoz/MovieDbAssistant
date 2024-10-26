using Microsoft.Extensions.Logging;

using MovieDbAssistant.Lib.Components.Logger;

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
    public static bool IsNewestFile(this string left,string right)
        => !File.Exists(right)
            || File.Exists(left)
                && new FileInfo(left).LastWriteTimeUtc >
                    new FileInfo(right).LastWriteTimeUtc;

    /// <summary>
    /// Copy the directory.
    /// </summary>
    /// <param name="sourceDir">The source dir.</param>
    /// <param name="destinationDir">The destination dir.</param>
    /// <param name="preserveNewest">preserve newest files when target already exists</param>
    /// <param name="logger">logger</param>
    public static void CopyDirectory(
        this string sourceDir, 
        string destinationDir,
        bool preserveNewest = true,
        ILogger? logger = null,
        Func<string,string,bool>? fileCopyPreHandler = null)
    {
        // Create the destination directory if it doesn't exist
        if (!Directory.Exists(destinationDir))
        {
            Directory.CreateDirectory(destinationDir);
            logger?.LogInformation(
                logger,
                "folder created: " + destinationDir);
        }

        // Copy all files
        foreach (var file in Directory.GetFiles(sourceDir))
        {
            var destFile = Path.Combine(destinationDir, Path.GetFileName(file));
                                   
            if (
                (fileCopyPreHandler==null || !fileCopyPreHandler(file, destFile))
                && (!preserveNewest || file.IsNewestFile(destFile)))
            {
                File.Copy(file, destFile, true);
                logger?.LogInformation(
                    logger,
                    "file copied: " + destFile);
            }
        }

        // Copy all subdirectories
        foreach (var directory in Directory.GetDirectories(sourceDir))
        {
            var dir = Path.GetFileName(directory);
            var destDir = Path.Combine(destinationDir, dir);
            directory.CopyDirectory(destDir,preserveNewest,logger,fileCopyPreHandler);            
        }

        logger?.LogInformation(
            logger,
            "folder copied: " + destinationDir);
    }
}
