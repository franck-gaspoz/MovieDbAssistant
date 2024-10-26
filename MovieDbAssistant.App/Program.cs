﻿using Microsoft.Extensions.DependencyInjection;
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
    /// <summary>
    /// main
    /// </summary>
    /// <param name="args">The args.</param>
    [STAThread]
    public static void Main(string[] args)
    {
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

        Application.Run(
            host.Services
                .GetRequiredService<TrayApplication>());

        host.Run();
    }
}
