using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using IMDBAssistant.Lib.Components.DependencyInjection.Attributes;
using IMDBAssistant.App.Services.Tray;

Console.WriteLine("system tray");

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
        services.
            AutoRegister(typeof(Program))
    )
    .Build();

Application.EnableVisualStyles();
Application.SetCompatibleTextRenderingDefault(false);

var ac = host.Services
    .GetRequiredService<TrayMenuBuilder>()
    .Build();

host.Services
    .GetRequiredService<TrayMenuService>()
    .ShowBalloonTip_Start();

Application.Run(
    host.Services
        .GetRequiredService<TrayApplication>());

host.Run();
