using IMDBAssistant.App.Services.Tray;
using IMDBAssistant.Dmn.Components;
using IMDBAssistant.Lib.Components.DependencyInjection;
using IMDBAssistant.Lib.Components.DependencyInjection.Attributes;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IMDBAssistant;

/// <summary>
/// The program.
/// </summary>
public class Program
{
    /// <summary>
    /// main
    /// </summary>
    /// <param name="args">The args.</param>
    [STAThread]
    public static void Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
        .ConfigureServices(services => services
            .AutoRegister(typeof(SingletonAttribute))
            .AutoRegister(typeof(Settings))
            .AutoRegister(typeof(Program))
        )
        .Build();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(
                host.Services
                    .GetRequiredService<TrayApplication>());

        host.Run();
    }
}
