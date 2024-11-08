using MovieDbAssistant.Lib.Components.Logger;

namespace MovieDbAssistant.Lib.Components.Sys;

/// <summary>
/// The environnement.
/// </summary>
public static class Env
{
    /// <summary>
    /// app data folder
    /// </summary>
    public static string AppDataFolder { get; set; } = "";

    /// <summary>
    /// initialize the app env
    /// <para>initialize app / user data at first launch</para>
    /// </summary>
    /// <param name="appDataFolder">The app data folder.</param>
    public static void Init(string appDataFolder)
    {
        AppDataFolder = appDataFolder;
        var audp = AppUserDataPath;
        if (!Directory.Exists(audp))
            Directory.CreateDirectory(audp);
        AppLogger.EnsureLogFileExists();
    }

    /// <summary>
    /// Get app user data path.
    /// </summary>
    /// <returns>A <see cref="string"/></returns>
    public static string AppUserDataPath
        => Path.Combine(
            Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData),
                AppDataFolder);
}
