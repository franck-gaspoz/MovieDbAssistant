using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using MovieDbAssistant.App.Services.Tray;
using MovieDbAssistant.Dmn.Components;
using MovieDbAssistant.Lib.Components.DependencyInjection;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;

namespace MovieDbAssistant;

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
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(
                typeof(Program).Assembly))
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
