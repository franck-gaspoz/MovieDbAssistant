using MovieDbAssistant.App.Services.Build;
using MovieDbAssistant.Dmn.Events;
using MovieDbAssistant.Lib.Components.DependencyInjection.Attributes;
using MovieDbAssistant.Lib.Components.Signal;

namespace MovieDbAssistant.App.Services.Tray;

/// <summary>
/// menu item states setter
/// </summary>
[Singleton]
sealed class TrayMenuItemsStates :
    ISignalHandler<BuildEndedEvent>
{
    readonly TrayMenuItems _trayMenuItems;

    public TrayMenuItemsStates(TrayMenuItems trayMenuItems)
        => _trayMenuItems = trayMenuItems;

    public void Handle(object sender, BuildEndedEvent @event) => _trayMenuItems.SetBuildItemsEnabled(true && !BuiIdInputFolderService.Buzy);
}
