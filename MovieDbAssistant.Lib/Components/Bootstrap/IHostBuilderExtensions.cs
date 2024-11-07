using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace MovieDbAssistant.Lib.Components.Bootstrap;

/// <summary>
/// The I host builder extensions.
/// </summary>
public static class IHostBuilderExtensions
{
    public const string Filename_Pattern_App_Settings_Devlopment = "appsettings.Development.json";
    public const string Filename_Pattern_App_Settings = "appsettings.{0}.json";

    /// <summary>
    /// Add localized settings.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns>An <see cref="IHostBuilder"/></returns>
    public static IHostBuilder AddLocalizedSettings(
        this IHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(conf =>
        {
            var currentCulture = Thread.CurrentThread.CurrentCulture.Name;
            var t = currentCulture.Split('-');
            var file1 = string.Format(Filename_Pattern_App_Settings, currentCulture);
            var file2 = string.Format(Filename_Pattern_App_Settings, t[0]);
            if (File.Exists(file2)) conf.AddJsonFile(file2);
            if (File.Exists(file1)) conf.AddJsonFile(file1);
        });
        return builder;
    }

    /// <summary>
    /// onfigure app env settings
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns>An <see cref="IHostBuilder"/></returns>
    public static IHostBuilder AddEnvironmentSettings(
        this IHostBuilder builder)
    {        
        builder.ConfigureAppConfiguration(conf =>
        {
#if DEBUG
            if (File.Exists(Filename_Pattern_App_Settings_Devlopment))
                conf.AddJsonFile(Filename_Pattern_App_Settings_Devlopment);
#endif
        });
        return builder;
    }
}
