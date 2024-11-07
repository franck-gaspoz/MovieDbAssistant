using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Logger;

namespace MovieDbAssistant.App.Services.Tray;

/// <summary>
/// The tray application.
/// </summary>
[Singleton]
public sealed class TrayApplication : ApplicationContext
{
    public TrayApplication(
        IServiceProvider serviceProvider,
        ILogger<TrayApplication> logger)
    {
        logger.LogInformation(this,"logging to file: " + AppLogger.GetLogFilePath());

        serviceProvider
            .GetRequiredService<TrayMenuBuilder>()
            .Build();

        serviceProvider
            .GetRequiredService<TrayMenuService>()
            .ShowBalloonTip_Start();
    }
}
