using IMDBAssistant.Lib.Components.DependencyInjection.Attributes;

using Microsoft.Extensions.DependencyInjection;

namespace IMDBAssistant.App.Services.Tray;

/// <summary>
/// The tray application.
/// </summary>
[Singleton]
public sealed class TrayApplication : ApplicationContext
{
    public TrayApplication(IServiceProvider serviceProvider)
    {
        serviceProvider
            .GetRequiredService<TrayMenuBuilder>()
            .Build();

        serviceProvider
            .GetRequiredService<TrayMenuService>()
            .ShowBalloonTip_Start();
    }
}
