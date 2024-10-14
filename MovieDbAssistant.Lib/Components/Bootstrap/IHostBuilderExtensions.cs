using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace MovieDbAssistant.Lib.Components.Bootstrap;

/// <summary>
/// The I service collection extensions.
/// </summary>
public static class IHostBuilderExtensions
{
    public const string Filename_Pattern_App_Settings = "appsettings.{0}.json";

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
}
