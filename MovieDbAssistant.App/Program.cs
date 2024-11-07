using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using MovieDbAssistant.App.Configuration;
using MovieDbAssistant.App.Services.Tray;
using MovieDbAssistant.Dmn.Configuration;
using MovieDbAssistant.Lib.Components.Bootstrap;
using MovieDbAssistant.Lib.Components.DependencyInjection;
using MovieDbAssistant.Lib.Components.Logger;
using MovieDbAssistant.Lib.Components.Signal;

namespace MovieDbAssistant;

/// <summary>
/// The program.
/// </summary>
public class Program
{
    public const string LogFolder = "logs";
    public const string LogFile = "log.txt";
    public const string PackageFolder = "package";
    public const int EXIT_OK = 0;

    /// <summary>
    /// Gets the log path.
    /// </summary>
    /// <value>A <see cref="string"/></value>
    public static string LogPath =>
        Path.Combine(
            System.AppContext.BaseDirectory!,
            LogFolder,
            LogFile);

    /// <summary>
    /// main
    /// </summary>
    /// <param name="args">The args.</param>
    [STAThread]
    public static async Task<int> Main(string[] args)
    {
        // setup base directory
        // msix: C:\Program Files\WindowsApps\FranckGaspoz.Software.MovieDbAssistant_1.0.0.0_x64__xtrrbsjxvn07w
        // inno setup: TODO: fix and test
        // dev: /bin/...
        var basePath = System.AppContext.BaseDirectory;
        if (basePath.Contains(PackageFolder))            
            basePath = basePath.Replace(PackageFolder, "");
        Directory.SetCurrentDirectory(basePath);

        Type[] fromTypes =
            [typeof(AppLogger),
            typeof(DmnSettings),
            typeof(AppSettings)];

        var host = Host.CreateDefaultBuilder(args)
            .AddLocalizedSettings()
            .ConfigureServices((context, services) => services
                .AutoRegister(fromTypes)
                .AddSignalR(fromTypes)
                .Configure<DmnSettings>(context.Configuration)
                .Configure<AppSettings>(context.Configuration))
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddAppLogger();
            })
            .Build();

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.SetHighDpiMode(HighDpiMode.DpiUnawareGdiScaled);

        Application.Run(
            host.Services
                .GetRequiredService<TrayApplication>());

        await host.RunAsync();
        return EXIT_OK;
    }
}
