using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using IMDBAssistant.Lib.Components.DependencyInjection.Attributes;
using IMDBAssistant.App.Services.Tray;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
        services.AutoRegister(typeof(Program))
    )
    .Build();

Application.EnableVisualStyles();
Application.SetCompatibleTextRenderingDefault(false);
Application.Run(
    host.Services
        .GetRequiredService<TrayApplication>());

host.Run();
